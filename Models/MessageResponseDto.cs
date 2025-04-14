namespace simple_messaging_system.Models
{
    public class MessageResponseDto
    {
        public int Id { get; set; }
        public required string To { get; set; }
        public required string From { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public required bool IsRead { get; set; }
        public int? ParentMessageId { get; set; }
        public List<MessageResponseDto> ChildMessages { get; set; } = new List<MessageResponseDto>();
    }
}
