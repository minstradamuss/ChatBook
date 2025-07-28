using ChatBook.Domain.Services;
using ChatBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBook.UI.ViewModel
{
    public class BookViewModel
    {
        private readonly UserService _userService;

        public BookViewModel(UserService userService)
        {
            _userService = userService;
        }

        public List<Book> GetUserBooks(string nickname)
        {
            return _userService.GetUserBooks(nickname);
        }

        public void AddBook(Book book, string nickname)
        {
            _userService.AddBook(book, nickname);
        }

        public void UpdateBook(Book book)
        {
            _userService.UpdateBook(book);
        }

        public void DeleteBook(int id)
        {
            _userService.DeleteBook(id);
        }
    }

}
