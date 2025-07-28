using ChatBook.Entities;

namespace ChatBook
{
    public static class AppSession
    {
        public static User LoggedUser { get; private set; }

        public static void SetLoggedUser(User user)
        {
            if (LoggedUser == null)
            {
                LoggedUser = user;
            }
        }

        public static string JwtToken { get; private set; }

        public static void SetJwtToken(string token)
        {
            JwtToken = token;
        }
    }
}
