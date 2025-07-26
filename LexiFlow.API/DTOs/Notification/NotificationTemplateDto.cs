namespace LexiFlow.API.DTOs.Notification
{
    public class NotificationTemplateDto
    {
        public int TemplateID { get; set; }
        public string TemplateName { get; set; }
        public string NotificationType { get; set; }
        public string TitleTemplate { get; set; }
        public string MessageTemplate { get; set; }
        public string DefaultPriority { get; set; }
        public string DefaultIconClass { get; set; }
        public bool IsActive { get; set; }
    }
}
