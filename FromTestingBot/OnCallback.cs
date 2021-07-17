using MongoDB.Bson;
using MongoDB.Driver;
using System;
using Telegram.Bot.Args;

namespace Order
{
   

    class OnCallback
    {
        

        public static async void BotOnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            MessageEventArgs e1 = new MessageEventArgs(e.CallbackQuery.Message);
            string str = e.CallbackQuery.Data;
            string[] arr = str.Split(' ');

            switch (arr[0])
            {
                default:
                    e1.Message.Text = e.CallbackQuery.Data;
                    break;
            }
            OnCommand.Bot_OnCommand(sender, e1);

            Program.bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id);//показываем телеге что колбек обработан
        }
    }
}
