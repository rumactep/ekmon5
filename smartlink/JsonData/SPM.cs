using System;
using System.Collections.Generic;

namespace smartlink.JsonData {

    public class SPM : BaseData {
        public ushort MPL { get; set; }
        public int RTD_SI { get; set; }
        public AnswerData Data2 { get; set; } = AnswerData.Empty;
        public void setData(AnswerData data1, AnswerData data2) {
            setData(data1);
            Data2 = data2;
        }
        public string getValue() {
            var dBm1 = Data.Byte(3);
            string strdBm = (dBm1 < 10 ? "0" : "") + dBm1;
            var dBc1 = Data.Byte(2);
            string strdBc = (dBc1 < 10 ? "0" : "") + dBc1;

            var date = new DateTime(Data2.UInt32() * 1000);
            var day = date.Day;
            string strday = (day < 10 ? "0" : "") + day;
            var month = date.Month + 1;
            string strmonth = (month < 10 ? "0" : "") + month;
            var stryear = date.Year;
            var hours = date.Hour - 2;
            string strhours = (hours < 10 ? "0" : "") + hours;
            var minutes = date.Minute;
            string strminutes = (minutes < 10 ? "0" : "") + minutes;
            var timestamp = strday + "/" + strmonth + "/" + stryear + " - " + strhours + ":" + strminutes;

            return timestamp + " " + strdBc + " dBcsv / " + strdBm + " dBmsv";
        }
    }

    public class SPMView : IView {
        private SPM _item;
        private Language _language;

        public SPMView(SPM item, Language language) {
            _item = item;
            _language = language;
        }

        public string GetString() {
            string strMpl = _language.GetString("MPL", _item.MPL);
            string strvalue = _item.getValue();

            return $"MPL:{_item.MPL}, RTD_SI:{_item.RTD_SI} {strMpl} {strvalue}\n";
        }
    }

    public class SPMs : List<SPM>, IViewCreator {
        public void Visit(IVisitor v) { v.VisitSpms(this, this); }
        public IView CreateView(object item, Language language) {
            return new SPMView((SPM)item, language);
        }

        public static void A_3000_SPM(ElektronikonRequest answers, List<SPM> JSON) {
            for (var i = 0; i < JSON.Count; i++) {
                var data1 = answers.getData(0x3015, JSON[i].RTD_SI);
                var data2 = answers.getData(0x3015, JSON[i].RTD_SI + 1);
                JSON[i].setData(data1, data2);
            }
        }

        public static void Q_3000_SPM(ElektronikonRequest QUESTIONS, List<SPM> JSON) {
            for (var i = 0; i < JSON.Count; i++) {
                QUESTIONS.Add(0x3015, JSON[i].RTD_SI);
                QUESTIONS.Add(0x3015, JSON[i].RTD_SI + 1);
            }
        }

        public static void A_2000_SPM(ElektronikonRequest vQuestions, List<SPM> sPM2) {
            for (var i = 0x2560; i < 0x2570; i++) {
                var data1 = vQuestions.getData(i, 1);
                if (data1.IsEmpty)
                    continue;

                var vStatus = data1.Byte(0) != 0;
                if (vStatus) {
                    var vSPM = new SPM {
                        MPL = vQuestions.getData(i, 1).UInt16(1),
                        RTD_SI = 2 * (i - 0x2560) + 1,
                    };
                    sPM2.Add(vSPM);
                }
            }
        }

        public static void Q_2000_SPM(ElektronikonRequest er) {
            for (var i = 0x2560; i < 0x2570; i++)
                er.Add(i, 1);
        }
    }
}