using System;
using System.Data.Entity.Migrations;

namespace ChatBook.Migrations
{
    public partial class AddGenreToBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Genre", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Books", "Genre");
        }
    }
}
