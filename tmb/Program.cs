using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NModbus;

namespace ek2mb {
    public class CompressorInfo {
        // UnitId - номер устройства в модбасе
        public byte UnitId { get; set; }

        // Cnumber - номер компрессора - для понимания человеком
        public ushort Cnumber { get; set; }
        public string Cip { get; set; }

        public override string ToString() {
            return $"Cnumber: {Cnumber}, UnitId: {UnitId}, Cip: {Cip}";
        }
    }

    public class Program {
        const int PORT_MODBUS = 502;
        private static void Main(string[] args) {
            List<CompressorInfo> compressorInfos = ReadCompressorList();
            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (IPAddress address in addressList) 
                Console.WriteLine($"found local ip: {address}");
            TcpListener tcpListener = new TcpListener(IPAddress.Any, PORT_MODBUS);
            Console.WriteLine($"starting TcpListener for Modbus on port: {PORT_MODBUS}");
            ReadLogger logger = new ReadLogger();
            IModbusFactory factory = new ModbusFactory(null, true);
            IModbusSlaveNetwork network = factory.CreateSlaveNetwork(tcpListener);
            for (int i = 0; i < compressorInfos.Count; i++) {
                var info = compressorInfos[i];
                var storage = new SlaveStorage(info);
                ElektronikonReader reader = new ElektronikonReader(storage, i);
                
                Task.Factory.StartNew(ElektronikonReader.StaticReadDataThreadAsync, reader);

                Thread.Sleep(2000);
                IModbusSlave slave = factory.CreateSlave(info.UnitId, storage);
                network.AddSlave(slave);
            }
            Thread.Sleep(5000);
            tcpListener.Start();
            //network.ListenAsync().GetAwaiter().GetResult();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();


        }

        private static List<CompressorInfo> ReadCompressorList() {
            // string jsonText = @"[{UnitId:4,cnumber:4,cip:""192.168.11.208""}, {UnitId:5,cnumber:5,cip:""192.168.11.209""}, {UnitId:8,cnumber:8,cip:""192.168.11.211""}, {UnitId:10,cnumber:10,cip:""192.168.11.210""}, {UnitId:12,cnumber:12,cip:""192.168.11.207""}, {UnitId:13,cnumber:13,cip:""192.168.11.212""}, {UnitId:14,cnumber:14,cip:""192.168.11.221""}]"; 
            string jsonText = @"[{UnitId:4,cnumber:4,cip:""192.168.11.208""}]";
            return JsonConvert.DeserializeObject<List<CompressorInfo>>(jsonText);
        }
    }
}