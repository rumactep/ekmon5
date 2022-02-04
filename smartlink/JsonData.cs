using System;
using System.Collections.Generic;

namespace smartlink;

public class JSON {
    public JSON() {
    }

    public List<AnalogInput> ANALOGINPUTS { get; set; } = new List<AnalogInput>();
    public List<DigitalInput> DIGITALINPUTS { get; set; } = new List<DigitalInput> { };
    public List<Counter> COUNTERS { get; set; } = new List<Counter> { };
    public List<Converter> CONVERTERS { get; set; } = new List<Converter> { };
    public List<DigitalOutput> DIGITALOUTPUTS { get; set; } = new List<DigitalOutput> { };
    public List<object> CALCULATEDANALOGINPUTS { get; set; } = new List<object> { };
    public List<object> SPECIALPROTECTIONS { get; set; } = new List<object> { };
    public List<object> ANALOGOUTPUTS { get; set; } = new List<object> { };
    public List<object> SPM2 { get; set; } = new List<object> { };
    public List<object> ES { get; set; } = new List<object> { };
    public List<object> SERVICEPLAN { get; set; } = new List<object> { };
    public List<object> DEVICE { get; set; } = new List<object> { };
}

public class DigitalInput {
    public ushort MPL { get; set; }
    public int RTD_SI { get; set; }
}
public class AnalogInput {
    public AnalogInput() {
        Data = AnswerData.Empty;
    }
    public AnswerData Data { get; set; }
    public ushort MPL { get; set; }
    public int RTD_SI { get; set; }
    public byte INPUTTYPE { get; set; }
    public byte DISPLAYPRECISION { get; set; }

    public void setData(AnswerData answerData) {
        Data = answerData;
    }
}
public class Counter {
    public ushort MPL { get; set; }
    public byte COUNTERUNIT { get; set; }
    public int RTD_SI { get; set; }
}

public class Converter {
    public byte CONVERTERTYPE { get; set; }
    public byte CONVERTERDEVICETYPE { get; set; }
    public int RTD_SI { get; set; }
}

public class DigitalOutput {
    public ushort MPL { get; set; }
    public int RTD_SI { get; set; }
}
