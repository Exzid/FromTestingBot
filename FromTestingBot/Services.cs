using Order.Context;
using System;
using System.Configuration;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Order
{
    class Services
    {
        public static TelegramBotClient CreateBotOnConfiguring()
        {
            TelegramBotClient bot = new TelegramBotClient(ConfigurationManager.AppSettings.Get("BotToken"));

            //Configuring
            bot.OnMessage += OnCommand.Bot_OnCommand;
            bot.OnCallbackQuery += OnCallback.BotOnCallbackQuery;
            bot.OnReceiveError += OnError.BotOnError;
            bot.OnReceiveGeneralError += OnError.BotOnGeneralError;
            bot.OnUpdate += OnPaymentUpdate.Bot_OnUpdate;

            return bot;
        }

        public async static void CheckSub(object obj) // Проверяет действительность подписки на канал
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
