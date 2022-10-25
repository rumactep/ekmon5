using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using NModbus;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace smartlink;

public class CompressorInfo {
    // UnitId - номер устройства в модбасе
    public byte UnitId { get; set; }

    // Cnumber - номер компрессора - для понимания человеком, сейчас совпадает с номермо усройства модбаса
    public ushort Cnumber { get; set; }
    public string Cip { get; set; } = string.Empty;

    public override string ToString() {
        return $"Cnumber: {Cnumber}, UnitId: {UnitId}, Cip: {Cip}";
    }
}

public class SmartlinkserverApp {
    const int PORT_MODBUS = 502;
    readonly string n1 = "5069080381";
    readonly string n2 = "AHCFv7nobSs7KDhIQPW5PlDsR1hwGBC0dM";
    List<SlaveStorage> _storages = new();

    public static void Main() { 
        new SmartlinkserverApp().Run();
    }

    public void Run() {
        //Console.OutputEncoding = Encoding.UTF8;
        IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        foreach (IPAddress address in addressList)
            Console.WriteLine($"Найден локальный ip: {address}");
        //TcpListener tcpListener = new(IPAddress.Any, PORT_MODBUS);
        IPAddress localAddr = IPAddress.Parse("0.0.0.0");
        TcpListener tcpListener = new(localAddr, PORT_MODBUS);
        Console.WriteLine($"starting TcpListener for Modbus on port: {PORT_MODBUS}");
        ReadLogger logger = new();
        IModbusFactory factory = new ModbusFactory(null, true, logger);
        using IModbusSlaveNetwork network = factory.CreateSlaveNetwork(tcpListener);

        ManualResetEvent mainExitEvent = new(false);
        List<CompressorInfo> compressorInfos = ReadCompressorList();
        ManualResetEvent[] workEndEvents = new ManualResetEvent[compressorInfos.Count];

        for (int i = 0; i < compressorInfos.Count; i++) {
            CompressorInfo info = compressorInfos[i];
            SlaveStorage storage = new(info);
            _storages.Add(storage);

            workEndEvents[i] = new ManualResetEvent(false);
            Worker work = new Worker(mainExitEvent, workEndEvents[i], info, storage);
            Thread workThread = new Thread(new ThreadStart(work.ThreadProc));
            workThread.Start();
            Thread.Sleep(200);
            IModbusSlave slave = factory.CreateSlave(info.UnitId, storage);
            network.AddSlave(slave);
        }

        // попытка считать актуальные значения, прежде чем заработает отдача данных в Modbus
        Thread.Sleep(5000);

        tcpListener.Start();
        network.ListenAsync();

        using var cts = new CancellationTokenSource();

        StartBot(cts);

        Console.WriteLine("Press Enter to exit");
        Console.ReadLine();

        cts.Cancel();

        tcpListener.Stop();
        mainExitEvent.Set();
        WaitHandle.WaitAll(workEndEvents);
        Console.WriteLine("All exited!");
    }

    string GetFullWorkdataString() {
        string result = "";
        foreach (var storage in _storages) {
            result += GetStorageState(storage);
        }
        return result;
    }

    string GetStateString(ushort state) {
        return state switch {
            // ❌❎⚡🔥😒😡👈 👉🛌💤🛢
            0 => "👉нет связи с ВК",
            3 => "⚡стоп",
            5 => "💤Oжидaниe",
            13 => "❗АВАРИЯ",
            16 => "⁉Запускается",
            91 => "⁉Пуск отложен",
            18 => "разгрузка",
            28 => "✅ок",
            _ => $"⛔ состояние {state}",
        };
    }

    string GetTimeString(ushort time) {
        var (t1, t2, t3) = (time / 1000, (time % 1000) / 10, (time % 10) * 10);
        string strtime = $"{t1}:{t2}";
        return strtime;
    }

    string GetStorageState(SlaveStorage storage) {
        var time = GetTimeString(storage.Time);
        var state = GetStateString(storage.WorkState);
        return $"№{storage.Number}, {time}:{state}, {storage.Pressure}бар, {storage.Temperature}°С, {storage.Flow}%\n";
    }

    async void StartBot(CancellationTokenSource cts) {
        var botClient = new TelegramBotClient(n1 + n2);
        Console.WriteLine($"bot № {n1}");

        var me = await botClient.GetMeAsync();
        Console.WriteLine($"Start listening for @{me.Username}");

        var receiverOptions = new ReceiverOptions {AllowedUpdates = { } };
        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: cts.Token);

        Console.WriteLine("Bot started. Press Enter to stop");
    }

    // я chatid 1041976546
    // старая группа chatid -448703968
    // новая супергруппа chatid chatId -1001539497277
    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token) {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Type != UpdateType.Message)
            return;
        // Only process text messages
        if (update.Message!.Type != MessageType.Text)
            return;

        var chatId = update.Message.Chat.Id;

         if (chatId == -1001539497277) {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                disableNotification: true,
                text: "Пишите боту vmAirBotName в личку",
                cancellationToken: token);
        }

        var messageText = update.Message.Text;
        if (string.IsNullOrEmpty(messageText))
            return;

        var strmessage = GetFullWorkdataString();
        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}. My answer is:\n{strmessage}");

        Message message2 = await botClient.SendTextMessageAsync(
            chatId: chatId,
            disableNotification: true,
            text: strmessage,
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

    private static List<CompressorInfo> ReadCompressorList() {
        // формат следующий:
        // UnitId - номер устройства в модбасе
        // Cnumber - номер компрессора - для понимания человеком, совпадает с номером модбаса
        // ip-адрес компа в сети
        string jsonText = @"[{UnitId:4,cnumber:4,cip:""192.168.11.208""}, {UnitId:5,cnumber:5,cip:""192.168.11.209""}, {UnitId:8,cnumber:8,cip:""192.168.11.211""}, {UnitId:11,cnumber:11,cip:""192.168.11.210""}, {UnitId:12,cnumber:12,cip:""192.168.11.207""}, {UnitId:13,cnumber:13,cip:""192.168.11.212""}, {UnitId:14,cnumber:14,cip:""192.168.11.221""}, {UnitId:16,cnumber:16,cip:""192.168.11.226""}]";
        // string jsonText = @"[{UnitId:4,cnumber:4,cip:""192.168.11.208""}]";
        //string jsonText = @"[{UnitId:11,cnumber:11,cip:""192.168.11.210""}]";
        var list = JsonConvert.DeserializeObject<List<CompressorInfo>>(jsonText)!;
        Console.WriteLine(JsonConvert.SerializeObject(list));
        return list;
    }
}

