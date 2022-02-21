using System.Collections.Generic;

namespace smartlink.JsonData;

public class Counter : BaseData {
    public ushort MPL { get; set; }
    public byte COUNTERUNIT { get; set; }
    public int RTD_SI { get; set; }

    public int getValue() {
        return Data.Int32();
    }


    /*
    var rawArray = new Array();
    var sum = 0;

    for (var i in json) {
        if (json[i].MPL >= 2706 && json[i].MPL <= 2710) {
            rawArray[json[i].MPL] = json[i].getValue();
    sum += rawArray[json[i].MPL];
        }
    }

    if (rawArray.length != 0 && sum != 0) {
    var quotients = new Array();
    var rests = new Array();

        for (var i in rawArray) {
        quotients[i] = Math.floor(rawArray[i] * 100 / sum);
        rests[i] = ((rawArray[i] * 100) % sum);
    }

    while (true) {
        var quotient_sum = 0;
        var max = 0;
        var max_index = 0;
            for (var i in quotients) {
            quotient_sum += quotients[i];

            if (rests[i] > max) {
                max = rests[i];
                max_index = i;
            }
        }

        if (quotient_sum == 100) {
            return quotients;
        }
        else {
            quotients[max_index]++;
            rests[max_index] = 0;
        }
    }
}
else if (rawArray.length == 0) {
    return new Array();
}
else {
    return rawArray;
} //*/

}

public class CounterView : IView {
    private Counter _item;
    private Language _language;

    public CounterView(Counter item, Language language) {
        _item = item;
        _language = language;
    }

    public string GetString() {
        return
            $"MPL:{_item.MPL}, RTD_SI:{_item.RTD_SI}, INPUTTYPE:{_item.COUNTERUNIT}, value:{_item.getValue()}\n";
    }
}


public class Counters : List<Counter>, IViewCreator { 
    public void Visit(IVisitor visitor) { visitor.VisitCounters(this, this); }
    public IView CreateView(object item, Language language) {
        return new CounterView((Counter)item, language);
    }
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
