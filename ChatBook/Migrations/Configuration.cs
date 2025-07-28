using System.Data.Entity.Migrations;
using ChatBook.DataAccess;
using System.Data.SQLite.EF6;
using System.Data.SQLite.EF6.Migrations;


namespace ChatBook.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = "Migrations";
            SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
        }

    }
}
