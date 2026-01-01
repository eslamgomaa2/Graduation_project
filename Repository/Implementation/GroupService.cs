// GroupService.cs
using Domains.Services;
using Domins.Dtos.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository;

namespace Graduation_Project.Services
{
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbcontext _context;
        private readonly ILogger<GroupService> _logger;

        public GroupService(ApplicationDbcontext context, ILogger<GroupService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> GroupExistsAsync(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                return false;

            return await _context.Groups
                .AnyAsync(g => g.Name == groupName);
        }

        public async Task<bool> IsUserInGroupAsync(string userId, string groupName)
        {
            return await _context.GroupMembers
                .AnyAsync(gm => gm.UserId == userId && gm.Group.Name == groupName);
        }

        public async Task<GroupDto?> GetGroupByNameAsync(string groupName)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Name == groupName);

            if (group == null)
                return null;

            return new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                MemberCount = group.Members.Count
                // Map other properties
            };
        }

        public async Task<List<string>> GetGroupMembersAsync(string groupName)
        {
            return await _context.GroupMembers
                .Where(gm => gm.Group.Name == groupName)
                .Select(gm => gm.UserId)
                .ToListAsync();
        }
    }
}