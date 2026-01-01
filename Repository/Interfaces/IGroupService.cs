
using Domins.Dtos.Dto;

namespace Domains.Services
{
    public interface IGroupService
    {
        Task<bool> GroupExistsAsync(string groupName);
        Task<bool> IsUserInGroupAsync(string userId, string groupName);
        Task<GroupDto?> GetGroupByNameAsync(string groupName);
        Task<List<string>> GetGroupMembersAsync(string groupName);
    }
}