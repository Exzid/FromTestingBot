using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace Order
{
    class OnPaymentUpdate
    {
        public static async void Bot_OnUpdate(object sender, UpdateEventArgs e)
        {
            TelegramBotClient bot = (TelegramBotClient)sender;
            switch (e.Update.Type)
            {
                case UpdateType.PreCheckoutQuery:
                    await bot.AnswerPreCheckoutQueryAsync(e.Update.PreCheckoutQuery.Id);
                    break;
            }
        }
    }
}
