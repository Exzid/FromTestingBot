﻿using System;
using Telegram.Bot;
using Order.Models;
using Order.Context;
using System.Linq;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types.Payments;
using System.Threading;
using Telegram.Bot.Types;

namespace Order
{
    class Program
    {
        public static TelegramBotClient bot;

        static void Main(string[] args)
        {
            bot = new TelegramBotClient(ConfigurationManager.AppSettings.Get("BotToken"));

            bot.OnMessage += OnCommand.Bot_OnCommand;
            bot.OnCallbackQuery += OnCallback.BotOnCallbackQuery;
            bot.OnReceiveError += OnError.BotOnError;
            bot.OnReceiveGeneralError += OnError.BotOnGeneralError;
            bot.OnUpdate += OnPaymentUpdate.Bot_OnUpdate;

            //Timer timer = new Timer(CheckSub, bot, 0, 7200000);
            Timer timer = new Timer(CheckSub, bot, 0, 2000);

            bot.StartReceiving();


            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            bot.StopReceiving();
        }

        public async static void CheckSub(object obj)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    TelegramBotClient bot = (TelegramBotClient)obj;
                    var users = db.Users.Where(u => u.IsActive == true && u.Subscription < DateTime.Now && u.IsAdmin == false).ToList();
                    foreach (Models.User u in users)
                    {
                        u.IsActive = false;
                        await bot.KickChatMemberAsync(
                            chatId: new ChatId(ConfigurationManager.AppSettings.Get("ChatIdChannel")),
                            userId: u.Id);
                        await bot.SendTextMessageAsync(
                            chatId: new ChatId(u.ChatId),
                            text: "Ваша подписка закончилась, для продления введите /payments и выберите тариф");
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("catch unable exception in CheckSub\n " + e.ToString());
            }
        }
    }      
}