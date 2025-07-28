using ChatBook.DataAccess;
using ChatBook.DataAccess.Repositories;
using ChatBook.Entities;
using System.Linq;
using Xunit;

namespace ChatBook.Tests.Database
{
    public class UserRepositoryMoreTests
    {
        private readonly ApplicationDbContext _context;
        private readonly UserRepository _repo;

        public UserRepositoryMoreTests()
        {
            _context = new ApplicationDbContext("name=TestDbContext");
            _repo = new UserRepository(_context);
            _context.Database.CreateIfNotExists();
        }

        [Fact]
        public void Register_ShouldAddUserToDatabase()
        {
            var user = new User { Nickname = "new_user", Password = "pass" };

            var result = _repo.Register(user);

            Assert.True(result);
            Assert.NotNull(_context.Users.FirstOrDefault(u => u.Nickname == "new_user"));
        }

        [Fact]
        public void GetByNickname_ShouldReturnCorrectUser()
        {
            var user = new User { Nickname = "nick_check", Password = "abc" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var found = _repo.GetByNickname("nick_check");

            Assert.NotNull(found);
            Assert.Equal("nick_check", found.Nickname);
        }

        [Fact]
        public void UpdateProfile_ShouldModifyUserData()
        {
            var user = new User { Nickname = "profile_user", FirstName = "Old", LastName = "Name", Password = "xyz" };
            _context.Users.Add(user);
            _context.SaveChanges();

            user.FirstName = "New";
            user.LastName = "Updated";

            var result = _repo.UpdateProfile(user);

            Assert.True(result);
            var updated = _context.Users.FirstOrDefault(u => u.Nickname == "profile_user");
            Assert.Equal("New", updated.FirstName);
        }

        [Fact]
        public void AreFriends_ShouldReturnTrue_WhenFriendshipExists()
        {
            var u1 = new User { Nickname = "u1", Password = "1" };
            var u2 = new User { Nickname = "u2", Password = "2" };
            _context.Users.AddRange(new[] { u1, u2 });
            _context.SaveChanges();

            _context.Friendships.Add(new Friendship { User1Id = u1.Id, User2Id = u2.Id });
            _context.SaveChanges();

            var result = _repo.AreFriends("u1", "u2");

            Assert.True(result);
        }

        [Fact]
        public void AddFriend_ShouldCreateFriendship()
        {
            var u1 = new User { Nickname = "af1", Password = "a" };
            var u2 = new User { Nickname = "af2", Password = "b" };
            _context.Users.AddRange(new[] { u1, u2 });
            _context.SaveChanges();

            var result = _repo.AddFriend("af1", "af2");

            Assert.True(result);
            Assert.NotNull(_context.Friendships.FirstOrDefault(f => f.User1Id == u1.Id && f.User2Id == u2.Id));
        }

        [Fact]
        public void RemoveFriend_ShouldDeleteFriendship()
        {
            var u1 = new User { Nickname = "rf1", Password = "a" };
            var u2 = new User { Nickname = "rf2", Password = "b" };
            _context.Users.AddRange(new[] { u1, u2 });
            _context.SaveChanges();

            _context.Friendships.Add(new Friendship { User1Id = u1.Id, User2Id = u2.Id });
            _context.SaveChanges();

            var result = _repo.RemoveFriend("rf1", "rf2");

            Assert.True(result);
            Assert.Empty(_context.Friendships.Where(f => f.User1Id == u1.Id && f.User2Id == u2.Id));
        }
    }
}
