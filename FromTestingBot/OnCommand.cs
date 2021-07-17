using Order.Context;
using Order.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Order
{
    public static class OnCommand
    {
        public static async void Bot_OnCommand(object sender, MessageEventArgs e)
        {
            TelegramBotClient bot = (TelegramBotClient)sender;
            if (e.Message.Text != null && e.Message.Text[0] == '/')
            {
                switch (e.Message.Text)
                {
                    case "/start":
                        Start(bot, e);
                        break;
                    case "/admin/users":
                        AllUsers(bot, e);
                        break;
                    default:
                        Default(bot, e);
                        break;
                }
            }
            else
            {
                OnMessage.Bot_OnMessage(bot, e);
            }
        }

        static async void PoolCommand(TelegramBotClient bot, MessageEventArgs e)
        {
            List<string> options = new List<string>();
            options.Add("неплохо");
            options.Add("неочень");

            await bot.SendPollAsync(chatId: e.Message.Chat,
                question: "Как тебе тестовый опрос?",
                options: options,
                true);
        }

        static async void ReplyKeyboardMarkup(TelegramBotClient bot, MessageEventArgs e)
        {
            ReplyKeyboardMarkup button = new ReplyKeyboardMarkup(
                new [] 
                { 
                    new[]
                    {
                        new KeyboardButton("test"),
                    },
                    new[]
                    {
                        new KeyboardButton("tes3"),
                        new KeyboardButton("test4")
                    }                   
                }, true, true);
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "test",            
                replyMarkup: button);
        }

        static async void Default(TelegramBotClient bot, MessageEventArgs e)
        {
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Я не знаю ткой команды");
        }

        static async void Start(TelegramBotClient bot, MessageEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // создаем два объекта User
                long test = e.Message.Chat.Id;
                User user1 = new User { Id = e.Message.From.Id, IdChat = e.Message.Chat.Id, Name = e.Message.From.FirstName };

                // добавляем их в бд
                db.Users.Add(user1);
                db.SaveChanges();
                Console.WriteLine("Объекты успешно сохранены");
            }
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Добро пожаловать, я бот для тестирования");
        }

        static async void AllUsers(TelegramBotClient bot, MessageEventArgs e)
        {
            StringBuilder usersStr = new StringBuilder("Список подключенных пользователей: \n");
            using (ApplicationContext db = new ApplicationContext())
            {
                // получаем объекты из бд и выводим на консоль
                var users = db.Users.ToList();
                
                Console.WriteLine("Users list:");
                foreach (User u in users)
                {
                    usersStr.Append($"{u.Id}.{u.IdChat} - {u.Name} \n");
                }
            }
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: usersStr.ToString());
        }

        // получаем объекты из бд и выводим на консоль   
        
    }
}
