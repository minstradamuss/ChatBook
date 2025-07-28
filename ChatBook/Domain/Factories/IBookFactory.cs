using ChatBook.Entities;

namespace ChatBook.Domain.Factories
{
    public interface IBookFactory
    {
        Book Create(string title, string author, string genre, string status, int rating, string review, byte[] coverImage, int userId);
    }
}
