using System.Collections.Generic;

namespace smartlink.JsonData {

    public class SpecialProtection : BaseData {
        public ushort MPL { get; set; }
        public int RTD_SI { get; set; }

        public ushort getStatus() {
            return Data.UInt16(0);
        }
    }

    public class SpecialProtectionView : IView {
        private SpecialProtection _item;
        private Language _language;

        public SpecialProtectionView(SpecialProtection item, Language language) {
            _item = item;
            _language = language;
        }

        string get_status_icon1(ushort status, string defaultIcon) {
            var src = defaultIcon;

            if ((status & 0x0008) == 0x0008)
                src = "Shutdown";
            else if ((status & 0x0020) == 0x0020)
                src = "PermissiveStartFailure";
            else if (((status & 0x0002) == 0x0002) || ((status & 0x0004) == 0x0004))
                src = "Warning";
            else if ((status & 0x0010) == 0x0010)
                src = "Service";
            else if ((status & 0x0001) == 0x0001)
                src = "PreWarning";
            else if ((status & 0x2000) == 0x2000)
                src = "PrePermissiveStartFailure";
            else if ((status & 0x0040) == 0x0040)
                src = "SensorError";

            return src;
        }

        public string GetString() {
            string strMpl = _language.GetString("MPL", _item.MPL);
            // where is "Question"?
            string statusicon = get_status_icon1(_item.getStatus(), "Ok");
            return $"MPL:{_item.MPL}, RTD_SI:{_item.RTD_SI}, {strMpl} {statusicon}\n";
        }
    }

    public class SpecialProtections : List<SpecialProtection>, IViewCreator {
        public void Visit(IVisitor v) { v.VisitSpecialProtections(this, this); }
        public IView CreateView(object item, Language language) {
            return new SpecialProtectionView((SpecialProtection)item, language);
        }
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
}