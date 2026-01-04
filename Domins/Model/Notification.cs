public class Notification
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; } = NotificationType.Info;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? UserId { get; set; }
    public bool IsRead { get; set; } = false;
    public bool IsDelivered { get; set; } = false;
    public DateTime? DeliveredAt {get; set; }
    public DateTime? ReadAt { get; set; }
}