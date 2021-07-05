using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace FromTestingBot
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
                    case "/test":
                        testCommand(bot, e);
                        break;
                    case "/help":
                        helpCommand(bot, e);
                        break;
                    default:
                        defaultCommand(bot, e);
                        break;
                }
            }
            else
            {
                OnMessage.Bot_OnMessage(bot, e);
            }
        }
        
        static async void testCommand(TelegramBotClient bot, MessageEventArgs e)
        {
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Тестируем бд");
            //await "тут подключаемся к бд"
        }

        static async void helpCommand(TelegramBotClient bot, MessageEventArgs e)
        {
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "/test - ping bot/n");
        }

        static async void defaultCommand(TelegramBotClient bot, MessageEventArgs e)
        {
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "What did you commanded me??????");
        }
    }
}
