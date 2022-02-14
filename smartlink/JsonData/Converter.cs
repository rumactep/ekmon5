using System;
using System.Collections.Generic;

namespace smartlink.JsonData;

public class Converter : BaseData {
    public byte CONVERTERTYPE { get; set; }
    public byte CONVERTERDEVICETYPE { get; set; }
    public int RTD_SI { get; set; }
    public AnswerData Data2 { get; set; } = AnswerData.Empty;

    public void setData(AnswerData data1, AnswerData data2) {
        setData(data1);
        Data2 = data2;
    }

    public ushort getValue() {
        return Data.UInt16(1);
    }

    public ushort getFlow() {
        return Data2.UInt16(0);
    }

    public override string ToString() {
        return
            $"MPL:-, RTD_SI:{RTD_SI}, TYPE:{CONVERTERTYPE}, DEVICETYPE:{CONVERTERDEVICETYPE}, Value:{getValue()}, Flow:{getFlow()}%\n";
    }
}

public class Converters : List<Converter> { 
    public void Visit(IVisitor visitor) { visitor.VisitConverters(this); }

    public static void A_3000_CNV(ElektronikonRequest answers, List<Converter> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x3020 + JSON[i].RTD_SI, 1), answers.getData(0x3021, 5));
    }

    public static void Q_3000_CNV(ElektronikonRequest QUESTIONS, List<Converter> JSON) {
        QUESTIONS.Add(0x3021, 5);
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3020 + JSON[i].RTD_SI, 1);
    }

    public static void A_2000_CNV(ElektronikonRequest vQuestions, List<Converter> cONVERTERS) {
        for (var i = 0x2681; i < 0x2689; i++) {
            var data1 = vQuestions.getData(i, 1);
            if (data1.IsEmpty)
                continue;
            var byte0 = data1.ToByte(0);
            if (byte0 != 0) {
                var vConverter = new Converter {
                    CONVERTERTYPE = data1.ToByte(1),
                    CONVERTERDEVICETYPE = vQuestions.getData(i, 7).ToByte(0),
                    RTD_SI = i - 0x2681 + 1
                };
                cONVERTERS.Add(vConverter);
            }
        }
    }

    public static void Q_2000_CNV(ElektronikonRequest er) {
        for (var i = 0x2681; i < 0x2689; i++) {
            er.Add(i, 1);
            er.Add(i, 7);
        }
    }
}
