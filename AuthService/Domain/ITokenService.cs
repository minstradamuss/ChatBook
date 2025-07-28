namespace AuthService.Domain
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}
