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
                case "/workloadPersent":
                    WorkloadCallback(e, arr[1]);
                    e1.Message.Text = "/ok";
                    break;
                case "/availComAgree":
                    AvailComAgreeCallback(e, arr[1]);
                    e1.Message.Text = "/availComAgree";
                    break;
                default:
                    e1.Message.Text = e.CallbackQuery.Data;
                    break;
            }
            
            OnCommand.Bot_OnCommand(sender, e1);

        }

        static async void WorkloadCallback(CallbackQueryEventArgs e, string num)
        {
            Console.WriteLine("WorkloadCallback: " + num);

            var users = Program.mongo.GetDatabase("test").GetCollection<BsonDocument>("users");

            BsonDocument filter = new BsonDocument("_id", e.CallbackQuery.From.Id);
            BsonDocument update = new BsonDocument("$set", new BsonDocument("workload", new BsonInt32(int.Parse(num))));
            UpdateOptions options = new UpdateOptions { IsUpsert = true };

            await users.UpdateOneAsync(filter, update, options);
        }

        static async void AvailComAgreeCallback(CallbackQueryEventArgs e, string num)
        {
            Console.WriteLine("WorkloadCallback: " + num);

            var users = Program.mongo.GetDatabase("test").GetCollection<BsonDocument>("users");

            BsonDocument filter = new BsonDocument("_id", e.CallbackQuery.From.Id);
            BsonDocument update = new BsonDocument("$set", new BsonDocument("availability", new BsonInt32(int.Parse(num))));
            UpdateOptions options = new UpdateOptions { IsUpsert = true };

            await users.UpdateOneAsync(filter, update, options);
        }
    }
}
