using ChatBook.Entities;

namespace ChatBook.Domain.Factories
{
    public class BookFactory : IBookFactory
    {
        public Book Create(string title, string author, string genre, string status, int rating, string review, byte[] coverImage, int userId)
        {
            return new Book
            {
                Title = title,
                Author = author,
                Genre = genre,
                Status = status,
                Rating = rating,
                Review = review,
                CoverImage = coverImage,
                UserId = userId
            };
        }
    }
}
