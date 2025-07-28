using ChatService.Domain;
using ChatService.Entities;
using ChatService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatService.ConsoleApp
{
    public class InMemoryChatRepository : IChatRepository
    {
        private readonly List<Message> _messages = new List<Message>();

        public void SaveMessage(Message message)
        {
            message.Id = _messages.Count + 1;
            message.Timestamp = DateTime.Now;
            _messages.Add(message);
        }

        public List<Message> GetMessagesBetween(string user1, string user2)
        {
            return _messages
                .Where(m =>
                    (m.FromUser == user1 && m.ToUser == user2) ||
                    (m.FromUser == user2 && m.ToUser == user1))
                .OrderBy(m => m.Timestamp)
                .ToList();
        }
    }
}
