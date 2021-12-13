﻿using System;
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
            var botClient = new TelegramBotClient("506908038:AHCFv7nobSs7KDhIQPW5PlDsR1hwGBC0dM");

            using var cts = new CancellationTokenSource();

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Start listening for @{me.Username}");



            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions {
                AllowedUpdates = { } // receive all update types
            };
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();

        }
        // я chatid 1041976546
        // группа chatid -448703968
        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Type != UpdateType.Message)
                return;
            // Only process text messages
            if (update.Message!.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            string firstName = update.Message.From.FirstName;
            DateTime date = update.Message.Date.ToLocalTime();

            Message message = await botClient.SendTextMessageAsync(
                chatId: chatId,
                disableNotification: true,
                text: $"You, {firstName}, {date} said:\n" + messageText,
                cancellationToken: cancellationToken);
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
