using System.Collections.Generic;

namespace smartlink.JsonData;

public class Counter : BaseData {
    public ushort MPL { get; set; }
    public byte COUNTERUNIT { get; set; }
    public int RTD_SI { get; set; }
    public override string ToString() {
        return
            $"MPL:{MPL}, RTD_SI:{RTD_SI}, INPUTTYPE:{COUNTERUNIT}\n";
    }

}

public class Counters : List<Counter> { 
    public void Visit(IVisitor visitor) { visitor.VisitCounters(this); }

    public static void Q_2000_CNT(ElektronikonRequest er) {
        for (var i = 1; i < 256; i++)
            er.Add(0x2607, i);
    }

    public static void A_2000_CNT(ElektronikonRequest vQuestions, List<Counter> cOUNTERS) {
        for (var i = 1; i < 256; i++) {
            var data1 = vQuestions.getData(0x2607, i);
            if (data1.IsEmpty)
                continue;
            var byte0 = data1.ToByte(0);
            if (byte0 != 0) {
                var vCounter = new Counter {
                    MPL = data1.UInt16(1),
                    COUNTERUNIT = data1.ToByte(1),
                    RTD_SI = i
                };
                cOUNTERS.Add(vCounter);
            }
        }
    }

    public static void Q_3000_CNT(ElektronikonRequest QUESTIONS, List<Counter> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3007, JSON[i].RTD_SI);
    }

    public static void A_3000_CNT(ElektronikonRequest answers, List<Counter> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x3007, JSON[i].RTD_SI));
    }
}
