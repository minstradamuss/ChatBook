using System.Collections.Generic;
using ChatBook.Entities;

namespace ChatBook.Domain.Interfaces
{
    public interface IMessageRepository
    {
        List<Message> GetMessages(string senderNickname, string receiverNickname);
        void SaveMessage(Message message);
        List<User> GetChatPartners(string nickname);
    }
}
