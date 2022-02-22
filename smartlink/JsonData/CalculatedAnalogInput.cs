using System.Collections.Generic;

namespace smartlink.JsonData {

    public class CalculatedAnalogInput : BaseData {
        public ushort MPL { get; set; }
        public byte INPUTTYPE { get; set; }
        public byte DISPLAYPRECISION { get; set; }
        public int RTD_SI { get; set; }

        public short getValue() {
            return Data.Int16(1);
        }

        public int getStatus() {
            return Data.UInt16(0);
        }
    }

    public class CalculatedAnalogInputView : IView {
        private CalculatedAnalogInput _item;
        private Language _language;

        public CalculatedAnalogInputView(CalculatedAnalogInput item, Language language) {
            _item = item;
            _language = language;
        }

        public string GetString() {
            string strMpl = _language.GetString("MPL", _item.MPL);
            string strvalue = AnalogInputView.FormatAiValue(_item.getValue(), _item.INPUTTYPE, _item.DISPLAYPRECISION, _language);
            return $"MPL:{strMpl}, RTD_SI:{_item.RTD_SI}, value:{strvalue}, INPUTTYPE:{_item.INPUTTYPE}, DISPLAYPRECISION:{_item.DISPLAYPRECISION}, status:{_item.getStatus()}\n";
        }
    }

    public class CalculatedAnalogInputs : List<CalculatedAnalogInput>, IViewCreator {
        public void Visit(IVisitor visitor) { visitor.VisitCalculatedAnalogInputs(this, this); }
        public IView CreateView(object item, Language language) {
            return new CalculatedAnalogInputView((CalculatedAnalogInput)item, language);
        }
        public static void A_3000_CAI(ElektronikonRequest answers, List<CalculatedAnalogInput> JSON) {
            for (var i = 0; i < JSON.Count; i++)
                JSON[i].setData(answers.getData(0x3004, JSON[i].RTD_SI));
        }

        public static void Q_3000_CAI(ElektronikonRequest QUESTIONS, List<CalculatedAnalogInput> JSON) {
            for (var i = 0; i < JSON.Count; i++)
                QUESTIONS.Add(0x3004, JSON[i].RTD_SI);
        }

        public static void A_2000_CAI(ElektronikonRequest vQuestions, List<CalculatedAnalogInput> cALCULATEDANALOGINPUTS) {
            for (var i = 0x2090; i < 0x20b0; i++) {
                var vData = vQuestions.getData(i, 1);
                if (vData.IsEmpty)
                    continue;

                var byte0 = vData.ToByte(0);
                if (byte0 != 0) {
                    var vCalculatedAnalogInput = new CalculatedAnalogInput {
                        MPL = vData.UInt16(1),
                        INPUTTYPE = vData.ToByte(1),
                        DISPLAYPRECISION = vQuestions.getData(i, 3).ToByte(3),
                        RTD_SI = i - 0x2090 + 1
                    };
                    cALCULATEDANALOGINPUTS.Add(vCalculatedAnalogInput);
                }
            }
        }

        public static void Q_2000_CAI(ElektronikonRequest er) {
            for (var i = 0x2090; i < 0x20b0; i++) {
                er.Add(i, 1);
                er.Add(i, 3);
            }
        }
    }    
}