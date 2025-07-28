using ChatBook.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatBook.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nickname { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public byte[] Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        public ICollection<Friendship> Friendships { get; set; }
        public ICollection<Book> Books { get; set; }
        public Profile Profile { get; set; }
    }
}
