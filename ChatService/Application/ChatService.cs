using ChatService.Domain;
using ChatService.Entities;
using System;
using System.Collections.Generic;

namespace ChatService.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _repo;

        public ChatService(IChatRepository repo)
        {
            _repo = repo;
        }

        public void SendMessage(string fromUser, string toUser, string text)
        {
            _repo.SaveMessage(new Message
            {
                FromUser = fromUser,
                ToUser = toUser,
                Text = text,
                Timestamp = DateTime.Now
            });
        }

        public List<Message> GetChatHistory(string user1, string user2)
        {
            return _repo.GetMessagesBetween(user1, user2);
        }
    }
}
