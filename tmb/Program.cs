using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        static float Ushort2Float(ushort sh1, ushort sh2) {
            return BitConverter.ToSingle(BitConverter.GetBytes(((uint)sh2 << 16) + sh1), 0);
        }

        static (ushort sh1, ushort sh2) Float2Ushort(float ff) {
            byte[] bb = BitConverter.GetBytes(ff);
            return (BitConverter.ToUInt16(bb, 0), BitConverter.ToUInt16(bb, 2));
        }


        public static async Task ReadDataThread(object o) {
            ElektronikondataReader reader = (ElektronikondataReader) o;
            ushort elapsed = 0;
            int offset = 4000 + reader.TaskNumber * 500;
            while (true) {
                SparsePointSource<ushort> registers = reader.Storage.InputRegisters;
                registers[4064] = (ushort) (offset + 4 + elapsed);
                registers[4065] = (ushort) (offset + 5 + elapsed);
                registers[4066] = (ushort) (offset + 6 + elapsed);
                registers[4067] = (ushort) (offset + 7 + elapsed);

                (registers[4000], registers[4001]) = Float2Ushort(18.8f);
                Debug.Assert(Math.Abs(18.8f - Ushort2Float(registers[4000], registers[4001])) < 0.001);
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