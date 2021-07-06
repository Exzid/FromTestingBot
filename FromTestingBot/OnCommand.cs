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
                    case "/setting":
                        settingCommand(bot, e);
                        break;
                    case "/help":
                        helpCommand(bot, e);
                        break;
                    case "/workload":
                        workloadCommand(bot, e);
                        break;
                    case "/availability":
                        availabilityCommand(bot, e);
                        break;
                    case "/ok":
                        okCommand(bot, e);
                        break;
                    case "/availComAgree":
                        availComAgreeCommand(bot, e);
                        break;
                    case "/availCom":
                        availComCommand(bot, e);
                        break;
                    case "/Ktest":
                        testCommand(bot, e);
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

        static async void settingCommand(TelegramBotClient bot, MessageEventArgs e)
        {
            var button = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Загруженность", "/workload"),
                            InlineKeyboardButton.WithCallbackData("Доступность", "/availability"),
                        },
                });
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Что вы хотите настроить?",
                replyMarkup: button);
        }

        static async void workloadCommand(TelegramBotClient bot, MessageEventArgs e)
        {
            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("0%", "/workloadPersent 0"),
                            InlineKeyboardButton.WithCallbackData("25%", "/workloadPersent 25"),
                            InlineKeyboardButton.WithCallbackData("50%", "/workloadPersent 50"),
                            InlineKeyboardButton.WithCallbackData("75%", "/workloadPersent 70"),
                            InlineKeyboardButton.WithCallbackData("100%", "/workloadPersent 100")
                        }   
                });
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Укажите степень вашей загруженности",
                replyMarkup: button);
        }

        static async void availabilityCommand(TelegramBotClient bot, MessageEventArgs e)
        {          
            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Не доступен", "/availComAgree 0"),
                            InlineKeyboardButton.WithCallbackData("Частично доступен", "/availComAgree 1"),
                            InlineKeyboardButton.WithCallbackData("На связи", "/availComAgree 10")
                        }
                });
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Укажите вашу доступность на данный момент",
                replyMarkup: button);
        }

        static async void availComAgreeCommand(TelegramBotClient bot, MessageEventArgs e)
        {
            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Да", "/availCom"),
                            InlineKeyboardButton.WithCallbackData("Нет", "/ok"),
                        }
                });
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Хотите оставить подробности?",
                replyMarkup: button);
        }

        static async void availComCommand(TelegramBotClient bot, MessageEventArgs e)
        {        
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Этот этап всё ещё в разработке");
        }
        
        static async void okCommand(TelegramBotClient bot, MessageEventArgs e)
        {
            await bot.SendStickerAsync(
                chatId: e.Message.Chat,
                sticker: "CAACAgIAAxkBAAIBF2DjgBPLOAv_8NF9iD-U8kKgAAGa5AACVgADQbVWDNWTZQVPrTRWIAQ");
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
