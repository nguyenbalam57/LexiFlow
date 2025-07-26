namespace LexiFlow.API.DTOs.Notification
{
    public class NotificationDto
    {
        public int NotificationID { get; set; }
        public int? UserID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Priority { get; set; }
        public bool IsRead { get; set; }
        public string ActionURL { get; set; }
        public string IconClass { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
