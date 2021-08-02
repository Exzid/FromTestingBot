using MongoDB.Bson;
using MongoDB.Driver;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Order
{
    class OnCallback
    {
        public static async void BotOnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            MessageEventArgs e1 = new MessageEventArgs(e.CallbackQuery.Message);
            TelegramBotClient bot = (TelegramBotClient)sender;

            e1.Message.Text = e.CallbackQuery.Data;

            OnCommand.Bot_OnCommand(sender, e1); //Колбеки отправляют команды, поэтому просто пересылаем их в другой метод

            await bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
        }
    }
}
