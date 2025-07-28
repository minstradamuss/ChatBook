using ChatBook.DataAccess;
using ChatBook.Entities;
using ChatBook.DataAccess.Repositories;
using System.Linq;
using Xunit;

namespace ChatBook.Tests.Database
{
    public class UserRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly UserRepository _repo;

        public UserRepositoryTests()
        {
            _context = new ApplicationDbContext("name=TestDbContext");
            _repo = new UserRepository(_context);

            _context.Database.CreateIfNotExists();
        }

        [Fact]
        public void DeleteUser_ShouldRemoveUserAndRelatedData()
        {
            var user = new User { Nickname = "delete_me", Password = "1234" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var book = new Book { Title = "Book", UserId = user.Id };
            var profile = new Profile { UserId = user.Id };
            var friend = new User { Nickname = "other", Password = "123" };
            _context.Users.Add(friend);
            _context.SaveChanges();

            var friendship = new Friendship { User1Id = user.Id, User2Id = friend.Id };
            var msg = new Message { SenderId = user.Id, ReceiverId = friend.Id, Content = "Hi" };

            _context.Books.Add(book);
            _context.Profiles.Add(profile);
            _context.Friendships.Add(friendship);
            _context.Messages.Add(msg);
            _context.SaveChanges();

            var result = _repo.DeleteUser("delete_me");

            Assert.True(result);
            Assert.Null(_context.Users.FirstOrDefault(u => u.Nickname == "delete_me"));
            Assert.Empty(_context.Books.Where(b => b.UserId == user.Id));
            Assert.Empty(_context.Messages.Where(m => m.SenderId == user.Id || m.ReceiverId == user.Id));
            Assert.Empty(_context.Friendships.Where(f => f.User1Id == user.Id || f.User2Id == user.Id));
            Assert.Null(_context.Profiles.FirstOrDefault(p => p.UserId == user.Id));
        }
    }
}
