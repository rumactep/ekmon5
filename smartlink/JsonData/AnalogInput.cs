using System;
using System.Collections.Generic;

namespace smartlink.JsonData {

    public class AnalogInput : BaseData {
        // absolute values with atmosphere not implemented
        public ushort MPL { get; set; }
        public int RTD_SI { get; set; }
        public byte INPUTTYPE { get; set; }
        public byte DISPLAYPRECISION { get; set; }
        //public byte PRESSUREMEASUREMENT { get; set; }
        //public ushort absATMpres { get; set; }

        public short getValue() {
            return Data.Int16(1);
        }

        public ushort getStatus() {
            return Data.UInt16(0);
        }
    }

    public interface IView {
        string GetString();
    }

    public interface IViewCreator {
        IView CreateView(object item, Language language);
    }

    public class AnalogInputView : IView {
        private readonly AnalogInput _item;
        private readonly Language _language;

        public AnalogInputView(AnalogInput item, Language language) {
            _item = item;
            _language = language;
        }
        public string GetString() {
            string strMpl = _language.GetString("MPL", _item.MPL);
            string strValue = FormatAiValue(_item.getValue(), _item.INPUTTYPE, _item.DISPLAYPRECISION, _language);
            return
                $"MPL:{_item.MPL} {strMpl}, RTD_SI:{_item.RTD_SI}, value:{strValue}, INPUTTYPE:{_item.INPUTTYPE}, DISPLAYPRECISION:{_item.DISPLAYPRECISION}, getStatus:{_item.getStatus()}\n";
        }

        public static string FormatAiValue(short AI_value, byte iNPUTTYPE, byte dISPLAYPRECISION, Language language) {
            // absolute values with atmosphere not implemented

            if (AI_value == short.MaxValue || AI_value == short.MinValue)
                return language.GetString("SENSORERROR", 1);
            // perhaps, need to increment (or decrement) value for negative numbers ?
            float value = AI_value;
            string unit = "";
            switch (iNPUTTYPE) {
                case 0: value = value / 1000; unit = "bar"; break; // mbar
                case 1: value = value / 10; unit = "°C"; break; //0.1°C
                case 2: value = value / 100; unit = "µm"; break;
                case 3: value = value / 10; unit = "mm"; break;
                case 4: unit = "µS/cm"; break;
                case 5: unit = "dB"; break;
                case 6: unit = "l/s"; break;
                case 7: value = value / 10; unit = "A"; break;//0.1 A        
                case 8: unit = "rpm"; break;
                case 9: value = value / 100; unit = "cBar"; break;
                case 10: unit = "%"; break;
                case 11: value = value * 10; unit = "rpm"; break;
                case 12: unit = "ppm"; break;
                case 13: value = value / 100; unit = "ppm"; break;
                case 14: value = value / 100; unit = "% VV"; break;
                case 15: unit = "%"; break;
                case 16: value = value / 10; unit = "°C"; break; // 0.1°C
                case 17: value = value / 1000; unit = ""; break;
                case 18: unit = ""; break;
                case 19: value = value / 10; unit = "kW"; break;
                case 20: unit = "kW"; break;
                case 21: unit = "J"; break;
                case 22: unit = "mі"; break;
                case 23: unit = "J/l"; break;
                case 24: unit = ""; break;
                case 25: unit = "Pa/s"; break;
                case 26: unit = ""; break;
                case 27: value = value / 100; unit = "%"; break;
                case 29: value = value * 10; unit = "Micron"; break;
            }

            switch (unit) {
                case "psi": value = 14.5038f * value; break;
                case "MPa": value = 0.1f * value; break;
                case "kg/cm\u00b2": value = 1.019716f * value; break;
                case "\u00b0F": value = (9 / 5) * value + 32; break; // grad Fahheit?
                case "K": value = value + 273.15f; break;
                case "mmHg": value = value * 750.061683f; break;
            }

            return value.ToString("N" + $"{dISPLAYPRECISION}") + " " + unit; //*/
        }
    }

    public class AnalogInputs : List<AnalogInput>, IViewCreator {
        public void Visit(IVisitor visitor) {
            visitor.VisitAnalogInputs(this, this);
        }
        public IView CreateView(object item, Language language) {
            return new AnalogInputView((AnalogInput)item, language);
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
            for (var i = 0x2010; i < 0x2090; i++) {
                AnswerData data1 = questions.getData(i, 1);
                if (data1.IsEmpty)
                    continue;
                var byte0 = data1.ToByte(0);
                if (byte0 != 0) {
                    AnswerData data4 = questions.getData(i, 4);
                    //AnswerData data6 = questions.getData(i, 6);
                    //byte byte62 = 0;
                    //if (data6 != AnswerData.Empty)
                    //    byte62 = data6.Byte(2);
                    var vAnalogInput = new AnalogInput {
                        MPL = data1.UInt16(1),
                        INPUTTYPE = data1.ToByte(1),
                        DISPLAYPRECISION = data4.ToByte(3),
                        //    PRESSUREMEASUREMENT = byte62,
                        RTD_SI = i - 0x2010 + 1
                    };
                    //                AnswerData data260B = questions.getData(0x260B, 3);
                    //                if ((vAnalogInput.INPUTTYPE == 0
                    //                     || vAnalogInput.INPUTTYPE == 35
                    //                     || vAnalogInput.INPUTTYPE == 9)
                    //                    && data260B != AnswerData.Empty) {                    
                    //                        //vAnalogInput.absATMpres = data260B.UInt16(0);
                    //                }

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
}