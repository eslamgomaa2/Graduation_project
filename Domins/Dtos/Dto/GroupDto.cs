namespace Domins.Dtos.Dto
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int MemberCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPrivate { get; set; }
        public string CreatedByUserId { get; set; }
        public string? CreatedByUsername { get; set; }
        public bool IsActive { get; set; }
        public int MaxMembers { get; set; }
    }
}