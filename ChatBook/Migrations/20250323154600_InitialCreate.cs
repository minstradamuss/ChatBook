using System.Data.Entity.Migrations;

namespace ChatBook.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.Users", c => new
            {
                Id = c.Int(nullable: false, identity: true),
                Nickname = c.String(nullable: false),
                FirstName = c.String(),
                LastName = c.String(),
                Password = c.String(),
                PhoneNumber = c.String(),
                Avatar = c.Binary(),
            })
            .PrimaryKey(t => t.Id);

            CreateTable("dbo.Books", c => new
            {
                Id = c.Int(nullable: false, identity: true),
                Title = c.String(nullable: false),
                Status = c.String(),
                Rating = c.Int(nullable: false),
                Review = c.String(),
                CoverImage = c.Binary(),
                Genre = c.String(),
                UserId = c.Int(nullable: false),
            })
            .PrimaryKey(t => t.Id)
            .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
            .Index(t => t.UserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Books", "UserId", "dbo.Users");
            DropIndex("dbo.Books", new[] { "UserId" });
            DropTable("dbo.Books");
            DropTable("dbo.Users");
        }
    }
}
