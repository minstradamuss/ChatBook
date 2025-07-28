using System;
using System.ComponentModel.DataAnnotations;

namespace ChatBook.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int ReceiverId { get; set; }
        public User Receiver { get; set; }

        public string Content { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
