using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NModbus;

namespace ek2mb {

    public class ElektronikondataReader {
        public SlaveStorage Storage { get; }

        // 
        public int UnitId { get; }

        public ElektronikondataReader(int unitId, SlaveStorage storage) {
            Storage = storage;
            UnitId = unitId;
        }

        // карта регистров данных воздушного компрессора
        // перечень всех возможных параметров
        // 1 word номер компрессора
        // 2 word состояние связи с компрессором
        // 3 word состояние аварии
        // 4 word состояние компрессора
        // 5,6 float давление на выходе
        // 7,8 float выход ступени компрессора
        // 9,10 float точка росы осушителя 

        public async Task ReadDataThreadAsync() {
            ushort elapsed = 0;
            int offset = 4000 + UnitId * 500;
            while (true) {
                SlaveStorage storage = reader.Storage;
                storage.InputRegisters[4064] = (ushort) -(offset + 4 + elapsed);
                storage.InputRegisters[4065] = (ushort) (offset + 5 + elapsed);
                storage.InputRegisters[4066] = (ushort) (offset + 6 + elapsed);
                storage.InputRegisters[4067] = (ushort) (offset + 7 + elapsed);

                storage[4000] = 18.8f;
                Debug.Assert(Math.Abs(18.8f - FloatHelper.Ushort2Float(storage.InputRegisters[4000], storage.InputRegisters[4001])) < 0.001);
                Console.WriteLine("TaskNumber={0}, elapsed={1}", UnitId, elapsed);
                await Task.Delay(10000);
                elapsed += 10;
            }        
        }

         public static async Task StaticReadDataThreadAsync(object o) {
            ElektronikondataReader reader = (ElektronikondataReader) o;
            await reader.ReadDataThreadAsync();            
        }
    }
    public class CompressorInfo {
        public int cnumber { get; set; }
        public string cip { get; set; }

        public override string ToString() {
            return $"cnumber: {cnumber}, cip: {cip}";
        }
    }

    public class Program {
        private static void Main(string[] args) {
            List<CompressorInfo> compressors = ReadCompressorList();
            const int port = 502;
            IPAddress localaddr = new IPAddress(new byte[] { 127, 0, 0, 1 });
            TcpListener slaveTcpListener = new TcpListener(localaddr, port);
            slaveTcpListener.Start();
            // NullModbusLogger.Instance = ;

            IModbusFactory factory = new ModbusFactory(null, true, new ReadLogger());
            IModbusSlaveNetwork network = factory.CreateSlaveNetwork(slaveTcpListener);
            // List<string> ips = new List<string>{"192.168.8.200", "192.168.8.201", "192.168.8.202" };
            for (int i = 0; i < compressors.Count; i++) {
                SlaveStorage storage = new SlaveStorage();
                ElektronikondataReader reader = new ElektronikondataReader(i + 1, storage);
                Task.Factory.StartNew(ElektronikondataReader.StaticReadDataThreadAsync, reader);
                IModbusSlave slave = factory.CreateSlave((byte) (i + 1), storage);
                network.AddSlave(slave);
            }

            network.ListenAsync().GetAwaiter().GetResult();

            // prevent the main thread from exiting
            // Thread.Sleep(Timeout.Infinite);
        

            Console.WriteLine("Press any key to exit " + args);
            Console.ReadKey();
        }

        private static List<CompressorInfo> ReadCompressorList() {
            string jsonText = @"[{cnumber:4,cip:""192.168.11.208""}, {cnumber:5,cip:""192.168.11.209""}, {cnumber:8,cip:""192.168.11.211""}, {cnumber:10,cip:""192.168.11.210""}, {cnumber:12,cip:""192.168.11.207""}, {cnumber:13,cip:""192.168.11.212""}, {cnumber:14,cip:""192.168.11.221""}]";
            return JsonConvert.DeserializeObject<List<CompressorInfo>>(jsonText);
        }
    }
}