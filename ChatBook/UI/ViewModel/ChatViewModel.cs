using ChatService.Domain;
using ChatService.Entities;
using ChatBook.Entities;
using ChatBook.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatBook.ViewModels
{
    public class ChatViewModel
    {
        private readonly IChatService _chatService;
        private readonly UserService _userService;

        public ChatViewModel(IChatService chatService, UserService userService)
        {
            _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public List<User> GetFriends(string nickname)
        {
            return _userService.GetFriends(nickname);
        }

        public List<User> GetAllChatPartners(string nickname)
        {
            return _userService.GetAllChatPartners(nickname);
        }

        public void SendMessage(string from, string to, string text)
        {
            _chatService.SendMessage(from, to, text);
        }

        public List<ChatService.Entities.Message> GetChatMessages(string from, string to)
        {
            return _chatService.GetChatHistory(from, to);
        }
    }
}
