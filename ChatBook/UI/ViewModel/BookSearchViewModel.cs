using ChatBook.Domain.Services;
using ChatBook.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ChatBook.ViewModels
{
    public class BookSearchViewModel
    {
        private readonly UserService _userService;

        public BookSearchViewModel(UserService userService)
        {
            _userService = userService;
        }

        public List<(Book Book, string ReviewerNickname)> Search(string query)
        {
            return _userService
                .SearchBooksWithReviews(query)
                .Select(bwr => (bwr.Book, bwr.ReviewerNickname))
                .ToList();
        }

        public User GetReviewer(string nickname)
        {
            return _userService.GetUserByNickname(nickname);
        }
    }
}
