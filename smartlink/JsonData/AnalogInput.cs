using System;
using System.Collections.Generic;

namespace smartlink.JsonData;

public class AnalogInput : BaseData {
    public ushort MPL { get; set; }
    public int RTD_SI { get; set; }
    public byte INPUTTYPE { get; set; }
    public byte DISPLAYPRECISION { get; set; }
    public byte PRESSUREMEASUREMENT { get; set; }
    public ushort absATMpres { get; set; }

    public int getValue() {
        return Data.Int16(1);
    }

    public int getStatus() {
        return Data.UInt16(0);
    }

//    public override string ToString() {
//        return            $"MMPL:{MPL}, RTD_SI:{RTD_SI}, INPUTTYPE:{INPUTTYPE}, DISPLAYPRECISION:{DISPLAYPRECISION}, PRESSUREMEASUREMENT:{PRESSUREMEASUREMENT}, absATMpres:{absATMpres}, getValue:{getValue()}, getStatus:{getStatus()}\n";
//    }


    //case 0:value=value/1000;unit=$('#P option:selected').attr('value');break;//mbar   // ---> loadlanguage.js
    //case 1:value=value/10;unit=$('#T option:selected').attr('value');break;//0.1°C   // ---> loadlanguage.js 

}

public interface IView {
    string GetString();
}

public interface IViewCreator {
    IView CreateView(object item);
}

// Зачем здесь это разделение на данные и View - непонятно- возможно это какая-то попытка посмотреть что из этого выйдет
public class AnalogInputView : IView {
    private readonly AnalogInput _item;
    public AnalogInputView(AnalogInput item) {
        _item = item;
    }
    public string GetString() {
        return
            $"+MPL:{_item.MPL}, RTD_SI:{_item.RTD_SI}, INPUTTYPE:{_item.INPUTTYPE}, DISPLAYPRECISION:{_item.DISPLAYPRECISION}, PRESSUREMEASUREMENT:{_item.PRESSUREMEASUREMENT}, absATMpres:{_item.absATMpres}, getValue:{_item.getValue()}, getStatus:{_item.getStatus()}\n";
    }
}

public class AnalogInputs : List<AnalogInput>, IViewCreator {
    public void Visit(IVisitor visitor) {
        visitor.VisitAnalogInputs(this, this);
    }
    public IView CreateView(object item) {
        return new AnalogInputView((AnalogInput)item);
    }
    public static void Q_2000_AI(ElektronikonRequest er) {
        for (var i = 0x2010; i < 0x2090; i++) {
            er.Add(i, 1);
            er.Add(i, 4);
            //}
            //    er.Add(i, 6);
        }
        //er.Add(0x260B,3);
    }

    public static void A_2000_AI(ElektronikonRequest questions, List<AnalogInput> aNALOGINPUTS) {
        var subIndexPresent = false;
        try {
            AnswerData data260B = questions.getData(0x260B, 3);
            if (data260B != AnswerData.Empty)
                //if (data.UInt16(0) > -1) //this will result in an error if the subindex is NOT present
                subIndexPresent = true;
        }
        catch (Exception) {
            // SubIndexPresent = false;
        }

        for (var i = 0x2010; i < 0x2090; i++) {
            AnswerData data1 = questions.getData(i, 1);
            if (data1.IsEmpty)
                continue;
            var byte0 = data1.ToByte(0);
            if (byte0 != 0) {
                AnswerData data4 = questions.getData(i, 4);
                AnswerData data6 = questions.getData(i, 6);
                byte byte62 = 0;
                if (data6 != AnswerData.Empty)
                    byte62 = data6.Byte(2);
                var vAnalogInput = new AnalogInput {
                    MPL = data1.UInt16(1),
                    INPUTTYPE = data1.ToByte(1),
                    DISPLAYPRECISION = data4.ToByte(3),
                    PRESSUREMEASUREMENT = byte62,
                    RTD_SI = i - 0x2010 + 1
                };
                if ((vAnalogInput.INPUTTYPE == 0
                     || vAnalogInput.INPUTTYPE == 35
                     || vAnalogInput.INPUTTYPE == 9)
                    && subIndexPresent) {
                    AnswerData data260B = questions.getData(0x260B, 3);
                    if (data260B != AnswerData.Empty)
                        vAnalogInput.absATMpres = data260B.UInt16(0);
                }

                aNALOGINPUTS.Add(vAnalogInput);
            }
        }
    }

    public static void Q_3000_AI(ElektronikonRequest QUESTIONS, List<AnalogInput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3002, JSON[i].RTD_SI);
    }

    public static void A_3000_AI(ElektronikonRequest answers, List<AnalogInput> JSON) {
        for (var i = 0; i < JSON.Count; i++) {
            AnswerData vdata = answers.getData(0x3002, JSON[i].RTD_SI);
            JSON[i].setData(vdata);
        }
    }
}