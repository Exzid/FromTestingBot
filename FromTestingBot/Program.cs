using System;
using Telegram.Bot;
using Order.Models;
using Order.Context;
using System.Linq;
using System.Configuration;

namespace Order
{
    class Program
    {
        public static TelegramBotClient bot;

        //static async void Main(string[] args)
        static void Main(string[] args)
        {
            bot = new TelegramBotClient(ConfigurationManager.AppSettings.Get("BotToken"));

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