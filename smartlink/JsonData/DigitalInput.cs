using System.Collections.Generic;

namespace smartlink.JsonData {

    public class DigitalInput : BaseData {
        public ushort MPL { get; set; }
        public int RTD_SI { get; set; }

        public ushort getValue() {
            return Data.UInt16(1);
        }

        public ushort getStatus() {
            return Data.UInt16(0);
        }
    }

    public class DigitalInputView : IView {
        private DigitalInput _item;
        private Language _language;

        public DigitalInputView(DigitalInput item, Language language) {
            _item = item;
            _language = language;
        }

        public string GetString() {
            string strMpl = _language.GetString("MPL", _item.MPL);
            string strvalue = _language.GetString("MPL", _item.MPL, _item.getValue() + 1);

            return $"RTD_SI:{_item.RTD_SI}, {strMpl} {strvalue}, status:{_item.getStatus()}\n";
        }
    }

    public class DigitalInputs : List<DigitalInput>, IViewCreator {
        public void Visit(IVisitor visitor) { visitor.VisitDigitalInputs(this, this); }
        public IView CreateView(object item, Language language) {
            return new DigitalInputView((DigitalInput)item, language);
        }

        public static void A_3000_DI(ElektronikonRequest answers, List<DigitalInput> JSON) {
            for (var i = 0; i < JSON.Count; i++)
                JSON[i].setData(answers.getData(0x3003, JSON[i].RTD_SI));
        }

        public static void Q_3000_DI(ElektronikonRequest QUESTIONS, List<DigitalInput> JSON) {
            for (var i = 0; i < JSON.Count; i++)
                QUESTIONS.Add(0x3003, JSON[i].RTD_SI);
        }

        public static void A_2000_DI(ElektronikonRequest vQuestions, List<DigitalInput> dIGITALINPUTS) {
            for (var i = 0x20b0; i < 0x2100; i++) {
                AnswerData data1 = vQuestions.getData(i, 1);
                if (data1.IsEmpty)
                    continue;
                var byte0 = data1.ToByte(0);
                if (byte0 != 0) {
                    var vDigitalInput = new DigitalInput {
                        MPL = data1.UInt16(1),
                        RTD_SI = i - 0x20b0 + 1
                    };
                    dIGITALINPUTS.Add(vDigitalInput);
                }
            }
        }

        public static void Q_2000_DI(ElektronikonRequest er) {
            for (var i = 0x20b0; i < 0x2100; i++)
                er.Add(i, 1);
        }
    }
}