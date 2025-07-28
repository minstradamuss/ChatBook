using AuthService.Models;

namespace AuthService.Domain
{
    public interface IUserService
    {
        UserDto? Authenticate(string username, string password);
        bool Register(string username, string password);
    }
}
