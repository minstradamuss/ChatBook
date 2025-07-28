using ChatBook.Entities;

namespace ChatBook.Models
{
    public static class UserMapper
    {
        public static UserModel ToModel(User user) => new UserModel
        {
            Nickname = user.Nickname,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Avatar = user.Avatar,
            PhoneNumber = user.PhoneNumber
        };

        public static User ToEntity(UserModel model)
        {
            return new User
            {
                Nickname = model.Nickname,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Avatar = model.Avatar
            };
        }
    }
}
