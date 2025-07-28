using ChatBook.Entities;
using ChatBook.DataAccess;
using System.Linq;
using Xunit;

namespace ChatBook.Tests.Database
{
    public class DbTests
    {
        private readonly ApplicationDbContext _context;

        public DbTests()
        {
            _context = new ApplicationDbContext("name=TestDbContext");

            _context.Database.CreateIfNotExists();
        }

        [Fact]
        public void CanInsertUserIntoDatabase()
        {
            var user = new User { Nickname = "testuser", Password = "1234" };

            _context.Users.Add(user);
            _context.SaveChanges();

            var found = _context.Users.FirstOrDefault(u => u.Nickname == "testuser");
            Assert.NotNull(found);
            Assert.Equal("testuser", found.Nickname);
        }

        [Fact]
        public void CanAddBookToUser()
        {
            var user = _context.Users.FirstOrDefault(u => u.Nickname == "testuser");

            if (user == null)
            {
                user = new User { Nickname = "testuser", Password = "1234" };
                _context.Users.Add(user);
                _context.SaveChanges();
            }

            var book = new Book
            {
                Title = "Test Book",
                Author = "Author",
                UserId = user.Id,
                Status = "Прочитано"
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            var saved = _context.Books.FirstOrDefault(b => b.Title == "Test Book");
            Assert.NotNull(saved);
            Assert.Equal("Test Book", saved.Title);
            Assert.Equal(user.Id, saved.UserId);
        }
    }
}
