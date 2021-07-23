using System;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Order
{
    class OnError
    {
        public static async void BotOnError(object sender, ReceiveErrorEventArgs e)
        {
            TelegramBotClient bot = (TelegramBotClient)sender;

            StringBuilder error = new StringBuilder();
            error.Append("ПРОИЗОШЛА ОШИБКА!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
            error.Append("ErrorCode: " + e.ApiRequestException.ErrorCode + "\n");           
            error.Append("Message: " + e.ApiRequestException.Message + "\n");
            error.Append("StackTrace: " + e.ApiRequestException.StackTrace + "\n");
            error.Append("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
            Console.WriteLine(error);
            if (e.ApiRequestException.Parameters != null)
            {
                error.Append("MigrateToChatId: " + e.ApiRequestException.Parameters.MigrateToChatId + "\n");
                error.Append("RetryAfter: " + e.ApiRequestException.Parameters.RetryAfter + "\n");
                await bot.SendTextMessageAsync(
                    chatId: e.ApiRequestException.Parameters.MigrateToChatId,
                    text: error.ToString());
            }                 
        }
        public static async void BotOnGeneralError(object sender, ReceiveGeneralErrorEventArgs e)
        {
            TelegramBotClient bot = (TelegramBotClient)sender;

            StringBuilder error = new StringBuilder();
            error.Append("ПРОИЗОШЛА ОШИБКА!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
            error.Append("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
            Console.WriteLine(error.ToString());
        }       
    }
}
