using System;
using System.IO;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Design;

namespace ChatBook.Migrations
{
    public static class MigrationHelper
    {
        public static void GenerateInitialMigration()
        {
            var config = new Configuration();
            var scaffolder = new MigrationScaffolder(config);

            var scaffolded = scaffolder.Scaffold("InitialCreate");
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"{timestamp}_InitialCreate";

            var migrationsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Migrations");
            Directory.CreateDirectory(migrationsPath);

            File.WriteAllText(Path.Combine(migrationsPath, fileName + ".cs"), scaffolded.UserCode);
            File.WriteAllText(Path.Combine(migrationsPath, fileName + ".Designer.cs"), scaffolded.DesignerCode);
        }
    }
}
