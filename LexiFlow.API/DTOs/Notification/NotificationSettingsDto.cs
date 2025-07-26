namespace LexiFlow.API.DTOs.Notification
{
    public class NotificationSettingsDto
    {
        public int UserID { get; set; }
        public bool EnableEmailNotifications { get; set; }
        public bool EnablePushNotifications { get; set; }
        public bool EnableInAppNotifications { get; set; }
        public Dictionary<string, NotificationTypeSettingDto> TypeSettings { get; set; } = new Dictionary<string, NotificationTypeSettingDto>();
    }
}
