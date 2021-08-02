using Order.Commands;
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
                User user = await db.Users.FindAsync(e.Message.From.Id);
                // Использую для проверки состояния пользователя
                if (user != null)
                {
                    switch (user.IsWait)
                    {
                        case (int)UserWait.Email:
                            e.Message.Text = "/editEmail " + e.Message.Text;
                            break;
                        case (int)UserWait.Phone:
                            e.Message.Text = "/editPhone " + e.Message.Text;
                            break;
                        default:
                            e.Message.Text = "/";
                            break;
                    }
                    OnCommand.Bot_OnCommand(bot, e);
                }
                else
                {
                    UserCommands.NotRegister(bot, e);
                }              
            }
            
        }

        
    }
}
