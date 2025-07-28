using System.Data.Entity;
using ChatBook.DataAccess;
using ChatBook.Entities;

namespace ChatBook.DB
{
    public class DbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var user = new User
            {
                Nickname = "seeduser",
                FirstName = "Seed",
                LastName = "User",
                Password = "1234"
            };

            context.Users.Add(user);
            context.SaveChanges();


            base.Seed(context);
        }

    }
}
