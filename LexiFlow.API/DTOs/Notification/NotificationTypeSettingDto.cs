namespace LexiFlow.API.DTOs.Notification
{
    public class NotificationTypeSettingDto
    {
        public string NotificationType { get; set; }
        public bool IsEnabled { get; set; }
        public string DeliveryMethod { get; set; }
        public bool MuteAfterHours { get; set; }
    }
}
