using MongoDB.Bson;
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
        public static MongoDB.Driver.IMongoCollection<BsonDocument> usersDb;
        public static MongoDB.Driver.IMongoCollection<BsonDocument> waitDb;
        const string token = "1838651577:AAE_PhuNZ4Fj9f3_ZbqtAq26apVHEIJ8h2g";
        static void Main()
        {
            mongo = new MongoClient(connectionString);
            usersDb = Program.mongo.GetDatabase("Wecker").GetCollection<BsonDocument>("users");
            waitDb = Program.mongo.GetDatabase("Wecker").GetCollection<BsonDocument>("wait");
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