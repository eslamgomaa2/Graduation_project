// UserService.cs
using Domains.Services;
using Domins.Dtos.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository;

namespace Graduation_Project.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbcontext _context; 
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbcontext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;

            return await _context.Users
                .AnyAsync(u => u.Id == userId);
        }

        public async Task<bool> IsUserBlockedAsync(string userId, string blockedByUserId)
        {
            
            return await _context.BlockedUsers
                .AnyAsync(b => b.BlockedUserId == userId && b.BlockedByUserId == blockedByUserId);
        }

       

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email
            };
        }

        public async Task UpdateUserOnlineStatusAsync(string userId, bool isOnline)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.IsOnline = isOnline;
                    user.LastSeen = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Updated online status for user {UserId}: {IsOnline}",
                        userId, isOnline);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating online status for user {UserId}", userId);
            }
        }

        public async Task UpdateUserLastSeenAsync(string userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.LastSeen = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating last seen for user {UserId}", userId);
            }
        }
    }
}