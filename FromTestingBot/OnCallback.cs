using MongoDB.Bson;
using MongoDB.Driver;
using System;
using Telegram.Bot.Args;

namespace Wecker
{
    class OnCallback
    {     
        public static void BotOnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            MessageEventArgs e1 = new(e.CallbackQuery.Message);
            string str = e.CallbackQuery.Data;
            string[] arr = str.Split(' ');
            switch (arr[0])
            {
                case "/gender":
                    if (arr.Length > 1)
                    {
                        SetGender(e1, arr[1]);
                    }
                    e1.Message.Text = "/editAge";
                    break;
                default:
                    e1.Message.Text = e.CallbackQuery.Data;
                    break;
            }          
            OnCommand.Bot_OnCommand(sender, e1);
        }
        static async void SetGender(MessageEventArgs e, string gender)
        {
            BsonDocument filter = new BsonDocument("_id", e.Message.Chat.Id);
            BsonDocument update = new BsonDocument("$set", new BsonDocument("gender", gender));
            UpdateOptions options = new UpdateOptions { IsUpsert = true };
            await Program.usersDb.UpdateOneAsync(filter, update, options);
        }
    }
}
