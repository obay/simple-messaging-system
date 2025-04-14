namespace simple_messaging_system.Models
{
    public class MessageDto
    {
        public required string To { get; set; }
        public required string From { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public int? ParentMessageId { get; set; }
    }
}
