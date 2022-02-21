using System;
using System.Collections.Generic;

namespace smartlink.JsonData;

public class ServicePlan : BaseData {
    public uint STATICVALUE { get; set; }
    public int RTD_SI { get; set; }
    public int LEVEL { get; set; }
    public bool Type { get; set; }
    public bool Next { get; set; }
    public void setData(AnswerData data1, AnswerData data2) {
        setData(data1);
        Next = ((data2.UInt32() >> (LEVEL - 1)) & 1) == 1;
    }

    public uint getValue() {
        return Data.UInt32();
    }
}

public class ServicePlanView : IView {
    private ServicePlan _item;
    private Language _language;

    public ServicePlanView(ServicePlan item, Language language) {
        _item = item;
        _language = language;
    }

    public string GetString() {
        return
            $"MPL:-, RTD_SI:{_item.RTD_SI}, STATICVALUE:{_item.STATICVALUE}, LEVEL:{_item.LEVEL}, Type:{_item.Type}, next:{_item.Next}, value:{_item.getValue() / 3600}\n";
    }
}

public class ServicePlans : List<ServicePlan>,  IViewCreator { 
    public void Visit(IVisitor v) { v.VisitServicePlans(this, this); }
    public IView CreateView(object item, Language language) {
        return new ServicePlanView((ServicePlan)item, language);
    }
    public static void A_3000_SPL(ElektronikonRequest answers, List<ServicePlan> JSON) {
        for (var i = 0; i < JSON.Count; i++) {
            var data1 = answers.getData(0x3009, JSON[i].RTD_SI);
            var data2 = answers.getData(0x3009, 1);
            JSON[i].setData(data1, data2);
        }
    }

    public static void Q_3000_SPL(ElektronikonRequest QUESTIONS, List<ServicePlan> JSON) {
        QUESTIONS.Add(0x3009, 1);
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3009, JSON[i].RTD_SI);
    }

    public static void A_2000_SPL(ElektronikonRequest QUESTIONS, List<ServicePlan> sERVICEPLAN) {
        for (var i = 1; i < 21; i++) {
            var datai = QUESTIONS.getData(0x2602, i);
            if (datai.IsEmpty)
                continue;
            var vSTATICVALUE = datai.UInt32();
            if (vSTATICVALUE != 0) {
                var vServicePlan = new ServicePlan {
                    STATICVALUE = vSTATICVALUE,
                    RTD_SI = ElektronikonRequest.GetServicePlanRTD_SI(i),
                    LEVEL = (int)Math.Ceiling(i / 2.0),
                    Type = (i % 2) == 1,
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
