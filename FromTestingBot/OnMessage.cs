using Order.Context;
using Order.Models;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using static Order.Enums;

namespace Order
{
    public static class OnMessage
    { 
        public static async void Bot_OnMessage(TelegramBotClient bot, MessageEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // получаем объекты из бд и выводим на консоль
                User user = await db.Users.FindAsync(e.Message.From.Id);
                switch (user.IsWait)
                {
                    case (int)WhatWait.Email:
                        e.Message.Text = "/editEmail " + e.Message.Text;
                        break;
                    case (int)WhatWait.Phone:
                        e.Message.Text = "/editPhone " + e.Message.Text;
                        break;
                    default:
                        e.Message.Text = "/";
                        break;
                }
                OnCommand.Bot_OnCommand(bot, e);
            }
            
        }

        
    }
}
