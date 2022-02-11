using System;
using System.Collections.Generic;

namespace smartlink;

public class AnalogInputs : List<AnalogInput> {
    public static void Q_2000_AI(ElektronikonRequest er) {
        for (var i = 0x2010; i < 0x2090; i++) {
            er.Add(i, 1);
            er.Add(i, 4);
            //}
            //    er.Add(i, 6);
        }
        //er.Add(0x260B,3);
    }

    public static void A_2000_AI(ElektronikonRequest questions, List<AnalogInput> aNALOGINPUTS) {
        bool SubIndexPresent = false;
        try {
            AnswerData data260b = questions.getData(0x260B, 3);
            if (data260b != AnswerData.Empty)
                //if (data.UInt16(0) > -1) //this will result in an error if the subindex is NOT present
                SubIndexPresent = true;
        }
        catch (Exception) {
            // SubIndexPresent = false;
        }

        for (var i = 0x2010; i < 0x2090; i++) {
            AnswerData data1 = questions.getData(i, 1);
            if (data1.IsEmpty)
                continue;
            var byte0 = data1.ToByte(0);
            if (byte0 != 0) {
                var data4 = questions.getData(i, 4);
                var data6 = questions.getData(i, 6);
                byte byte62 = 0;
                if (data6 != AnswerData.Empty)
                    byte62 = data6.Byte(2);
                var vAnalogInput = new AnalogInput {
                    MPL = data1.ToUInt16(1),
                    INPUTTYPE = data1.ToByte(1),
                    DISPLAYPRECISION = data4.ToByte(3),
                    PRESSUREMEASUREMENT = byte62,
                    RTD_SI = i - 0x2010 + 1,
                };
                if (((vAnalogInput.INPUTTYPE == 0)
                    || (vAnalogInput.INPUTTYPE == 35)
                    || (vAnalogInput.INPUTTYPE == 9))
                        && SubIndexPresent) {
                    var data260b = questions.getData(0x260B, 3);
                    if (data260b != AnswerData.Empty)
                        vAnalogInput.absATMpres = data260b.UInt16(0);
                }
                aNALOGINPUTS.Add(vAnalogInput);
            }
            else {
                // TODO: invalidate data
            }
        }
    }

    public static void Q_3000_AI(ElektronikonRequest QUESTIONS, List<AnalogInput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3002, JSON[i].RTD_SI);
    }

    public static void A_3000_AI(ElektronikonRequest answers, List<AnalogInput> JSON) {
        for (var i = 0; i < JSON.Count; i++) {
            AnswerData vdata = answers.getData(0x3002, JSON[i].RTD_SI);
            JSON[i].setData(vdata);
        }
    }
}

public interface IPartWriter {
    void StartPart(string partName);
    void Write(string str);
    void WriteLine(string str);
    void StopPart();
    string Text { get; }
}

public class StringWriter : IPartWriter {
    public string Text { get; private set; } = string.Empty;

    public void StartPart(string partName) {
        Text += partName;
    }

    public void StopPart() {
        Text += Environment.NewLine;
    }

    public void Write(string str) {
        Text += str;
    }

    public void WriteLine(string str) {
        Text += str;
        Text += Environment.NewLine;
    }
}

public class JSONS {
    public void BuildString(IPartWriter writer) {
        writer.StartPart(ANALOGINPUTS.GetType().ToString());
        writer.WriteLine(string.Empty);
        foreach (AnalogInput item in ANALOGINPUTS) { 
            writer.Write(item.ToString());
        }
        writer.StopPart();
    }
    public AnalogInputs ANALOGINPUTS { get; set; } = new();
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

