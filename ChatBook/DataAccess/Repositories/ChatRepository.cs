using ChatService.Domain;
using ChatService.Entities;
using ChatBook.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace ChatBook.DataAccess.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SaveMessage(Message message)
        {
            var sender = _context.Users.FirstOrDefault(u => u.Nickname == message.FromUser);
            var receiver = _context.Users.FirstOrDefault(u => u.Nickname == message.ToUser);

            if (sender == null || receiver == null) return;

            var dbMessage = new ChatBook.Entities.Message
            {
                SenderId = sender.Id,
                ReceiverId = receiver.Id,
                Content = message.Text,
                Timestamp = message.Timestamp
            };

            _context.Messages.Add(dbMessage);
            _context.SaveChanges();
        }

        public List<Message> GetMessagesBetween(string user1, string user2)
        {
            var sender = _context.Users.FirstOrDefault(u => u.Nickname == user1);
            var receiver = _context.Users.FirstOrDefault(u => u.Nickname == user2);

            if (sender == null || receiver == null) return new List<Message>();

            return _context.Messages
                .Where(m =>
                    (m.SenderId == sender.Id && m.ReceiverId == receiver.Id) ||
                    (m.SenderId == receiver.Id && m.ReceiverId == sender.Id))
                .OrderBy(m => m.Timestamp)
                .Select(m => new Message
                {
                    Id = m.Id,
                    FromUser = m.Sender.Nickname,
                    ToUser = m.Receiver.Nickname,
                    Text = m.Content,
                    Timestamp = m.Timestamp
                })
                .ToList();
        }
    }
}
