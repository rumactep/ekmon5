using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace airtgbot {
    class Program {
        
        static async Task Main(string[] args) {
            string n1 = "5069080381";
            string n2 = "AHCFv7nobSs7KDhIQPW5PlDsR1hwGBC0dM";
            var botClient = new TelegramBotClient(n1+ n2);
            Console.WriteLine($"bot № {n1}");

            using var cts = new CancellationTokenSource();

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Start listening for @{me.Username}");
            
            var receiverOptions = new ReceiverOptions {
                AllowedUpdates = { } // receive all update types
            };
            //botClient.OnM
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            //Console.CancelKeyPress += (_, _) => cts.Cancel();
            Console.WriteLine("Bot started. Press Enter to stop");
            //await Task.Delay(-1, cancellationToken: cts.Token); // Такой вариант советуют MS: https://github.com/dotnet/runtime/issues/28510#issuecomment-458139641
            //Console.WriteLine("Bot stopped");

            Console.ReadLine();
            cts.Cancel();

            Thread.Sleep(500);

        }
        // я chatid 1041976546
        // старая группа chatid -448703968
        // новая супергруппа chatid chatId -1001539497277
        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token) {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Type != UpdateType.Message)
                return;
            // Only process text messages
            if (update.Message!.Type != MessageType.Text)
                return;

            // все чужие чаты отключены
            var chatId = update.Message.Chat.Id;
            if (chatId != 1041976546 || chatId != -448703968 || chatId != -1001539497277) {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    disableNotification: true,
                    text: "Bot is working with white list only.",
                    cancellationToken: token);
                return;
            }

            var messageText = update.Message.Text;
            if (string.IsNullOrEmpty(messageText))
                return;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            //string firstName = update.Message.From.FirstName;
            //DateTime date = update.Message.Date.ToLocalTime();

            Message message = await botClient.SendTextMessageAsync(
                chatId: chatId,
                disableNotification: true,
                text: "Bot is not working now...\n",
                cancellationToken: token);
        }

        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
            var errorMessage = exception switch {
                ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}
