using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Order
{
    public static class OnMessage
    { 
        public static async void Bot_OnMessage(TelegramBotClient bot, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
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
