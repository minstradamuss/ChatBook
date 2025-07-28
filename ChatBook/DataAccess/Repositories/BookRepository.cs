using ChatBook.Domain.Interfaces;
using ChatBook.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ChatBook.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddBook(Book book, string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Nickname == username);
            if (user == null) return false;

            book.UserId = user.Id;
            _context.Books.Add(book);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteBook(int bookId)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null) return false;

            _context.Books.Remove(book);
            _context.SaveChanges();
            return true;
        }

        public List<BookWithReview> SearchBooksWithReviews(string titleQuery)
        {
            return _context.Books.Include(b => b.User)
                .Where(b => b.Status == "Прочитано" && b.Title.ToLower().Contains(titleQuery.ToLower()))
                .OrderByDescending(b => b.Rating)
                .Select(b => new BookWithReview
                {
                    Book = b,
                    ReviewerNickname = b.User.Nickname,
                    Review = b.Review
                }).ToList();
        }

        public Book GetBookByUserAndTitle(int userId, string bookTitle)
        {
            return _context.Books.FirstOrDefault(b => b.UserId == userId && b.Title == bookTitle);
        }

        public List<Book> GetReadBooks(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Nickname == username);
            return user == null ? new List<Book>() :
                _context.Books.Where(b => b.UserId == user.Id && b.Status == "Прочитано").ToList();
        }

        public List<Book> GetUserBooks(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Nickname == username);
            return user == null ? new List<Book>() :
                _context.Books.Where(b => b.UserId == user.Id).ToList();
        }

        public bool UpdateBook(Book updated)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == updated.Id);
            if (book == null) return false;

            book.Title = updated.Title;
            book.Status = updated.Status;
            book.Rating = updated.Rating;
            book.Review = updated.Review;
            book.CoverImage = updated.CoverImage;

            _context.SaveChanges();
            return true;
        }
    }
    
}
