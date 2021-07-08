using MongoDB.Bson;
using MongoDB.Driver;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Wecker
{
    public static class OnCommand
    {
        static TelegramBotClient bot;
        public enum WaitToAnswer 
        {
            No = 0,
            EditName,
            EditAge,
            EditRegion
        }

        public static async void Bot_OnCommand(object sender, MessageEventArgs e)
        {
            bot = (TelegramBotClient)sender;
            if (e.Message.Text != null && e.Message.Text[0] == '/')
            {
                switch (e.Message.Text)
                {
                    case "/start":
                        Start(bot, e);
                        break;
                    case "/editName":
                        EditName(bot, e);
                        break;
                    case "/editGender":
                        EditGenderKeyboard(bot, e);
                        break;
                    case "/editAge":
                        EditAgeKeyboard(bot, e);
                        break;
                    case "/editRegion":
                        EditRegionKeyboard(bot, e);
                        break;

                    case "/main":
                        MainKeyboard(bot, e);
                        break;
                    case "/whoCall":
                        WhoCallKeyboard(bot, e);
                        break;
                    case "/call":
                        CallKeyboard(bot, e);
                        break;
                    case "/stats":
                        StatsKeyboard(bot, e);
                        break;
                    case "/endCallContact":
                        EndCallContact(bot, e);
                        break;
                    case "/orderCallPerson":
                        OrderCallPerson(bot, e);
                        break;
                    case "/orderCallBot":
                        OrderCallBot(bot, e);
                        break;
                    case "/requestAccepted":
                        RequestAccepted(bot, e);
                        break;
                    case "/endCallRate":
                        EndCallRate(bot, e);
                        break;
                    case "/endCallGetContact":
                        EndCallGetContact(bot, e);
                        break;
                    case "/OrderCallComment":
                        OrderCallComment(bot, e);
                        break;

                    case "/developing":
                        Developing(bot, e);
                        break;
                    default:
                        Default(bot, e);
                        break;
                }
            }
            else
            {
                OnMessage.Bot_OnMessage(bot, e);
            }
        }

        ///
        static async void WhoCallKeyboard(TelegramBotClient bot, MessageEventArgs e)
        {

            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Мужчина", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Женщина", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Бот", "/developing"),
                        },
                });

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Заказ звонка: Кто вам позвонит?",
                replyMarkup: button);
        }

        static async void CallKeyboard(TelegramBotClient bot, MessageEventArgs e)
        {

            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Отказаться", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Главное меню", "main"),
                        },

                });

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text:  $"Имя: {"Всё"}"
                     + $"Дата: {"Ещё"}"
                     + $"Действие: {"В разработке"}",//developing
                replyMarkup: button);
        }

        static async void StatsKeyboard(TelegramBotClient bot, MessageEventArgs e)
        {

            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Главное меню", "/main"),
                        },
                });

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: $"Исходящие: {"Всё"}"
                     + $"Входящие: {"Ещё"}"
                     + $"Рейтинг: {"В разработке"}",//developing
                replyMarkup: button);
        }

        static async void EndCallContact(TelegramBotClient bot, MessageEventArgs e)
        {
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "окончание звонка ( этот блок всё ещё в разработке )");//developing
        }

        static async void OrderCallPerson(TelegramBotClient bot, MessageEventArgs e)
        {

            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Заказать звонок", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Позвонить", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Статистика", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Редактировать профиль", "/editName"),
                        }
                });

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Выбор действия",
                replyMarkup: button);
        }

        static async void OrderCallBot(TelegramBotClient bot, MessageEventArgs e)
        {

            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Заказать звонок", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Позвонить", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Статистика", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Редактировать профиль", "/editName"),
                        }
                });

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Выбор действия",
                replyMarkup: button);
        }

        static async void RequestAccepted(TelegramBotClient bot, MessageEventArgs e)
        {

            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Заказать звонок", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Позвонить", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Статистика", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Редактировать профиль", "/editName"),
                        }
                });

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Выбор действия",
                replyMarkup: button);
        }

        static async void EndCallRate(TelegramBotClient bot, MessageEventArgs e)
        {

            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Заказать звонок", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Позвонить", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Статистика", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Редактировать профиль", "/editName"),
                        }
                });

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Выбор действия",
                replyMarkup: button);
        }

        static async void EndCallGetContact(TelegramBotClient bot, MessageEventArgs e)
        {

            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Заказать звонок", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Позвонить", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Статистика", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Редактировать профиль", "/editName"),
                        }
                });

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Выбор действия",
                replyMarkup: button);
        }

        static async void OrderCallComment(TelegramBotClient bot, MessageEventArgs e)
        {

            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Заказать звонок", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Позвонить", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Статистика", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Редактировать профиль", "/editName"),
                        }
                });

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Выбор действия",
                replyMarkup: button);
        }
        ///

        static async void Start(TelegramBotClient bot, MessageEventArgs e)
        {
            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Продолжить", "/editName"),
                        }
                });
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Приветственное сообщение",
                replyMarkup: button);
        }

        static async void MainKeyboard(TelegramBotClient bot, MessageEventArgs e)
        {

            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Заказать звонок", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Позвонить", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Статистика", "/developing"),
                        },
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Редактировать профиль", "/editName"),
                        }       
                });

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text:"Выбор действия",
                replyMarkup: button);
        }

        static async void Developing(TelegramBotClient bot, MessageEventArgs e)
        {
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Данная команда находится на этапе разработки");
        }

        static async void EditName(TelegramBotClient bot, MessageEventArgs e)
        {

            BsonDocument filter = new BsonDocument("_id", e.Message.Chat.Id);
            BsonDocument update = new BsonDocument("$set", new BsonDocument("nextIsAnswer", WaitToAnswer.EditName));
            UpdateOptions options = new UpdateOptions { IsUpsert = true };

            await Program.waitDb.UpdateOneAsync(filter, update, options);

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Как вас зовут?");
        }

        static async void EditAgeKeyboard(TelegramBotClient bot, MessageEventArgs e)
        {

            BsonDocument filter = new BsonDocument("_id", e.Message.Chat.Id);
            BsonDocument update = new BsonDocument("$set", new BsonDocument("nextIsAnswer", WaitToAnswer.EditAge));
            UpdateOptions options = new UpdateOptions { IsUpsert = true };

            await Program.waitDb.UpdateOneAsync(filter, update, options);

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Введите свой возраст");
        }

        static async void EditRegionKeyboard(TelegramBotClient bot, MessageEventArgs e)
        {

            BsonDocument filter = new BsonDocument("_id", e.Message.Chat.Id);
            BsonDocument update = new BsonDocument("$set", new BsonDocument("nextIsAnswer", WaitToAnswer.EditRegion));
            UpdateOptions options = new UpdateOptions { IsUpsert = true };

            await Program.waitDb.UpdateOneAsync(filter, update, options);

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Введите свой регион");
        }

        static async void EditGenderKeyboard(TelegramBotClient bot, MessageEventArgs e)
        {
            var button = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Мужчина", "/gender мужчина"),
                            InlineKeyboardButton.WithCallbackData("Женщина", "/gender женщина"),
                        }
                });

            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Укажите ваш пол",
                replyMarkup: button);
        }

        static async void Default(TelegramBotClient bot, MessageEventArgs e)
        {
            await bot.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Я не знаю такой команды");
        }
    }
}
