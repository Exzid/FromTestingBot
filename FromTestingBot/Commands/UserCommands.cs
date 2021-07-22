using Order.Context;
using Order.Models;
using Order.Validation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using static Order.Enums;

namespace Order.Commands
{
    public static class UserCommands
    {
        public static async void Start(TelegramBotClient bot, MessageEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                long test = e.Message.Chat.Id;
                Models.User user1 = new Models.User { Id = e.Message.From.Id, ChatId = e.Message.Chat.Id, Name = e.Message.From.FirstName, };
                if (db.Users.Find(user1.Id) == null)
                {
                    db.Users.Add(user1);
                    db.SaveChanges();
                }
            }
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Добро пожаловать, я бот для тестирования");
            Menu(bot, e);
        }

        public static async void Menu(TelegramBotClient bot, MessageEventArgs e)
        {
            var button = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Мои данные", "/getData")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Ввести email", "/editEmail")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Ввести номер телефона", "/editPhone")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Информация о канале", "/infoChannel")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Перейти к покупке доступа", "/payments")
                    }
                });
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Выберите действие",
                replyMarkup: button);
        }

        public static async void EditEmail(TelegramBotClient bot, MessageEventArgs e)
        {
            string[] str = e.Message.Text.Split(' ');
            string text;
            switch (str.Length)
            {
                case 1:
                    WaitAnswer((int)WhatWait.Email, e.Message.Chat.Id);
                    text = "Введите ваш Email адрес";
                    break;
                case 2:
                    if (UserValidation.EmailRegex(str[1]) != null)
                    {
                        using (ApplicationContext db = new ApplicationContext())
                        {
                            Models.User user = await db.Users.FindAsync(e.Message.From.Id);
                            user.Email = str[1].ToLower();
                            db.SaveChanges();
                        }
                        WaitAnswer((int)WhatWait.NoWait, e.Message.From.Id);
                        text = $"Ваш Email адрес \"{str[1]}\" был добавлен";
                    }
                    else
                    {
                        text = ErrorFormat(bot, e, (int)WhatWait.Email);
                    }
                    break;
                default:
                    text = "Переданно слишком много аргументов";
                    break;
            }
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: text);
        }

        public static async void EditPhone(TelegramBotClient bot, MessageEventArgs e)
        {
            string[] str = e.Message.Text.Split(' ');
            string text;
            switch (str.Length)
            {
                case 1:
                    WaitAnswer((int)WhatWait.Phone, e.Message.Chat.Id);
                    text = "Введите ваш номер телефона";
                    break;
                case 2:
                    if (UserValidation.PhoneRegex(str[1]) != null)
                    {
                        using (ApplicationContext db = new ApplicationContext())
                        {
                            Models.User user = await db.Users.FindAsync(e.Message.From.Id);
                            user.Phone = str[1];
                            db.SaveChanges();
                        }
                        WaitAnswer((int)WhatWait.NoWait, e.Message.From.Id);
                        text = $"Ваш номер \"{str[1]}\" был добавлен";
                    }
                    else
                    {
                        text = ErrorFormat(bot, e, (int)WhatWait.Phone);
                    }
                    break;
                default:
                    text = "Переданно слишком много аргументов";
                    break;
            }
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: text);
        }

        public static async void GetData(TelegramBotClient bot, MessageEventArgs e)
        {
            string text;
            using (ApplicationContext db = new ApplicationContext())
            {
                Models.User user = db.Users.Where(u=>u.ChatId == e.Message.Chat.Id).ToList().First();
                if (user != null)
                {
                    string str = user.IsActive ? user.Subscription.ToString() : "Недействительна";
                    text = $"TelegramId: {user.Id} \n" +
                        $"ChatId: {user.ChatId} \n" +
                        $"Phone: {user.Phone} \n" +
                        $"Email: {user.Email} \n" +
                        $"IsAdmin: {user.IsAdmin} \n" +
                        $"Действие подписки: {str}";
                }
                else
                {
                    text = "Вы не зарегистрированы в системе";
                }
            }
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: text);
        }
       
        public static async void InfoChannel(TelegramBotClient bot, MessageEventArgs e)
        {
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Тут ваша информация о канале");
        }

        public static async void Cancel(TelegramBotClient bot, MessageEventArgs e)
        {
            WaitAnswer(0, e.Message.Chat.Id);
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Ввод данных отменён");
        }

        public static async void Payments(TelegramBotClient bot, MessageEventArgs e)
        {
            InlineKeyboardButton[][] rates;
            using (ApplicationContext db = new ApplicationContext())
            {
                var rateSort = from rate in db.Rate.ToList()
                               orderby rate.Id
                               select rate;

                rates = new InlineKeyboardButton[rateSort.Count()+1][];
                int count = 0;

                rates[count++] = new[] { InlineKeyboardButton.WithCallbackData($"Пользовательское соглашение", "/offer") };

                foreach (var rate in rateSort)
                {
                    var rateButton = new[] { InlineKeyboardButton.WithCallbackData($"Тариф: {rate.Name} Период: {rate.Period} дней Цена: {rate.Price} рублей", $"/rate {rate.Id}") };
                    rates[count++] = rateButton;
                }
            }
            
            var button = new InlineKeyboardMarkup(rates);
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Выберите тариф",
                replyMarkup: button);
        }

        public static async void Offer(TelegramBotClient bot, MessageEventArgs e)
        {
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Тут публичная оферта");
        }

        public static async void Rate(TelegramBotClient bot, MessageEventArgs e)
        {
            var args = e.Message.Text.Split(" ");
            string text;
            switch (args.Length)
            {
                case 2:
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        int id = 0;
                        if (int.TryParse(args[1], out id))
                        {
                            Rate rate = await db.Rate.FindAsync(id);
                            if (rate != null)
                            {
                                LabeledPrice[] prices = { new LabeledPrice("Руб", rate.Price*100) }; // в копейках
                                await bot.SendInvoiceAsync(
                                    chatId: e.Message.Chat.Id,
                                    title: "заголовок",
                                    description: "Описание",
                                    payload: "payload",
                                    providerToken: ConfigurationManager.AppSettings.Get("PaymentToken"),
                                    currency: "RUB",
                                    prices: prices
                                    );

                                text = "Это тестовый сценарий, для оплаты введите следующие данные карты:\n" +
                                       "Номер: 1111 1111 1111 1026,\nСрок действия: 12/22,\nCVC: 000.";
                            }
                            else
                            {
                                text = $"Тарифа с id: {id}, несуществует";
                            }
                        }
                        else
                        {
                            text = $"Неверный формат записи id";
                        }
                    }
                    break;
                default:
                    text = "Неверный формат";
                    break;
            }
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: text);
        }

        public static async void SuccessfulPayment(TelegramBotClient bot, MessageEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Models.User user = db.Users.ToList().Where(u => u.ChatId == e.Message.Chat.Id).First();
                Rate rate = db.Rate.ToList().Where(r => r.Price == e.Message.SuccessfulPayment.TotalAmount / 100).First();

                user.IsActive = true;
                user.Subscription = DateTime.Now.AddDays(rate.Period);
                await db.SaveChangesAsync();

                ChatInviteLink result = await bot.CreateChatInviteLinkAsync(
                    chatId: ConfigurationManager.AppSettings.Get("ChatIdChannel"),
                    memberLimit: 1);

                var button = new InlineKeyboardMarkup(
                    new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl($"Продолжить", result.InviteLink)
                        }
                    });

                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Оплата подтверждна, нажмите продожить для перехода в канал",
                    replyMarkup: button);
            }
        }

        //Сервисы
        public static string ErrorFormat(TelegramBotClient bot, MessageEventArgs e, int waitIndex)
        {
            string text;
            switch (waitIndex)
            {
                case (int)WhatWait.Phone:
                    text = "Неверный формат записи телефона";
                    break;
                case (int)WhatWait.Email:
                    text = "Неверный формат записи емейла";
                    break;
                default:
                    text = "Неверный формат записи";
                    break;
            }
            return text;
        }

        public static async void Default(TelegramBotClient bot, MessageEventArgs e)
        {
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Я не знаю такой команды");
        }

        public static void WaitAnswer(int waitIndex, long id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Models.User user = db.Users.Where(u => u.ChatId == id).First();
                if (user != null)
                {
                    user.IsWait = waitIndex;
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("user is null");
                }
            }
        }
    }
}