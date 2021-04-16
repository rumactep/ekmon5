using System;
using System.Collections.Generic;
using NModbus;

namespace ek2mb {

    // Discrete Inputs (CoilDiscretes) — дискретные входы, для чтения. 10001 по 19999. 
    // Coils (CoilInputs) — дискретные выходы, для чтения и записи. 20001 по 29999. 
    // Input Registers — 16-битные входы для чтения. 30001 по 39999
    // Holding Registers — 16-битные выходы для чтения и записи. 40001 по 49999
    public class ReadLogger : IModbusLogger {
        public void Log(LoggingLevel level, string message) {
            if (level == LoggingLevel.Information)
                Console.WriteLine("level: {0}, message: {1}", level, message);
        }

        public bool ShouldLog(LoggingLevel level) {
            switch (level) {
                case LoggingLevel.Critical: 
                case LoggingLevel.Debug:
                case LoggingLevel.Error:
                case LoggingLevel.Trace:
                case LoggingLevel.Warning: return false;
                case LoggingLevel.Information: return true;

                default: return false;
            }
        }
    }
    public static class FloatHelper {
        public static float Ushort2Float(ushort ush1, ushort ush2) {
            return BitConverter.ToSingle(BitConverter.GetBytes(((uint)ush2 << 16) + ush1), 0);
        }

        public static (ushort sh1, ushort sh2) Float2Ushort(float ff) {
            byte[] bb = BitConverter.GetBytes(ff);
            return (BitConverter.ToUInt16(bb, 0), BitConverter.ToUInt16(bb, 2));
        }
    }
    public class SlaveStorage : ISlaveDataStore {
        public SlaveStorage() {
            CoilDiscretes = new SparsePointSource<bool>(); // Discrete Inputs
            CoilInputs = new SparsePointSource<bool>(); // Coils
            InputRegisters = new SparsePointSource<ushort>();
            HoldingRegisters = new SparsePointSource<ushort>();
        }

        public SparsePointSource<bool> CoilDiscretes { get; } // Discrete Inputs
        public SparsePointSource<bool> CoilInputs { get; } // Coils
        public SparsePointSource<ushort> HoldingRegisters { get; }
        public SparsePointSource<ushort> InputRegisters { get; }

        public float this[ushort inputRegister] {
            get { return FloatHelper.Ushort2Float(InputRegisters[inputRegister], InputRegisters[(ushort)(inputRegister + 1)]); }
            set { (InputRegisters[inputRegister], InputRegisters[(ushort)(inputRegister + 1)]) = FloatHelper.Float2Ushort(value); }
        }

        IPointSource<bool> ISlaveDataStore.CoilDiscretes => CoilDiscretes;
        IPointSource<bool> ISlaveDataStore.CoilInputs => CoilInputs;
        IPointSource<ushort> ISlaveDataStore.HoldingRegisters => HoldingRegisters;
        IPointSource<ushort> ISlaveDataStore.InputRegisters => InputRegisters;
    }

    /// <summary>
    /// Sparse storage for points.
    /// </summary>
    public class SparsePointSource<TPoint> : IPointSource<TPoint> {
        private readonly Dictionary<ushort, TPoint> _values = new Dictionary<ushort, TPoint>();

        public event EventHandler<StorageEventArgs<TPoint>> StorageOperationOccurred;

        /// <summary>
        /// Gets or sets the value of an individual point wih tout 
        /// </summary>
        /// <param name="registerIndex"></param>
        /// <returns></returns>
        public TPoint this[ushort registerIndex] {
            get => _values.TryGetValue(registerIndex, out var value) ? value : default(TPoint);
            set => _values[registerIndex] = value;
        }

        public TPoint[] ReadPoints(ushort startAddress, ushort numberOfPoints) {
            var points = new TPoint[numberOfPoints];
            for (ushort i = 0; i < numberOfPoints; i++) {
                points[i] = this[(ushort) (i + startAddress)];
            }
            StorageOperationOccurred?.Invoke(this,
                new StorageEventArgs<TPoint>(PointOperation.Read, startAddress, points));
            return points;
        }

        public void WritePoints(ushort startAddress, TPoint[] points) {
            for (ushort i = 0; i < points.Length; i++) {
                this[(ushort) (i + startAddress)] = points[i];
            }
            StorageOperationOccurred?.Invoke(this,
                new StorageEventArgs<TPoint>(PointOperation.Write, startAddress, points));
        }
    }

    public class StorageEventArgs<TPoint> : EventArgs {
        public StorageEventArgs(PointOperation pointOperation, ushort startingAddress, TPoint[] points) {
            Operation = pointOperation;
            StartingAddress = startingAddress;
            Points = points;
        }

        public ushort StartingAddress { get; }

        public TPoint[] Points { get; }

        public PointOperation Operation { get; }
    }

    public enum PointOperation {
        Read,
        Write
    }
}