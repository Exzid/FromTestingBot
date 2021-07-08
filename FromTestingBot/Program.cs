using System.Configuration;
using MongoDB.Driver;
using System;
using Telegram.Bot;
using MongoDB.Bson;

namespace Order
{
    class Program
    {
        public static MongoClient mongo;
        public static TelegramBotClient bot;
        public static IMongoCollection<BsonDocument> usersCollection;

        static void Main(string[] args)
        {
            mongo = new MongoClient(ConfigurationManager.AppSettings.Get("mongoConStr"));      
            bot = new TelegramBotClient(ConfigurationManager.AppSettings.Get("BotToken"));
            usersCollection = mongo.GetDatabase(ConfigurationManager.AppSettings.Get("Database"))
                         .GetCollection<BsonDocument>("users");

            bot.OnMessage += OnCommand.Bot_OnCommand;
            bot.OnCallbackQuery += OnCallback.BotOnCallbackQuery;
            bot.OnReceiveError += OnError.BotOnError;

            bot.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            bot.StopReceiving();
        }
        
    }
}