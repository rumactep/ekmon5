using System;
using System.Collections.Generic;

namespace smartlink.JsonData;

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
    public override string ToString() {
        return
            $"MPL:-, RTD_SI:{RTD_SI}, STATICVALUE:{STATICVALUE}, LEVEL:{LEVEL}, Type:{Type}\n";
    }

}

public class ServicePlans : List<ServicePlan> { 
    public void Visit(IVisitor v) { v.VisitServicePlans(this); }

    public static void A_3000_SPL(ElektronikonRequest answers, List<ServicePlan> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x3009, JSON[i].RTD_SI), answers.getData(0x3009, 1));
    }

    public static void Q_3000_SPL(ElektronikonRequest QUESTIONS, List<ServicePlan> JSON) {
        QUESTIONS.Add(0x3009, 1);
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3009, JSON[i].RTD_SI);
    }

    public static void A_2000_SPL(ElektronikonRequest QUESTIONS, List<ServicePlan> sERVICEPLAN) {
        for (var i = 1; i < 21; i++) {
            var datai = QUESTIONS.getData(0x2602, i);
            var vSTATICVALUE = datai.UInt32();
            if (vSTATICVALUE != 0) {
                var vServicePlan = new ServicePlan {
                    STATICVALUE = vSTATICVALUE,
                    RTD_SI = ElektronikonRequest.GetServicePlanRTD_SI(i),
                    LEVEL = Math.Ceiling(i / 2.0),
                    Type = (i % 2),
                };
                sERVICEPLAN.Add(vServicePlan);
            }
        }
    }

    public static void Q_2000_SPL(ElektronikonRequest er) {
        for (var i = 1; i < 21; i++)
            er.Add(0x2602, i);
    }
}
