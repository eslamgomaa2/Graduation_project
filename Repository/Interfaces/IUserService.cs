// IUserService.cs
using Domins.Dtos.Dto;

namespace Domains.Services
{
    public interface IUserService
    {
        Task<bool> UserExistsAsync(string userId);
        Task<bool> IsUserBlockedAsync(string userId, string blockedByUserId);
        Task<UserDto?> GetUserByIdAsync(string userId);
        Task UpdateUserOnlineStatusAsync(string userId, bool isOnline);
        Task UpdateUserLastSeenAsync(string userId);
    }
}