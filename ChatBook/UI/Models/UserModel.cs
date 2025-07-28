namespace ChatBook.Models
{
    public class UserModel
    {
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Avatar { get; set; }
        public string PhoneNumber { get; set; }
    }
}