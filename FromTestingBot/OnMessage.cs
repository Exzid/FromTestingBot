using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static Wecker.OnCommand;

namespace Wecker
{
    public static class OnMessage
    { 
        public static async void Bot_OnMessage(TelegramBotClient bot, MessageEventArgs e)
        {
            BsonDocument filter = new BsonDocument("_id", e.Message.Chat.Id);
            IFindFluent<BsonDocument, BsonDocument> waitAnswer = Program.waitDb.Find(filter);
            if (waitAnswer.Count() != 0)
            {
                var result = await waitAnswer.FirstAsync();
                string select;
                string nextCommand;

                switch (result.GetValue("nextIsAnswer").ToInt32())
                {
                    case (int)WaitToAnswer.EditName:
                        select = "name";
                        nextCommand = "/editGender";
                        break;
                    case (int)WaitToAnswer.EditAge:
                        select = "age";
                        nextCommand = "/editRegion";
                        break;
                    case (int)WaitToAnswer.EditRegion:
                        select = "region";
                        nextCommand = "/main";
                        break;
                    default:
                        select = "errorOnMessage";
                        nextCommand = "/main";
                        break;
                }

                await Program.waitDb.DeleteOneAsync(filter);
                BsonDocument update = new BsonDocument("$set", new BsonDocument(select, e.Message.Text));
                UpdateOptions options = new UpdateOptions { IsUpsert = true };
                await Program.usersDb.UpdateOneAsync(filter, update, options);
                Console.WriteLine(nextCommand);
                e.Message.Text = nextCommand;
                Bot_OnCommand(bot, e);
            }
            else
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Это текст  :" + e.Message.Text
                );
            }
            else
            if (e.Message.Photo != null)
            {
                await bot.SendPhotoAsync(
                    chatId: e.Message.Chat,
                    photo: e.Message.Photo[0].FileId,
                    caption: "пересылая обратно");
                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: $"Это фото, его id: {e.Message.Photo[0].FileId}"
                );                
            }
            else
            if (e.Message.Document != null)
            {
                await bot.SendDocumentAsync(
                    chatId: e.Message.Chat,
                    document: e.Message.Document.FileId
                );
                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: $"Это Документ, его id: {e.Message.Document.FileId}"
                );               
            }
            else
            if(e.Message.Sticker != null)
            {
                await bot.SendStickerAsync(
                    chatId: e.Message.Chat,
                    sticker: e.Message.Sticker.FileId);
                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: $"id стикера: {e.Message.Sticker.FileId}");
            }           
            else
            {
                await bot.SendTextMessageAsync(
                      chatId: e.Message.Chat,
                      text: "не поддерживаю этот формат");
            }
        }
    }
}
