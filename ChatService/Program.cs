using System;
using System.Threading;
using ChatService.Entities;
using ChatService.Services;

namespace ChatService.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ChatService Имитация переписки ===");

            var chatRepo = new InMemoryChatRepository();
            var chatService = new ChatService.Services.ChatService(chatRepo);


            string user1 = "Alice";
            string user2 = "Bob";

            chatService.SendMessage(user1, user2, "Привет, Боб!");
            Thread.Sleep(500);
            chatService.SendMessage(user2, user1, "Привет! Как дела?");
            Thread.Sleep(500);
            chatService.SendMessage(user1, user2, "Отлично, спасибо.");

            Console.WriteLine("\nИстория чата между Alice и Bob:\n");

            var history = chatService.GetChatHistory(user1, user2);
            foreach (var msg in history)
            {
                Console.WriteLine($"[{msg.Timestamp:HH:mm:ss}] {msg.FromUser} ➤ {msg.ToUser}: {msg.Text}");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
