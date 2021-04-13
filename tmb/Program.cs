using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NModbus;

namespace tmb {
    public class ElektronikondataReader {
        public SlaveStorage Storage { get; }
        public int TaskNumber { get; }

        public ElektronikondataReader(int taskNumber, SlaveStorage storage) {
            Storage = storage;
            TaskNumber = taskNumber;
        }

        public static async Task ReadDataThread(object o) {
            ElektronikondataReader reader = (ElektronikondataReader) o;
            ushort elapsed = 0;
            int offset = 40001 + reader.TaskNumber * 500;
            while (true) {
                reader.Storage.InputRegisters[30011] = (ushort) (offset + 11 + elapsed);
                reader.Storage.InputRegisters[30012] = (ushort) (offset + 12 + elapsed);
                reader.Storage.InputRegisters[30013] = (ushort) (offset + 13 + elapsed);
                reader.Storage.InputRegisters[30014] = (ushort) (offset + 14 + elapsed);
                reader.Storage.InputRegisters[30015] = (ushort) (offset + 15 + elapsed);
                reader.Storage.InputRegisters[30016] = (ushort) (offset + 16 + elapsed);
                Console.WriteLine("TaskNumber={0}, elapsed={1}", reader.TaskNumber, elapsed);
                await Task.Delay(10000);
                elapsed += 10;
            }
        }
    }

    public class Program {
        private static void Main(string[] args) {
            const int port = 502;
            IPAddress localaddr = new IPAddress(new byte[] { 127, 0, 0, 1 });
            TcpListener slaveTcpListener = new TcpListener(localaddr, port);
            slaveTcpListener.Start();
            // NullModbusLogger.Instance = ;

            IModbusFactory factory = new ModbusFactory(null, true, new ReadLogger());
            IModbusSlaveNetwork network = factory.CreateSlaveNetwork(slaveTcpListener);
            List<string> ips = new List<string>{"192.168.8.200", "192.168.8.201", "192.168.8.202" };
            for (int i = 0; i < ips.Count; i++) {
                SlaveStorage storage = new SlaveStorage();
                ElektronikondataReader reader = new ElektronikondataReader(i, storage);
                Task.Factory.StartNew(ElektronikondataReader.ReadDataThread, reader);
                IModbusSlave slave = factory.CreateSlave((byte) i, storage);
                network.AddSlave(slave);
            }

            network.ListenAsync().GetAwaiter().GetResult();

            // prevent the main thread from exiting
            // Thread.Sleep(Timeout.Infinite);
        

            Console.WriteLine("Press any key to exit " + args);
            Console.ReadKey();
        }
    }
}