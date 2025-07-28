using ChatBook.Domain.Interfaces;
using ChatBook.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ChatBook.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddFriend(string userNickname, string friendNickname)
        {
            var user = _context.Users.FirstOrDefault(u => u.Nickname == userNickname);
            var friend = _context.Users.FirstOrDefault(u => u.Nickname == friendNickname);

            if (user == null || friend == null || AreFriends(userNickname, friendNickname))
                return false;

            _context.Friendships.Add(new Friendship { User1Id = user.Id, User2Id = friend.Id });
            _context.SaveChanges();
            return true;
        }

        public bool AreFriends(string user1, string user2)
        {
            var u1 = _context.Users.FirstOrDefault(u => u.Nickname == user1);
            var u2 = _context.Users.FirstOrDefault(u => u.Nickname == user2);

            return u1 != null && u2 != null &&
                   _context.Friendships.Any(f =>
                       (f.User1Id == u1.Id && f.User2Id == u2.Id) ||
                       (f.User1Id == u2.Id && f.User2Id == u1.Id));
        }

        public List<User> GetFollowers(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Nickname == username);
            if (user == null) return new List<User>();

            var followerIds = _context.Friendships.Where(f => f.User2Id == user.Id).Select(f => f.User1Id).ToList();
            return _context.Users.Where(u => followerIds.Contains(u.Id)).ToList();
        }

        public List<User> GetFriends(string nickname)
        {
            var user = _context.Users.FirstOrDefault(u => u.Nickname == nickname);
            if (user == null) return new List<User>();

            var friendIds = _context.Friendships.Where(f => f.User1Id == user.Id).Select(f => f.User2Id).ToList();
            return _context.Users.Where(u => friendIds.Contains(u.Id)).ToList();
        }

        public User GetByNickname(string nickname)
        {
            return _context.Users.Include(u => u.Profile).FirstOrDefault(u => u.Nickname == nickname);
        }

        public bool Register(User user)
        {
            if (_context.Users.Any(u => u.Nickname == user.Nickname))
                return false;

            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        public bool RemoveFriend(string userNickname, string friendNickname)
        {
            var user = _context.Users.FirstOrDefault(u => u.Nickname == userNickname);
            var friend = _context.Users.FirstOrDefault(u => u.Nickname == friendNickname);

            if (user == null || friend == null) return false;

            var friendship = _context.Friendships.FirstOrDefault(f =>
                (f.User1Id == user.Id && f.User2Id == friend.Id) ||
                (f.User1Id == friend.Id && f.User2Id == user.Id));

            if (friendship == null) return false;

            _context.Friendships.Remove(friendship);
            _context.SaveChanges();
            return true;
        }

        public List<User> SearchUsers(string nickname)
        {
            return _context.Users.Where(u => u.Nickname.ToLower().Contains(nickname.ToLower())).ToList();
        }

        public bool UpdateProfile(User user)
        {
            var existing = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existing == null) return false;

            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.PhoneNumber = user.PhoneNumber;
            existing.Avatar = user.Avatar;
            _context.SaveChanges();

            return true;
        }

        public bool DeleteUser(string nickname)
        {
            var user = _context.Users.FirstOrDefault(u => u.Nickname == nickname);
            if (user == null) return false;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var books = _context.Books.Where(b => b.UserId == user.Id);
                    _context.Books.RemoveRange(books);

                    var messages = _context.Messages
                        .Where(m => m.SenderId == user.Id || m.ReceiverId == user.Id);
                    _context.Messages.RemoveRange(messages);

                    var friendships = _context.Friendships
                        .Where(f => f.User1Id == user.Id || f.User2Id == user.Id);
                    _context.Friendships.RemoveRange(friendships);

                    var profile = _context.Profiles.FirstOrDefault(p => p.UserId == user.Id);
                    if (profile != null)
                        _context.Profiles.Remove(profile);

                    _context.Users.Remove(user);

                    _context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

    }


}
