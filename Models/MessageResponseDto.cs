namespace simple_messaging_system.Models
{
    public class MessageResponseDto
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
    }
} 