using System.ComponentModel.DataAnnotations;

namespace ChatBook.Entities
{
    public class Friendship
    {
        [Key]
        public int Id { get; set; }

        public int User1Id { get; set; }

        public int User2Id { get; set; }
    }
}
