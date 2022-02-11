using System.Collections.Generic;

namespace smartlink.JsonData;

public class Devices : List<ushort> { 
    public void Visit(IVisitor v) { v.VisitDevices(this); }

    public static void A_3000_MS(ElektronikonRequest answers, List<ushort> JSON) {
        var vData = answers.getData(0x3001, 8);
        JSON[0] = vData.UInt16(0);
    }

    public static void Q_3000_MS(ElektronikonRequest QUESTIONS) {
        QUESTIONS.Add(0x3001, 8);
    }

    public static void A_2000_MMT(ElektronikonRequest vQuestions, List<ushort> dEVICE) {
        byte data = vQuestions.getData(0x2001, 1).Byte(0);
        dEVICE.Add(data);
    }

    public static void Q_2000_MMT(ElektronikonRequest er) {
        er.Add(0x2001, 1);
    }
}