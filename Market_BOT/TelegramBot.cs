using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Market_BOT
{
    public class TelegramBot
    {
        private static ObservableCollection<Coffee> output { get; set; } = new ObservableCollection<Coffee>();
        private static TelegramBotClient botClient;
        private const string TOKEN = "1990458778:AAHT0uJWG4Reo-6xlOttIPNXVZ7AWQgyvJs";
        public TelegramBot()
        {
            LoadProduct();
            botClient = new TelegramBotClient(TOKEN);
            botClient.OnMessage += BotClient_OnMessage;
            botClient.OnCallbackQuery += BotClient_OnCallbackQuery;
            botClient.StartReceiving();
            Console.ReadLine();
            botClient.StopReceiving();
        }

        private void LoadProduct()
        {
            Server.ConnectServer($"LoadData");
            output = Server.output;
        }

        private static async void BotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg.Text.Equals("/start"))
            {
                var inlineKeyBoard = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Yes"),
                            InlineKeyboardButton.WithCallbackData("No"),
                        }
                    });
                await botClient.SendTextMessageAsync(msg.From.Id, "Welcome to Coffee-Market (BOT)\nDo you want to order something?", replyMarkup: inlineKeyBoard);
            }
        }

        private static async void BotClient_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            async void DeleteMsg() => await botClient.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
            var msg = e.CallbackQuery;
            string buttonText = e.CallbackQuery.Data;

            if (buttonText.Equals("Yes"))
            {
                DeleteMsg();
                Console.WriteLine($"User { msg.Message.Chat.Username } press {buttonText}");

                var inlineKeyBoard = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Coffee"),
                        }
                    });
                await botClient.SendTextMessageAsync(msg.From.Id, "Choose the product you need", replyMarkup: inlineKeyBoard);
            }
            else if (buttonText.Equals("No"))
            {
                DeleteMsg();
                Console.WriteLine($"User { msg.Message.Chat.Username } press {buttonText}");
                await botClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "To bring up the menu: /start");
            }
            else if (buttonText.Equals("Coffee"))
            {
                DeleteMsg();

                await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Coffee Market");

                foreach (var item in output)
                {
                    var inlineKeyBoard = new InlineKeyboardMarkup(new[]
                    { new []{ InlineKeyboardButton.WithCallbackData($"{item.ToString()}")} });
                    await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, $"Name: {item.Name}\nPrice: {item.Price}$\nTo buy this product, click on the button below.", replyMarkup: inlineKeyBoard);
                }
            }

            foreach (var item in output)
            {
                if (buttonText.Equals(item.ToString()))
                {
                    Console.WriteLine($"User { msg.Message.Chat.Username } buy product {item.ToString()}");
                    await botClient.SendTextMessageAsync(msg.From.Id, $"You have successfully bought: {item.Name}");
                    Server.ConnectServer($"SellTG/{e.CallbackQuery.Message.Chat.Username}/{item.Name}/{item.Price}");
                }
            }
        }
    }
}
