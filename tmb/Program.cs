using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NModbus;

namespace ek2mb {
    public class ElektronikondataReader {
        public SlaveStorage Storage { get; }
        public int TaskNumber1 { get; }

        // taskNumber1 
        public ElektronikondataReader(int taskNumber1, SlaveStorage storage) {
            Storage = storage;
            TaskNumber1 = taskNumber1;
        }

        public static async Task ReadDataThread(object o) {
            ElektronikondataReader reader = (ElektronikondataReader) o;
            ushort elapsed = 0;
            int offset = 4000 + reader.TaskNumber1 * 500;
            while (true) {
                SlaveStorage storage = reader.Storage;
                storage.InputRegisters[4064] = (ushort) -(offset + 4 + elapsed);
                storage.InputRegisters[4065] = (ushort) (offset + 5 + elapsed);
                storage.InputRegisters[4066] = (ushort) (offset + 6 + elapsed);
                storage.InputRegisters[4067] = (ushort) (offset + 7 + elapsed);

                storage[4000] = 18.8f;
                Debug.Assert(Math.Abs(18.8f - FloatHelper.Ushort2Float(storage.InputRegisters[4000], storage.InputRegisters[4001])) < 0.001);
                Console.WriteLine("TaskNumber1={0}, elapsed={1}", reader.TaskNumber1, elapsed);
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
                ElektronikondataReader reader = new ElektronikondataReader(i + 1, storage);
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