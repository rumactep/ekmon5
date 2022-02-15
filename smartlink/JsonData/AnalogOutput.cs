using System.Collections.Generic;

namespace smartlink.JsonData;

public class AnalogOutput : BaseData {
    public ushort MPL { get; set; }
    public byte OUTPUTTYPE { get; set; }
    public byte DISPLAYPRECISION { get; set; }
    public int RTD_SI { get; set; }

    public int getValue() {
        return Data.Int16(1);
    }

    public int getStatus() {
        return Data.UInt16(0);
    }

    public override string ToString() {
        return
            $"MPL:{MPL}, RTD_SI:{RTD_SI}, OUTPUTTYPE:{OUTPUTTYPE}, DISPLAYPRECISION:{DISPLAYPRECISION}, value:P{getValue()}, status:{getStatus()}\n";
    }
} 
public class AnalogOutputs : List<AnalogOutput> { 
    public void Visit(IVisitor v) { v.VisitAnalogOutputs(this); }

    public static void A_3000_AO(ElektronikonRequest answers, List<AnalogOutput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x3006, JSON[i].RTD_SI));
    }

    public static void Q_3000_AO(ElektronikonRequest QUESTIONS, List<AnalogOutput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3006, JSON[i].RTD_SI);
    }

    public static void A_2000_AO(ElektronikonRequest vQuestions, List<AnalogOutput> aNALOGOUTPUTS) {
        for (var i = 0x2150; i < 0x2170; i++) {
            var data1 = vQuestions.getData(i, 1);
            if (data1.IsEmpty)
                continue;
            var vStatus = data1.Byte(0) != 0;
            if (vStatus) {
                var vAnalogOutput = new AnalogOutput {
                    MPL = vQuestions.getData(i, 1).UInt16(1),
                    OUTPUTTYPE = vQuestions.getData(i, 1).Byte(1),
                    DISPLAYPRECISION = vQuestions.getData(i, 3).Byte(3),
                    RTD_SI = i - 0x2150 + 1,
                };
                aNALOGOUTPUTS.Add(vAnalogOutput);

            }    
        }
    }

    public static void Q_2000_AO(ElektronikonRequest er) {
        for (var i = 0x2150; i < 0x2170; i++) {
            er.Add(i, 1);
            er.Add(i, 3);
        }
    }
}
