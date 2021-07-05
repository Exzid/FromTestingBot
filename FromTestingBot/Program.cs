using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace FromTestingBot
{
    class Program
    {
        
        const string token = "1803795947:AAH6g_h-_Z4MiaCSCCXEZXpVpw-VPoJFy9c";
        static void Main(string[] args)
        {           
            TelegramBotClient Bot = new TelegramBotClient(token);
           
            var me = Bot.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );
            Bot.OnMessage += OnCommand.Bot_OnCommand;
            Bot.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            Bot.StopReceiving();
        }
    }
}