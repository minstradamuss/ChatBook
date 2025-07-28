using System.Collections.Generic;
using ChatService.Entities;

namespace ChatService.Domain
{
    public interface IChatRepository
    {
        void SaveMessage(Message message);
        List<Message> GetMessagesBetween(string user1, string user2);
    }
}