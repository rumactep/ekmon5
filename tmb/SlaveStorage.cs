using System;
using System.Collections.Generic;
using NModbus;

namespace ek2mb {
    // используем InputRegisters
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
                case LoggingLevel.Warning: return true; 
                case LoggingLevel.Information: return false;

                default: return false;
            }
        }
    }
    public static class FloatHelper {
        public static float Ushort2Float((ushort ush1, ushort ush2) ush) {
            return Ushort2Float(ush.ush1, ush.ush2);
        }
        public static float Ushort2Float(ushort ush1, ushort ush2) {
            return BitConverter.ToSingle(BitConverter.GetBytes(((uint)ush2 << 16) + ush1), 0);
        }

        public static (ushort sh1, ushort sh2) Float2Ushort(float ff) {
            byte[] bb = BitConverter.GetBytes(ff);
            return (BitConverter.ToUInt16(bb, 0), BitConverter.ToUInt16(bb, 2));
        }
    }

    // карта регистров данных воздушного компрессора
    // перечень всех возможных параметров
    // используем InputRegisters
    // 1 word номер компрессора
    // 2 word состояние компрессора. 0 - (неизвестно, нет связи) 1 - авария, 2 - стоп, 3 - разгрузка, 4 загрузка
    // 3 word Pacxoд (в процентах)
    // 4 word Time
    // 5 6 float давление на выходе, бар
    // 7 8 float выход ступени компрессора

    public class SlaveStorage : ISlaveDataStore {
        public SlaveStorage(CompressorInfo info) {
            Info = info;
            CoilDiscretes = new SparsePointSource<bool>(); // Discrete Inputs
            CoilInputs = new SparsePointSource<bool>(); // Coils
            InputRegisters = new SparsePointSource<ushort>();
            HoldingRegisters = new SparsePointSource<ushort>();
            Number = info.Cnumber;
        }

        public enum CompressorTags : ushort {
            Number = 1,
            WorkState = 2,
            Flow = 3,
            Time = 4,
            Pressure = 5,
            Temperature = 7,
        }
        /// номер компрессора
        public ushort Number {
            get => InputRegisters[(ushort)CompressorTags.Number];
            set => InputRegisters[(ushort)CompressorTags.Number] = value;
        }

        public ushort WorkState {
            get => InputRegisters[(ushort)CompressorTags.WorkState];
            set => InputRegisters[(ushort)CompressorTags.WorkState] = value;
        }

        public ushort Flow {
            get => InputRegisters[(ushort)CompressorTags.Flow];
            set => InputRegisters[(ushort)CompressorTags.Flow] = value;
        }

        public float Pressure {
            get => this[(ushort)CompressorTags.Pressure];
            set => this[(ushort)CompressorTags.Pressure] = value;
        }

        public float Temperature {
            get => this[(ushort)CompressorTags.Temperature];
            set => this[(ushort)CompressorTags.Temperature] = value;
        }

        public ushort Time {
            get => InputRegisters[(ushort)CompressorTags.Time];
            set => InputRegisters[(ushort)CompressorTags.Time] = value;
        }

        public CompressorInfo Info { get; }
        protected SparsePointSource<bool> CoilDiscretes { get; } // Discrete Inputs
        protected SparsePointSource<bool> CoilInputs { get; } // Coils
        protected SparsePointSource<ushort> HoldingRegisters { get; }
        public SparsePointSource<ushort> InputRegisters { get; }

        protected float this[ushort register] {
            get => FloatHelper.Ushort2Float(InputRegisters.GetTwoValues(register));
            set {
                (ushort ush1, ushort ush2) = FloatHelper.Float2Ushort(value);
                InputRegisters.SetTwoValues(register, ush1, ush2);
            }
        }

        IPointSource<bool> ISlaveDataStore.CoilDiscretes => CoilDiscretes;
        IPointSource<bool> ISlaveDataStore.CoilInputs => CoilInputs;
        IPointSource<ushort> ISlaveDataStore.HoldingRegisters => HoldingRegisters;
        IPointSource<ushort> ISlaveDataStore.InputRegisters => InputRegisters;
    }

    public class SparsePointSource<TPoint> : IPointSource<TPoint> {
        private readonly object _valuesLock = new object();
        private readonly Dictionary<ushort, TPoint> _values = new Dictionary<ushort, TPoint>();

        public event EventHandler<StorageEventArgs<TPoint>> StorageOperationOccurred;

        public TPoint this[ushort registerIndex] {
            get {
                lock (_valuesLock) 
                    return _values.TryGetValue(registerIndex, out var value) ? value : default;
            }
            set {
                lock (_valuesLock)
                    _values[registerIndex] = value;
            }
        }

        public (TPoint, TPoint) GetTwoValues(ushort registerIndex) {
            // lock (_valuesLock) 
                return (this[registerIndex], this[(ushort) (registerIndex + 1)]);
        }

        public void SetTwoValues(ushort registerIndex, TPoint value1, TPoint value2) {
            // lock (_valuesLock) {
                _values[registerIndex] = value1;
                _values[(ushort) (registerIndex + 1)] = value2;
            // }
        }


        public TPoint[] ReadPoints(ushort startAddress, ushort numberOfPoints) {
            var points = new TPoint[numberOfPoints];
            for (ushort i = 0; i < numberOfPoints; i++) 
                points[i] = this[(ushort) (i + startAddress)];
            StorageOperationOccurred?.Invoke(this, new StorageEventArgs<TPoint>(PointOperation.Read, startAddress, points));
            return points;
        }

        public void WritePoints(ushort startAddress, TPoint[] points) {
            for (ushort i = 0; i < points.Length; i++) 
                this[(ushort) (i + startAddress)] = points[i];
            StorageOperationOccurred?.Invoke(this, new StorageEventArgs<TPoint>(PointOperation.Write, startAddress, points));
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

        public override string ToString() {
            return $"tag: {StartingAddress}, Points:{string.Join(" ", Points)}";
        }
    }

    public enum PointOperation {
        Read,
        Write
    }
}