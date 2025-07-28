using System.Data.Entity;
using ChatBook.Entities;

namespace ChatBook.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("name=EntitesContext")
        {
        }

        public ApplicationDbContext(string connectionString)
            : base(connectionString) 
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOptional(u => u.Profile)
                .WithRequired(p => p.User);

            modelBuilder.Entity<Friendship>()
                .HasKey(f => new { f.User1Id, f.User2Id });

            base.OnModelCreating(modelBuilder);
        }
    }
}
