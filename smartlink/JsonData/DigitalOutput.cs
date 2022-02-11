using System.Collections.Generic;

namespace smartlink.JsonData;

public class DigitalOutput : BaseData {
    public ushort MPL { get; set; }
    public int RTD_SI { get; set; }
}
public class DigitalOutputs : List<DigitalOutput> { 
    public void Visit(IVisitor v) { v.VisitDigitalOutputs(this); }

    public static void A_3000_DO(ElektronikonRequest answers, List<DigitalOutput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x3005, JSON[i].RTD_SI));
    }

    public static void Q_3000_DO(ElektronikonRequest QUESTIONS, List<DigitalOutput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3005, JSON[i].RTD_SI);
    }

    public static void A_2000_DO(ElektronikonRequest vQuestions, List<DigitalOutput> dIGITALOUTPUTS) {
        for (var i = 0x2100; i < 0x2150; i++) {
            var data1 = vQuestions.getData(i, 1);
            if (data1.IsEmpty)
                continue;
            var vStatus = data1.ToByte(0) != 0;
            if (vStatus) {
                var vDigitalOutput = new DigitalOutput {
                    MPL = vQuestions.getData(i, 1).UInt16(1),
                    RTD_SI = i - 0x2100 + 1
                };
                dIGITALOUTPUTS.Add(vDigitalOutput);
            }
        }
    }

    public static void Q_2000_DO(ElektronikonRequest er) {
        for (var i = 0x2100; i < 0x2150; i++)
            er.Add(i, 1);
    }
}
