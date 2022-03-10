using System;
using System.Collections.Generic;

namespace smartlink.JsonData {

    public class Counter : BaseData {
        public ushort MPL { get; set; }
        public byte COUNTERUNIT { get; set; }
        public int RTD_SI { get; set; }

        public uint getValue() {
            return Data.UInt32();
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
        Dictionary<int, uint> _percentes;

        public CounterView(Counter item, Language language, Dictionary<int, uint> percentes) {
            _item = item;
            _language = language;
            _percentes = percentes;
        }


        public string GetString() {
            string strMpl = _language.GetString("MPL", _item.MPL);

            string strvalue = _percentes.TryGetValue(_item.MPL, out uint value) ?
                value.ToString() + " %" :
                format_CO_value(_item.getValue(), _item.COUNTERUNIT);

            return
                $"RTD_SI:{_item.RTD_SI} {strMpl} {strvalue}, INPUTTYPE:{_item.COUNTERUNIT}, value:{_item.getValue()}\n";
        }
        string format_CO_value(uint CO_value, byte CO_unit) {
            uint value = CO_value;
            string unit;
            switch (CO_unit) {
                case 0:
                    value = value / 3600;
                    unit = value == 1 ?
                        " " + _language.GetString("HOURS", 2) :
                        " " + _language.GetString("HOURS", 1);
                    break;
                case 1:
                    unit = "";
                    break;
                case 2:
                    unit = value == 0 ? " m3" : "000 m3";
                    break;
                case 3:
                    unit = " %";
                    break;
                case 4:
                    unit = " kW";
                    return (value / 10).ToString("N1") + unit;
                case 5:
                    unit = "";
                    break;
                case 6:
                    unit = "kWh";
                    break;
                default:
                    unit = "";
                    break;
            }

            string str = value.ToString() + unit;
            return str;
        }
    }


    public class Counters : List<Counter>, IViewCreator {
        public void Visit(IVisitor visitor) {
            _percentes = CalculatePercentes(this);
            visitor.VisitCounters(this, this);
        }

        Dictionary<int, uint> _percentes = new();

        Dictionary<int, uint> CalculatePercentes(List<Counter> counters) {
            //var rawArray = new Array();
            uint sum = 0;

            Dictionary<int, uint> hours = new();
            foreach (Counter counter in counters) {
                if (counter.MPL >= 2706 && counter.MPL <= 2710) {
                    uint v = counter.getValue();
                    var uintv1000 = v / 1000;
                    hours.Add(counter.MPL, v);
                    sum += v;
                }
            }
            Dictionary<int, uint> percentes = new();
            foreach (KeyValuePair<int, uint> p in hours) {
                double percent = p.Value * 100.0 / sum;
                percentes[p.Key] = (uint)percent;
            }

            // сумма процентов может быть меньше 100% потому что отбрасываются дробные части
            // например 9.9% + 9.9% + 9.9% + 9.9% + 50.6% = 9 + 9 + 9 + 9 + 50 = 96%
            // TODO: сделать пересчет процентов с учетом остатков

            return percentes;
        }

        public IView CreateView(object item, Language language) {
            return new CounterView((Counter)item, language, _percentes);
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
}