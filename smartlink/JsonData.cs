using System;
using System.Collections.Generic;

namespace smartlink;

public class JSON {
    public List<AnalogInput> ANALOGINPUTS { get; set; } = new();
    public List<DigitalInput> DIGITALINPUTS { get; set; } = new();
    public List<Counter> COUNTERS { get; set; } = new();
    public List<Converter> CONVERTERS { get; set; } = new();
    public List<DigitalOutput> DIGITALOUTPUTS { get; set; } = new();
    public List<CalculatedAnalogInput> CALCULATEDANALOGINPUTS { get; set; } = new(); 
    public List<SpecialProtection> SPECIALPROTECTIONS { get; set; } = new();
    public List<AnalogOutput> ANALOGOUTPUTS { get; set; } = new();
    public List<SPM1> SPM2 { get; set; } = new();
    // ES is not implemented yet
    public List<object> ES { get; set; } = new();
    public List<ServicePlan> SERVICEPLAN { get; set; } = new();
    public List<ushort> DEVICE { get; set; } = new();

}

public abstract class BaseData {
    public AnswerData Data { get; set; } = AnswerData.Empty;
    public void setData(AnswerData answerData) {
        Data = answerData;
    }
}

public class DigitalInput : BaseData {
    public ushort MPL { get; set; }
    public int RTD_SI { get; set; }
}
public class AnalogInput : BaseData {
    public ushort MPL { get; set; }
    public int RTD_SI { get; set; }
    public byte INPUTTYPE { get; set; }
    public byte DISPLAYPRECISION { get; set; }
    public int getValue() => Data.Int16(1); 
    public int getStatus() => Data.UInt16(1);
}

public class Counter : BaseData {
    public ushort MPL { get; set; }
    public byte COUNTERUNIT { get; set; }
    public int RTD_SI { get; set; }
}

public class Converter : BaseData {
    public byte CONVERTERTYPE { get; set; }
    public byte CONVERTERDEVICETYPE { get; set; }
    public int RTD_SI { get; set; }
    public AnswerData Data2 { get; set; } = AnswerData.Empty;
    public void setData(AnswerData data1, AnswerData data2) {
        setData(data1);
        Data2 = data2;
    }
}

public class DigitalOutput : BaseData {
    public ushort MPL { get; set; }
    public int RTD_SI { get; set; }
}

public class CalculatedAnalogInput : BaseData {
    public ushort MPL { get; set; }
    public byte INPUTTYPE { get; set; }
    public byte DISPLAYPRECISION { get; set; }
    public int RTD_SI { get; set; }
}

public class SpecialProtection : BaseData {
    public ushort MPL { get; set; }
    public int RTD_SI { get; set; }
}

public class AnalogOutput : BaseData {
    public ushort MPL { get; set; }
    public byte OUTPUTTYPE { get; set; }
    public byte DISPLAYPRECISION { get; set; }
    public int RTD_SI { get; set; }
}

public class SPM1 : BaseData {
    public ushort MPL { get; set; }
    public int RTD_SI { get; set; }
    public AnswerData Data2 { get; set; } = AnswerData.Empty;
    public void setData(AnswerData data1, AnswerData data2) {
        setData(data1);
        Data2 = data2;
    }
}

public class ServicePlan : BaseData {
    public uint STATICVALUE { get; set; }
    public int RTD_SI { get; set; }
    public double LEVEL { get; set; }
    public int Type { get; set; }
    public AnswerData Data2 { get; set; } = AnswerData.Empty;
    public void setData(AnswerData data1, AnswerData data2) {
        setData(data1);
        Data2 = data2;
    }
}
