using ChatBook.Entities;
using System.Collections.Generic;

namespace ChatBook.Domain.Interfaces
{
    public interface IBookRepository
    {
        bool AddBook(Book book, string username);
        bool UpdateBook(Book book);
        bool DeleteBook(int bookId);
        List<Book> GetUserBooks(string username);
        List<Book> GetReadBooks(string username);
        Book GetBookByUserAndTitle(int userId, string bookTitle);
        List<BookWithReview> SearchBooksWithReviews(string titleQuery);
    }
}
