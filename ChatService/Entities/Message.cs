using System;

namespace ChatService.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
