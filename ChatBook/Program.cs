using ChatBook.DataAccess;
using ChatBook.UI.Forms;
using ChatBook.UI.Windows;
using ChatBook.Domain.Interfaces;
using System;
using System.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Forms;
using ChatBook.ViewModels;
using ChatBook.DB;
using ChatBook.DataAccess.Repositories;
using ChatBook.Domain.Factories;
using ChatService.Domain;

namespace ChatBook
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            ConfigureServices();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            
            var loginForm = ServiceProvider.GetRequiredService<LoginForm>();
            
            
            Application.Run(loginForm);
        }

        static void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ApplicationDbContext>();

            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IBookRepository, BookRepository>();
            services.AddSingleton<IMessageRepository, MessageRepository>();

            services.AddSingleton<ChatBook.Domain.Services.UserService>();

            services.AddSingleton<AddBookViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<BookSearchViewModel>();
            services.AddSingleton<ProfileViewModel>();
            services.AddTransient<ChatViewModel>();
            services.AddSingleton<FriendsViewModel>();

            services.AddTransient<LoginForm>();
            services.AddTransient<MainForm>();
            services.AddTransient<FriendsForm>();
            services.AddTransient<EditProfileWindow>();
            services.AddTransient<ChatForm>();
            services.AddTransient<BookSearchWindow>();
            services.AddSingleton<IBookFactory, BookFactory>();

            services.AddTransient<AddBookWindow>();

            services.AddSingleton<IChatRepository, ChatRepository>();

            services.AddSingleton<IChatService, ChatService.Services.ChatService>();

            Database.SetInitializer(new DbInitializer());

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
