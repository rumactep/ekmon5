using System.Collections.Generic;

namespace smartlink.JsonData;

public class DevicesView : IView {
    private ushort _item;
    private Language _language;

    public DevicesView(ushort item, Language language) {
        _item = item;
        _language = language;
    }

    public string GetString() {
        return _item.ToString();
    }
}

public class Devices : List<ushort>, IViewCreator { 
    public void Visit(IVisitor v) { v.VisitDevices(this, this); }
    public IView CreateView(object item, Language language) {
        return new DevicesView((ushort)item, language);
    }

    public static void A_3000_MS(ElektronikonRequest answers, List<ushort> JSON) {
        var vData = answers.getData(0x3001, 8);
        JSON[0] = vData.UInt16(0);
    }

    public static void Q_3000_MS(ElektronikonRequest QUESTIONS) {
        QUESTIONS.Add(0x3001, 8);
    }

    public static void A_2000_MMT(ElektronikonRequest vQuestions, List<ushort> dEVICE) {
        var data1 = vQuestions.getData(0x2001, 1);
        if (data1.IsEmpty)
            return;
        byte data = data1.Byte(0);
        dEVICE.Add(data);
    }

    public static void Q_2000_MMT(ElektronikonRequest er) {
        er.Add(0x2001, 1);
    }
}