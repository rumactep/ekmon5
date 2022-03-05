// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using NModbus;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using smartlinkserver;

App2.Main();

public class App2 {
    const int PORT_MODBUS = 502;
    public static void Main() {
        List<CompressorInfo> compressorInfos = ReadCompressorList();
        IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        foreach (IPAddress address in addressList)
            Console.WriteLine($"found local ip: {address}");
        TcpListener tcpListener = new(IPAddress.Any, PORT_MODBUS);
        Console.WriteLine($"starting TcpListener for Modbus on port: {PORT_MODBUS}");
        ReadLogger logger = new();
        IModbusFactory factory = new ModbusFactory(null, true);
        using IModbusSlaveNetwork network = factory.CreateSlaveNetwork(tcpListener);
        for (int i = 0; i < compressorInfos.Count; i++) {
            CompressorInfo info = compressorInfos[i];
            SlaveStorage storage = new(info);

            Task task = Task.Run(async () =>  {
                await Task.Delay(i * 1000);
                        Console.WriteLine($"started working on {info}");

                    }
                );
            task.ContinueWith(t => {
                Console.WriteLine($"task {info} stopped");
            });
            IModbusSlave slave = factory.CreateSlave(info.UnitId, storage);
            network.AddSlave(slave);

        }
        Thread.Sleep(1000);
        tcpListener.Start();
        network.ListenAsync();
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        tcpListener.Stop();
        

    }

    private static List<CompressorInfo> ReadCompressorList() {
        // string jsonText = @"[{UnitId:4,cnumber:4,cip:""192.168.11.208""}, {UnitId:5,cnumber:5,cip:""192.168.11.209""}, {UnitId:8,cnumber:8,cip:""192.168.11.211""}, {UnitId:10,cnumber:10,cip:""192.168.11.210""}, {UnitId:12,cnumber:12,cip:""192.168.11.207""}, {UnitId:13,cnumber:13,cip:""192.168.11.212""}, {UnitId:14,cnumber:14,cip:""192.168.11.221""}]"; 
        string jsonText = @"[{UnitId:4,cnumber:4,cip:""192.168.11.208""}]";
        return JsonConvert.DeserializeObject<List<CompressorInfo>>(jsonText)!;
    }
}

