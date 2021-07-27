using Order.Context;
using Order.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static Order.Enums;

namespace Order.Commands
{
    class AdminCommands
    {
        public static async void Test(TelegramBotClient bot, MessageEventArgs e)
        {
            //Console.WriteLine(new System.Diagnostics.StackTrace());
            try
            {
                if (await CheckAdmin(bot, e))
                {
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
                else
                {
                    await bot.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: "Вы не имеете доступа для выполнения данных команд");
                }
            }
            catch (ApiRequestException ex)
            {
                
                Console.WriteLine("\n" + ex.ToString());
                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "К сожалению бот не подключен ни к одному каналу, мы сообщили об этой ошибки администраторов, просим прощения за предоставленные неудобства");
            }
            
        }

        public static async void AllUsers(TelegramBotClient bot, MessageEventArgs e)
        {
            if (await CheckAdmin(bot, e))
            {
                StringBuilder usersStr = new StringBuilder("Список подключенных пользователей: \n");
                using (ApplicationContext db = new ApplicationContext())
                {
                    // получаем объекты из бд и выводим на консоль
                    var users = db.Users.ToList();

                    Console.WriteLine("Users list:");
                    foreach (Models.User u in users)
                    {
                        usersStr.Append($"{u.Id}.{u.ChatId} - {u.Name} \n");
                    }
                }
                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: usersStr.ToString());
            }
            else
            {
                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Вы не имеете доступа для выполнения данных команд");
            }
        }

        public static async void EditRate(TelegramBotClient bot, MessageEventArgs e)
        {
            if (await CheckAdmin (bot, e))
            {
                string[] args = e.Message.Text.Split(" ");
                int id = 0;
                int price = 0;
                int period = 0;
                switch (args.Length)
                {
                    case 1:
                        ErrorFormat(bot, e, (int)ErrFormatCode.editRate);
                        break;
                    case 2:
                        if (int.TryParse(args[1], out id))
                        {

                            using (ApplicationContext db = new ApplicationContext())
                            {
                                Rate rate = await db.Rate.FindAsync(id);
                                await bot.SendTextMessageAsync(
                                    chatId: e.Message.Chat,
                                    text: $"Название тарифа: {rate.Name} Цена: {rate.Price} рублей");
                            }
                        }
                        break;
                    case 3:
                        
                        if (int.TryParse(args[1], out id))
                        {
                            await EditRate(id, args[2]);
                            await bot.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: "Данные изменены");
                        }
                        else
                        {
                            ErrorFormat(bot, e, (int)ErrFormatCode.editRate);
                        }
                        break;
                    case 4:
                        if (int.TryParse(args[1], out id) && int.TryParse(args[3], out price))
                        {
                            await EditRate(id, args[2], price);
                            await bot.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: "Данные изменены");
                        }
                        else
                        {
                            ErrorFormat(bot, e, (int)ErrFormatCode.editRate);
                        }
                        break;
                    case 5:
                        if (int.TryParse(args[1], out id) && int.TryParse(args[3], out price) && int.TryParse(args[4], out period))
                        {
                            await EditRate(id, args[2], price, period);
                            await bot.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: "Данные изменены");
                        }
                        else
                        {
                            ErrorFormat(bot, e, (int)ErrFormatCode.editRate);
                        }
                        break;
                } 
            }
            else
            {
                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Вы не имеете доступа для выполнения данных команд");
            }

        }

        public static async void ErrorFormat(TelegramBotClient bot, MessageEventArgs e, int errFormat)
        {
            string text;
            switch (errFormat)
            {
                case (int)ErrFormatCode.editRate:
                    text = "Слишком мало аргументов или неверный формат записи\n\n" +
                                "Информация о тарифе       /admin/editRate {id} \n" +
                                "Пример                    /admin/editRate 1 \n\n" +
                                "Изменение названия        /admin/editRate {id} {Name} \n" +
                                "Пример                    /admin/editRate 1 Месячный \n\n" +
                                "Изменение названия и цены /admin/editRate {id} {Name} {Price} \n" +
                                "Пример                    /admin/editRate 1 Месячный 100 \n\n" +
                                "Изменение названия и цены и периода /admin/editRate {id} {Name} {Price} {Period}\n" +
                                "Пример                    /admin/editRate 1 Месячный 100 7 \n\n";
                    break;
                case (int)ErrFormatCode.addAdmin:
                    text = "Ошибка формата добавление администратора";
                    break;
                case (int)ErrFormatCode.removeAdmin:
                    text = "Ошибка формата удаления администратора";
                    break;
                default:
                    text = "Ошибка формата";
                    break;
            }
            await bot.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: text);
        }

        public static async void AddAdmin(TelegramBotClient bot, MessageEventArgs e)
        {
            if (await CheckAdmin (bot, e))
            {
                string[] args = e.Message.Text.Split(" ");
                long id = 0;
                switch (args.Length)
                {
                    case 2:
                        if(long.TryParse(args[1], out id))
                        {
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                Models.User user = await db.Users.FindAsync(id);
                                string text;
                                if(user != null)
                                {
                                    user.IsAdmin = true;
                                    db.SaveChanges();
                                    text = $"Пользователь: {user.Name} стал администратором";
                                }
                                else
                                {
                                    text = "Пользователь с таким id не подключён к боту";
                                }
                                await bot.SendTextMessageAsync(
                                        chatId: e.Message.Chat,
                                        text: text);
                            }
                        }
                        else
                        {
                            ErrorFormat(bot, e, (int)ErrFormatCode.addAdmin);
                        }
                        break;
                    default:
                        ErrorFormat(bot, e, (int)ErrFormatCode.addAdmin);
                        break;
                }
            }
            else
            {
                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Вы не имеете доступа для выполнения данных команд");
            }
        }

        public static async void RemoveAdmin(TelegramBotClient bot, MessageEventArgs e)
        {
            if (await CheckAdmin(bot, e))
            {
            string[] args = e.Message.Text.Split(" ");
            long id = 0;
                switch (args.Length)
                {
                    case 2:
                        if (long.TryParse(args[1], out id))
                        {
                            using (ApplicationContext db = new ApplicationContext())
                            {
                                Models.User user = await db.Users.FindAsync(id);
                                string text;
                                if (user != null)
                                {
                                    user.IsAdmin = false;
                                    db.SaveChanges();
                                    text = $"Пользователь: {user.Name} удалён из администраторов";
                                }
                                else
                                {
                                    text = "Пользователь с таким id не подключён к боту";
                                }
                                await bot.SendTextMessageAsync(
                                        chatId: e.Message.Chat,
                                        text: text);
                            }
                        }
                        else
                        {
                            ErrorFormat(bot, e, (int)ErrFormatCode.removeAdmin);
                        }
                        break;
                    default:
                        ErrorFormat(bot, e, (int)ErrFormatCode.removeAdmin);
                        break;
                }
            }
            else
            {
                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Вы не имеете доступа для выполнения данных команд");
            }
        }



        //services
        public static async Task<bool> CheckAdmin(TelegramBotClient bot, MessageEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Models.User user = await db.Users.FindAsync(e.Message.Chat.Id);
                if (user != null && user.IsAdmin)
                {
                    return true;
                }
                return false;
            }
        }

        public static async Task EditRate(int id, string name, int? price = null, int? period = null)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Rate rate = await db.Rate.FindAsync(id);
                rate.Name = name;
                if (price != null)
                {
                    rate.Price = (int)price;
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
