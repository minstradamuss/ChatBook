using ChatBook.Domain.Services;
using ChatBook.Entities;

namespace ChatBook.ViewModels
{
    public class AddBookViewModel
    {
        private readonly UserService _userService;

        public AddBookViewModel(UserService userService)
        {
            _userService = userService;
        }

        public bool AddBook(Book book, string nickname)
        {
            try
            {
                _userService.AddBook(book, nickname);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateBook(Book book)
        {
            try
            {
                _userService.UpdateBook(book);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void DeleteBook(int bookId)
        {
            _userService.DeleteBook(bookId);
        }
    }
}
