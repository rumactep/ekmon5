using System.Collections.Generic;

namespace smartlink.JsonData;

public class SpecialProtection : BaseData {
    public ushort MPL { get; set; }
    public int RTD_SI { get; set; }
    public override string ToString() {
        return
            $"MPL:{MPL}, RTD_SI:{RTD_SI}\n";
    }
}
public class SpecialProtections : List<SpecialProtection> { 
    public void Visit(IVisitor v) { v.VisitSpecialProtections(this); }

    public static void A_3000_SPR(ElektronikonRequest answers, List<SpecialProtection> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x300E, JSON[i].RTD_SI));
    }

    public static void Q_3000_SPR(ElektronikonRequest QUESTIONS, List<SpecialProtection> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x300E, JSON[i].RTD_SI);
    }

    public static void A_2000_SPR(ElektronikonRequest vQuestions, List<SpecialProtection> sPECIALPROTECTIONS) {
        for (var i = 0x2300; i < 0x247F; i++) {
            var data1 = vQuestions.getData(i, 1);
            if (data1.IsEmpty)
                continue;
            var vStatus = data1.ToByte(0) != 0;
            if (vStatus) {
                var vSpecialProtection = new SpecialProtection {
                    MPL = vQuestions.getData(i, 1).UInt16(1),
                    RTD_SI = i - 0x2300 + 1
                };
                sPECIALPROTECTIONS.Add(vSpecialProtection);
            }
        }
    }

    public static void Q_2000_SPR(ElektronikonRequest er) {
        for (var i = 0x2300; i < 0x247F; i++)
            er.Add(i, 1);
    }
}
