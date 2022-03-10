// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using NModbus;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace  smartlinkserver;

public class SmartlinkserverApp {
    const int PORT_MODBUS = 502;
    public static void Main() {
        List<CompressorInfo> compressorInfos = ReadCompressorList();
        IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        foreach (IPAddress address in addressList)
            Console.WriteLine($"found local ip: {address}");
        TcpListener tcpListener = new(IPAddress.Any, PORT_MODBUS);
        Console.WriteLine($"starting TcpListener for Modbus on port: {PORT_MODBUS}");
        ReadLogger logger = new();
        IModbusFactory factory = new ModbusFactory(null, true, logger);
        using IModbusSlaveNetwork network = factory.CreateSlaveNetwork(tcpListener);

        ManualResetEvent mainExitEvent = new(false);
        int THREAD_COUNT = compressorInfos.Count;
        ManualResetEvent[] workEndEvents = new ManualResetEvent[THREAD_COUNT];

        for (int i = 0; i < compressorInfos.Count; i++) {
            CompressorInfo info = compressorInfos[i];
            SlaveStorage storage = new(info);

            workEndEvents[i] = new ManualResetEvent(false);
            Worker work = new Worker(mainExitEvent, workEndEvents[i], info, storage);
            Thread workThread = new Thread(new ThreadStart(work.ThreadProc));
            workThread.Start();
            Thread.Sleep(1000);
            IModbusSlave slave = factory.CreateSlave(info.UnitId, storage);
            network.AddSlave(slave);
        }

        // попытка считать актуальные значения, прежде чем заработает отдача данных в Modbus
        Thread.Sleep(5000);

        tcpListener.Start();
        network.ListenAsync();
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        tcpListener.Stop();
        mainExitEvent.Set();
        WaitHandle.WaitAll(workEndEvents);
        Console.WriteLine("All exited!");
    }

    private static List<CompressorInfo> ReadCompressorList() {
        string jsonText = @"[{UnitId:4,cnumber:4,cip:""192.168.11.28""}, {UnitId:5,cnumber:5,cip:""192.168.11.209""}, {UnitId:8,cnumber:8,cip:""192.168.11.211""}, {UnitId:10,cnumber:10,cip:""192.168.11.210""}, {UnitId:12,cnumber:12,cip:""192.168.11.207""}, {UnitId:13,cnumber:13,cip:""192.168.11.212""}, {UnitId:14,cnumber:14,cip:""192.168.11.221""}]"; 
        //string jsonText = @"[{UnitId:4,cnumber:4,cip:""192.168.11.208""}]";
        return JsonConvert.DeserializeObject<List<CompressorInfo>>(jsonText)!;
    }
}

