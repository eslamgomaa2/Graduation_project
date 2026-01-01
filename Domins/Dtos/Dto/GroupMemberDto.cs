using Domains.Entities;

namespace Domins.Dtos.Dto
{
    public class GroupMemberDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public GroupRole Role { get; set; }
        public DateTime JoinedAt { get; set; }
        public bool IsMuted { get; set; }
        public bool IsOnline { get; set; }
    }
}