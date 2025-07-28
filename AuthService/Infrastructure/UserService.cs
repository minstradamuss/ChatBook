using AuthService.Domain;
using AuthService.Models;

namespace AuthService.Infrastructure
{
    public class UserService : IUserService
    {
        private static readonly List<UserDto> _users = new();

        public UserDto? Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public bool Register(string username, string password)
        {
            if (_users.Any(u => u.Username == username))
                return false;

            _users.Add(new UserDto { Username = username, Password = password });
            return true;
        }
    }
}
