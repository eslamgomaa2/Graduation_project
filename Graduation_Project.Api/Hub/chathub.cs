
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Collections.Concurrent;
using Domains.Services;


namespace Domains.Helper
{
    [Authorize] 
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;

        private static readonly ConcurrentDictionary<string, HashSet<string>> _userConnections = new();

        private static readonly ConcurrentDictionary<string, HashSet<string>> _connectionGroups = new();

        public ChatHub(
            ILogger<ChatHub> logger,
            IUserService userService,
            IGroupService groupService)
        {
            _logger = logger;
            _userService = userService;
            _groupService = groupService;
        }

        #region Connection Management

        public override async Task OnConnectedAsync()
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = Context.User?.Identity?.Name ?? "Unknown";

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User connected without valid user ID");
                    await base.OnConnectedAsync();
                    return;
                }

                
                _userConnections.AddOrUpdate(
                    userId,
                    new HashSet<string> { Context.ConnectionId },
                    (key, existing) =>
                    {
                        existing.Add(Context.ConnectionId);
                        return existing;
                    });

                
                await _userService.UpdateUserOnlineStatusAsync(userId, true);

                _logger.LogInformation(
                    "User {UserId} ({UserName}) connected with ConnectionId {ConnectionId}",
                    userId, userName, Context.ConnectionId);

                // Notify user's contacts that they're online
                await Clients.Others.SendAsync("UserOnline", userId, userName);

                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnConnectedAsync for ConnectionId {ConnectionId}",
                    Context.ConnectionId);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = Context.User?.Identity?.Name ?? "Unknown";

                if (!string.IsNullOrEmpty(userId))
                {
                    if (_userConnections.TryGetValue(userId, out var connections))
                    {
                        connections.Remove(Context.ConnectionId);

                        if (connections.Count == 0)
                        {
                            _userConnections.TryRemove(userId, out _);
                            await _userService.UpdateUserOnlineStatusAsync(userId, false);

                            
                            await Clients.Others.SendAsync("UserOffline", userId, userName);
                        }
                    }

                   
                  

                    _logger.LogInformation(
                        "User {UserId} ({UserName}) disconnected. ConnectionId: {ConnectionId}. Reason: {Reason}",
                        userId, userName, Context.ConnectionId, exception?.Message ?? "Normal disconnect");
                }

                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnDisconnectedAsync for ConnectionId {ConnectionId}",
                    Context.ConnectionId);
            }
        }

        #endregion

        #region Group/Room Management

        public async Task JoinRoom(string roomName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roomName))
                {
                    await Clients.Caller.SendAsync("Error", "Room name cannot be empty");
                    return;
                }

                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = Context.User?.Identity?.Name ?? "Unknown";

                if (string.IsNullOrEmpty(userId))
                {
                    await Clients.Caller.SendAsync("Error", "User not authenticated");
                    return;
                }

                var groupExists = await _groupService.GroupExistsAsync(roomName);
                if (!groupExists)
                {
                    await Clients.Caller.SendAsync("Error", $"Group '{roomName}' does not exist");
                    _logger.LogWarning("User {UserId} tried to join non-existent group {GroupName}",
                        userId, roomName);
                    return;
                }

                var isMember = await _groupService.IsUserInGroupAsync(userId, roomName);
                if (!isMember)
                {
                    await Clients.Caller.SendAsync("Error", "You are not a member of this group");
                    _logger.LogWarning("User {UserId} tried to join group {GroupName} without membership",
                        userId, roomName);
                    return;
                }

                
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

               
                if (_connectionGroups.TryGetValue(Context.ConnectionId, out var groups))
                {
                    groups.Add(roomName);
                }

                _logger.LogInformation("User {UserId} joined room {RoomName}", userId, roomName);

               
                await Clients.Group(roomName).SendAsync(
                    "UserJoinedRoom",
                    roomName,
                    userId,
                    userName,
                    DateTime.UtcNow); 

                await Clients.Caller.SendAsync("JoinedRoom", roomName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error joining room {RoomName}", roomName);
                await Clients.Caller.SendAsync("Error", "Failed to join room");
            }
        }

        public async Task LeaveRoom(string roomName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roomName))
                {
                    await Clients.Caller.SendAsync("Error", "Room name cannot be empty");
                    return;
                }

                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = Context.User?.Identity?.Name ?? "Unknown";

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);

                
                if (_connectionGroups.TryGetValue(Context.ConnectionId, out var groups))
                {
                    groups.Remove(roomName);
                }

                _logger.LogInformation("User {UserId} left room {RoomName}", userId, roomName);


                await Clients.Group(roomName).SendAsync(
                    "UserLeftRoom",
                    roomName,
                    userId,
                    userName,
                    DateTime.UtcNow);
                
                await Clients.Caller.SendAsync("LeftRoom", roomName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error leaving room {RoomName}", roomName);
                await Clients.Caller.SendAsync("Error", "Failed to leave room");
            }
        }

        #endregion

        #region Messaging Features

        public async Task SendTypingIndicator(string recipientId)
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = Context.User?.Identity?.Name ?? "Unknown";

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(recipientId))
                    return;

                await Clients.User(recipientId).SendAsync("UserTyping", userId, userName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending typing indicator");
            }
        }

        public async Task SendTypingIndicatorToGroup(string groupName)
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = Context.User?.Identity?.Name ?? "Unknown";

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(groupName))
                    return;

                await Clients.OthersInGroup(groupName).SendAsync("UserTypingInGroup", groupName, userId, userName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending typing indicator to group");
            }
        }

        public async Task StopTypingIndicator(string recipientId)
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(recipientId))
                    return;

                await Clients.User(recipientId).SendAsync("UserStoppedTyping", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending stop typing indicator");
            }
        }

        public async Task MarkMessageAsRead(int messageId, string senderId)
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return;

                await Clients.User(senderId).SendAsync("MessageRead", messageId, userId, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking message as read");
            }
        }

        #endregion

        #region Utility Methods

        public async Task GetOnlineUsers()
        {
            try
            {
                var onlineUserIds = _userConnections.Keys.ToList();
                await Clients.Caller.SendAsync("OnlineUsers", onlineUserIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting online users");
            }
        }

        public async Task GetRoomMembers(string roomName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roomName))
                    return;

                var members = await _groupService.GetGroupMembersAsync(roomName);
                await Clients.Caller.SendAsync("RoomMembers", roomName, members);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting room members");
            }
        }

        public static bool IsUserOnline(string userId)
        {
            return _userConnections.ContainsKey(userId);
        }

        public static int GetConnectionCount(string userId)
        {
            return _userConnections.TryGetValue(userId, out var connections)
                ? connections.Count
                : 0;
        }

        #endregion
    }
}