using Domains.Dtos;
using Domains.Helper;
using Domains.Services;
using Domins.Dtos.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Graduation_Project.Api.Controllers
{
    [ApiController]
    [Route("api/chat")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<ChatController> _logger;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;    
        public ChatController(
            IHubContext<ChatHub> hubContext,
            ILogger<ChatController> logger,
            IUserService userService,
            IGroupService groupService)
        {
            _hubContext = hubContext;
            _logger = logger;
            _userService = userService;
            _groupService = groupService;
        }

        [HttpPost("send-to-client")]
        public async Task<IActionResult> SendPrivateMessage([FromBody] Privatemessagedto model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.Message))
                {
                    return BadRequest(new { error = "Message cannot be empty" });
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "User not authenticated";

                var receiverExists = await _userService.UserExistsAsync(model.ReceiverId);
                if (!receiverExists)
                {
                    return NotFound(new { error = "Receiver not found" });
                }
             
                var isBlocked = await _userService.IsUserBlockedAsync(userId, model.ReceiverId);
                if (isBlocked)
                {
                    return BadRequest(new { error = "You cannot send messages to this user" });
                }

                var username = User.Identity?.Name ?? "Unknown";
               

                await _hubContext.Clients
                    .User(model.ReceiverId)
                    .SendAsync("ReceiveMessage", username, model.Message, DateTime.UtcNow);

                await _hubContext.Clients
                    .User(userId)
                    .SendAsync("ReceiveMessage", "You", model.Message, DateTime.UtcNow);

                _logger.LogInformation(
                    "Private message sent from {SenderId} to {ReceiverId}",
                    userId, model.ReceiverId);

                return Ok(new
                {
                    success = true,
                    message = "Message sent successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending private message");
                return StatusCode(500, new { error = "Failed to send message" });
            }
        }

        [HttpPost("send-to-all")]
        public async Task<IActionResult> SendMessageToAll([FromBody] SendMessageDto model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.Message))
                {
                    return BadRequest(new { error = "Message cannot be empty" });
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

               

                var username = User.Identity?.Name ?? "Unknown";

                await _hubContext.Clients.All
                    .SendAsync("ReceiveMessage", username, model.Message, DateTime.UtcNow);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
                return StatusCode(500, new { error = "Failed to send message" });
            }
        }

        [HttpPost("send-to-group")]
        public async Task<IActionResult> SendMessageToGroup([FromBody] SendMessageDto model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.Message))
                {
                    return BadRequest(new { error = "Message cannot be empty" });
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var groupExists = await _groupService.GroupExistsAsync(model.GroupName);
                if (!groupExists)
                {
                    return NotFound(new { error = "Group not found" });
                }

                var isMember = await _groupService.IsUserInGroupAsync(userId, model.GroupName);
                if (!isMember)
                {
                    return Forbid("You are not a member of this group");
                }

                var username = User.Identity?.Name ?? "Unknown";

                await _hubContext.Clients
                    .Group(model.GroupName)
                    .SendAsync("ReceiveMessage", username, model.Message, DateTime.UtcNow);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending group message");
                return StatusCode(500, new { error = "Failed to send message" });
            }
        }

        
    }
}