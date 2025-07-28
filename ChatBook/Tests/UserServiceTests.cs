using ChatBook.Domain.Services;
using ChatBook.Domain.Interfaces;
using ChatBook.Entities;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace ChatBook.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new Mock<IUserRepository>();
        private readonly Mock<IBookRepository> _bookRepoMock = new Mock<IBookRepository>();
        private readonly Mock<IMessageRepository> _messageRepoMock = new Mock<IMessageRepository>();

        private readonly UserService _service;

        public UserServiceTests()
        {
            _service = new UserService(_userRepoMock.Object, _bookRepoMock.Object, _messageRepoMock.Object);
        }

        [Fact]
        public void Register_ShouldHashPassword_And_Call_Repository()
        {
            var user = new User { Nickname = "test", Password = "plain" };
            _userRepoMock.Setup(r => r.Register(It.IsAny<User>())).Returns(true);

            var result = _service.Register(user);

            Assert.True(result);
            _userRepoMock.Verify(r => r.Register(It.Is<User>(u => u.Password != "plain")), Times.Once);
        }

        [Fact]
        public void Login_ShouldReturnUser_WhenCorrectPassword()
        {
            var plainPassword = "1234";
            var hashedPassword = typeof(UserService)
                .GetMethod("HashPassword", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_service, new object[] { plainPassword }) as string;

            var user = new User { Nickname = "user1", Password = hashedPassword };
            _userRepoMock.Setup(r => r.GetByNickname("user1")).Returns(user);

            var result = _service.Login("user1", "1234");

            Assert.NotNull(result);
            Assert.Equal("user1", result.Nickname);
        }

        [Fact]
        public void Login_ShouldReturnNull_WhenWrongPassword()
        {
            var user = new User { Nickname = "user1", Password = "hashedWrong" };
            _userRepoMock.Setup(r => r.GetByNickname("user1")).Returns(user);

            var result = _service.Login("user1", "1234");

            Assert.Null(result);
        }

        [Fact]
        public void GetFriends_ShouldReturnCorrectList()
        {
            var expected = new List<User> { new User { Nickname = "f1" } };
            _userRepoMock.Setup(r => r.GetFriends("me")).Returns(expected);

            var friends = _service.GetFriends("me");

            Assert.Single(friends);
            Assert.Equal("f1", friends[0].Nickname);
            _userRepoMock.Verify(r => r.GetFriends("me"), Times.Once);
        }

        [Fact]
        public void AddBook_ShouldCallRepository()
        {
            var book = new Book { Title = "title" };
            _bookRepoMock.Setup(r => r.AddBook(book, "nick")).Returns(true);

            var result = _service.AddBook(book, "nick");

            Assert.True(result);
            _bookRepoMock.Verify(r => r.AddBook(book, "nick"), Times.Once);
        }

        [Fact]
        public void SaveMessage_ShouldCallRepository()
        {
            var msg = new Message { Content = "hi" };
            _messageRepoMock.Setup(r => r.SaveMessage(msg));

            _service.SaveMessage(msg);

            _messageRepoMock.Verify(r => r.SaveMessage(msg), Times.Once);
        }

        [Fact]
        public void DeleteBook_ShouldCallRepository()
        {
            _bookRepoMock.Setup(r => r.DeleteBook(1)).Returns(true);

            var result = _service.DeleteBook(1);

            Assert.True(result);
            _bookRepoMock.Verify(r => r.DeleteBook(1), Times.Once);
        }

        [Fact]
        public void SearchUsers_ShouldCallRepository()
        {
            var users = new List<User> { new User { Nickname = "searchUser" } };
            _userRepoMock.Setup(r => r.SearchUsers("search")).Returns(users);

            var result = _service.SearchUsers("search");

            Assert.Single(result);
            Assert.Equal("searchUser", result[0].Nickname);
        }

        [Fact]
        public void UpdateBook_ShouldCallRepository()
        {
            var book = new Book { Id = 1, Title = "Updated" };
            _bookRepoMock.Setup(r => r.UpdateBook(book)).Returns(true);

            var result = _service.UpdateBook(book);

            Assert.True(result);
            _bookRepoMock.Verify(r => r.UpdateBook(book), Times.Once);
        }

        [Fact]
        public void GetUserByNickname_ShouldReturnUser()
        {
            var user = new User { Nickname = "john" };
            _userRepoMock.Setup(r => r.GetByNickname("john")).Returns(user);

            var result = _service.GetUserByNickname("john");

            Assert.Equal("john", result.Nickname);
        }

        [Fact]
        public void AreFriends_ShouldCallRepository()
        {
            _userRepoMock.Setup(r => r.AreFriends("u1", "u2")).Returns(true);

            var result = _service.AreFriends("u1", "u2");

            Assert.True(result);
            _userRepoMock.Verify(r => r.AreFriends("u1", "u2"), Times.Once);
        }
    }
}