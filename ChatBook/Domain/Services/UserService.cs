using ChatBook.Entities;
using ChatBook.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace ChatBook.Domain.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IBookRepository _bookRepo;
        private readonly IMessageRepository _messageRepo;

        public UserService(IUserRepository userRepo, IBookRepository bookRepo, IMessageRepository messageRepo)
        {
            _userRepo = userRepo;
            _bookRepo = bookRepo;
            _messageRepo = messageRepo;
        }

        public bool Register(User user)
        {
            user.Password = HashPassword(user.Password);
            return _userRepo.Register(user);
        }

        public User Login(string nickname, string password)
        {
            var hashed = HashPassword(password);
            var user = _userRepo.GetByNickname(nickname);
            return user != null && user.Password == hashed ? user : null;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public User GetUserByNickname(string nickname) => _userRepo.GetByNickname(nickname);
        public bool UpdateProfile(User user) => _userRepo.UpdateProfile(user);
        public List<User> SearchUsers(string nickname) => _userRepo.SearchUsers(nickname);
        public List<User> GetFriends(string nickname) => _userRepo.GetFriends(nickname);
        public List<User> GetFollowers(string nickname) => _userRepo.GetFollowers(nickname);
        public bool AddFriend(string from, string to) => _userRepo.AddFriend(from, to);
        public bool RemoveFriend(string user, string friend) => _userRepo.RemoveFriend(user, friend);
        public bool AreFriends(string user1, string user2) => _userRepo.AreFriends(user1, user2);

        public bool AddBook(Book book, string nickname) => _bookRepo.AddBook(book, nickname);
        public bool UpdateBook(Book book) => _bookRepo.UpdateBook(book);
        public bool DeleteBook(int id) => _bookRepo.DeleteBook(id);
        public List<Book> GetUserBooks(string nickname) => _bookRepo.GetUserBooks(nickname);
        public List<BookWithReview> SearchBooksWithReviews(string titleQuery) => _bookRepo.SearchBooksWithReviews(titleQuery);
        public void SaveMessage(Message msg) => _messageRepo.SaveMessage(msg);
        public List<Message> GetChatMessages(string from, string to) => _messageRepo.GetMessages(from, to);
        public List<User> GetAllChatPartners(string nickname) => _messageRepo.GetChatPartners(nickname);
    }
}