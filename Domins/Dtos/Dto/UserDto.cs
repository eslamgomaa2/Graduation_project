namespace Domins.Dtos.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? LastSeen { get; set; }
        public string? Bio { get; set; }
    }
}