using ChatBook.Domain.Services;
using ChatBook.Entities;
using System.Collections.Generic;

namespace ChatBook.ViewModels
{
    public class MainViewModel
    {
        private readonly UserService _userService;

        public MainViewModel(UserService userService)
        {
            _userService = userService;
        }

        public List<Book> GetUserBooks(string nickname)
        {
            return _userService.GetUserBooks(nickname);
        }

        public User GetUser(string nickname)
        {
            return _userService.GetUserByNickname(nickname);
        }

        public bool RemoveFriend(string userNickname, string friendNickname)
        {
            return _userService.RemoveFriend(userNickname, friendNickname);
        }

        public bool UpdateProfile(User user)
        {
            if (user == null)
                return false;

            return _userService.UpdateProfile(user);
        }
    }
}
