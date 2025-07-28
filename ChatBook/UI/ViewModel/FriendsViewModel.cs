using ChatBook.Domain.Services;
using ChatBook.Entities;
using System.Collections.Generic;

namespace ChatBook.ViewModels
{
    public class FriendsViewModel
    {
        private readonly UserService _userService;

        public FriendsViewModel(UserService userService)
        {
            _userService = userService;
        }

        public List<User> GetFriends(string nickname)
        {
            return _userService.GetFriends(nickname);
        }

        public List<User> GetFollowers(string nickname)
        {
            return _userService.GetFollowers(nickname);
        }

        public List<User> SearchUsers(string query)
        {
            return _userService.SearchUsers(query);
        }

        public bool AddFriend(string from, string to)
        {
            return _userService.AddFriend(from, to);
        }

        public bool AreFriends(string user1, string user2)
        {
            return _userService.AreFriends(user1, user2);
        }
    }
}
