using ChatBook.Entities;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ChatBook.Models;
using ChatBook.Domain.Services;
using System;

namespace ChatBook.ViewModels
{
    public class LoginViewModel
    {
        private readonly UserService _userService;
        private readonly HttpClient _httpClient = new HttpClient();
        public static string JwtToken { get; private set; }
        public LoginViewModel(UserService userService)
        {
            _userService = userService;
        }

        public async Task<UserModel> LoginAsync(string nickname, string password)
        {
            var content = new StringContent(JsonSerializer.Serialize(new
            {
                Username = nickname,
                Password = password
            }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:52695/api/auth/login", content);

            var raw = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Ошибка логина: {response.StatusCode}, ответ: {raw}");
                return null;
            }

            var payload = JsonSerializer.Deserialize<LoginResponse>(raw, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            JwtToken = payload?.Token;
            AppSession.SetJwtToken(JwtToken);

            if (payload is null || string.IsNullOrEmpty(payload.Token))
            {
                Console.WriteLine("Ответ без токена.");
                return null;
            }

            var existingUser = _userService.GetUserByNickname(nickname);
            if (existingUser == null)
            {
                var newUser = new User { Nickname = nickname, Password = password };
                _userService.Register(newUser);
                existingUser = _userService.GetUserByNickname(nickname);
            }

            return existingUser != null ? UserMapper.ToModel(existingUser) : null;
        }

        public async Task<bool> RegisterAsync(string nickname, string password)
        {
            var content = new StringContent(JsonSerializer.Serialize(new
            {
                Username = nickname,
                Password = password
            }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:52695/api/auth/register", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Регистрация не удалась: {error}");
                return false;
            }

            var user = new User
            {
                Nickname = nickname,
                Password = password
            };

            return _userService.Register(user);
        }

    }
}
