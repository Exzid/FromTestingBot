using Order.Context;
using Order.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using Order.Commands;
using Telegram.Bot.Types;

namespace Order
{
    public static class OnCommand
    {
        public static async void Bot_OnCommand(object sender, MessageEventArgs e)
        {
            
                TelegramBotClient bot = (TelegramBotClient)sender;
            if (e.Message.Text != null && e.Message.Text[0] == '/')
            {
                switch (e.Message.Text.Split(" ")[0])
                {
                    //UserCommands

                    case "/start":
                        UserCommands.Start(bot, e);
                        break;

                    case "/cancel":
                        UserCommands.Cancel(bot, e);
                        break;
                    //menu
                    case "/menu":
                        UserCommands.Menu(bot, e);
                        break;
                    case "/getData":
                        UserCommands.GetData(bot, e);
                        break;
                    case "/editEmail":
                        UserCommands.EditEmail(bot, e);
                        break;
                    case "/editPhone":
                        UserCommands.EditPhone(bot, e);
                        break;
                    case "/infoChannel":
                        UserCommands.InfoChannel(bot, e);
                        break;
                    case "/payments":
                        UserCommands.Payments(bot, e);
                        break;

                    //offers
                    case "/offer":
                        UserCommands.Offer(bot, e);
                        break;
                    case "/rate":
                        UserCommands.Rate(bot, e);
                        break;

                    //AdminCommands
                    case "/test":
                        AdminCommands.Test(bot, e);
                        break;
                    case "/admin/users":
                        AdminCommands.AllUsers(bot, e);
                        break;
                    case "/admin/editRate":
                        AdminCommands.EditRate(bot, e);
                        break;
                    case "/admin/addAdmin":
                        AdminCommands.AddAdmin(bot, e);
                        break;
                    case "/admin/removeAdmin":
                        AdminCommands.RemoveAdmin(bot, e);
                        break;

                    //Voximplant
                    case "/voximplant/test":
                        VoximplantCommands.Test(bot, e);
                        break;
                    default:
                        UserCommands.Default(bot, e);
                        break;
                }
            }
            else
            {
                if (e.Message.SuccessfulPayment != null)
                {
                    UserCommands.SuccessfulPayment(bot, e);
                }
                else
                {
                    OnMessage.Bot_OnMessage(bot, e);
                }
            }
        }
    }
}
