using MongoDB.Driver;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;

namespace Wecker
{
    class Program
    {
        private static readonly string connectionString = "mongodb://localhost";

        public static MongoClient mongo;

        const string token = "1803795947:AAH6g_h-_Z4MiaCSCCXEZXpVpw-VPoJFy9c";
        static void Main()
        {
            mongo = new MongoClient(connectionString);
            TelegramBotClient Bot = new(token);
           
            var me = Bot.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );     
            Bot.OnMessage += OnCommand.Bot_OnCommand;
            Bot.OnCallbackQuery += OnCallback.BotOnCallbackQuery;
            Bot.OnReceiveError += OnError.BotOnError;
            Bot.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            Bot.StopReceiving();
        }
        
    }
}