using AuthService.Domain;

namespace AuthService.Infrastructure
{
    public class TokenService : ITokenService
    {
        private readonly ITokenStrategy _strategy;

        public TokenService(ITokenStrategy strategy)
        {
            _strategy = strategy;
        }

        public string GenerateToken(string username)
        {
            return _strategy.Generate(username);
        }
    }
}
