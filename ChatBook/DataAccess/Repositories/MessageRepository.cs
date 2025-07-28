using ChatBook.Domain.Interfaces;
using ChatBook.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ChatBook.DataAccess.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Message> GetMessages(string senderNickname, string receiverNickname)
        {
            var sender = _context.Users.FirstOrDefault(u => u.Nickname == senderNickname);
            var receiver = _context.Users.FirstOrDefault(u => u.Nickname == receiverNickname);

            if (sender == null || receiver == null) return new List<Message>();

            return _context.Messages
                .Where(m => (m.SenderId == sender.Id && m.ReceiverId == receiver.Id) ||
                            (m.SenderId == receiver.Id && m.ReceiverId == sender.Id))
                .OrderBy(m => m.Timestamp)
                .ToList();
        }

        public void SaveMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
        }

        public List<User> GetChatPartners(string nickname)
        {
            var user = _context.Users.FirstOrDefault(u => u.Nickname == nickname);
            if (user == null) return new List<User>();

            return _context.Messages
                .Where(m => m.SenderId == user.Id || m.ReceiverId == user.Id)
                .Select(m => m.SenderId == user.Id ? m.Receiver : m.Sender)
                .Distinct()
                .ToList();
        }
    }
}
