namespace AuthService.Domain
{
    public interface ITokenStrategy
    {
        string Generate(string username);
    }
}
