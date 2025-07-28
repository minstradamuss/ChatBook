using System.ComponentModel.DataAnnotations;

namespace ChatBook.Entities
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
