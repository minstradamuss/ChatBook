using System.ComponentModel.DataAnnotations;

namespace ChatBook.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string Status { get; set; }

        public int Rating { get; set; }

        public string Review { get; set; }

        public byte[] CoverImage { get; set; }
        public int UserId { get; set; }

        public string Genre { get; set; }
        public string Author { get; set; }

        public virtual User User { get; set; }
    }
}
