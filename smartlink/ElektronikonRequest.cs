using System;
using System.Collections.Generic;
using System.Text;

namespace smartlink;

public class ElektronikonRequest {

    //private readonly SortedDictionary<DataItem, string> _requests = new(new DataItemComparer());
    private readonly List<Question> _requests = new();
    public int Length => _requests.Count;

    public void Add(Question dataItem) {
        _requests.Add(dataItem);
    }

    public void Add(int index, int subindex) {
        _requests.Add(new Question(index, subindex));
    }

    public ElektronikonRequest SparseQuestions() {
        ElektronikonRequest newRequest = new ElektronikonRequest();
        foreach (Question dataItem in _requests) {
            if (!dataItem.Data.IsEmpty)
                newRequest.Add(dataItem);
        }
        return newRequest;
    }

    public static void ProcessData(ElektronikonRequest answers, JSONS json) {
        AnalogInputs.A_3000_AI(answers, json.ANALOGINPUTS);
        A_3000_DI(answers, json.DIGITALINPUTS);
        A_3000_CNT(answers, json.COUNTERS);
        A_3000_CNV(answers, json.CONVERTERS);
        A_3000_DO(answers, json.DIGITALOUTPUTS);
        A_3000_CAI(answers, json.CALCULATEDANALOGINPUTS);

        A_3000_SPR(answers, json.SPECIALPROTECTIONS);
        A_3000_AO(answers, json.ANALOGOUTPUTS);
        A_3000_SPM(answers, json.SPM2);
        A_3000_ES(answers, json.ES);
        A_3000_SPL(answers, json.SERVICEPLAN);
        A_3000_MS(answers, json.DEVICE);
    }



    private static void A_3000_DI(ElektronikonRequest answers, List<DigitalInput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x3003, JSON[i].RTD_SI));
    }

    private static void A_3000_CNT(ElektronikonRequest answers, List<Counter> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x3007, JSON[i].RTD_SI));
    }

    private static void A_3000_CNV(ElektronikonRequest answers, List<Converter> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x3020 + JSON[i].RTD_SI, 1), answers.getData(0x3021, 5));
    }

    private static void A_3000_DO(ElektronikonRequest answers, List<DigitalOutput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x3005, JSON[i].RTD_SI));
    }

    private static void A_3000_CAI(ElektronikonRequest answers, List<CalculatedAnalogInput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x3004, JSON[i].RTD_SI));
    }

    private static void A_3000_SPR(ElektronikonRequest answers, List<SpecialProtection> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x300E, JSON[i].RTD_SI));
    }

    private static void A_3000_AO(ElektronikonRequest answers, List<AnalogOutput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x3006, JSON[i].RTD_SI));
    }

    private static void A_3000_SPM(ElektronikonRequest answers, List<SPM1> JSON) {
        for (var i = 0; i < JSON.Count; i++) {
            var data1 = answers.getData(0x3015, JSON[i].RTD_SI);
            var data2 = answers.getData(0x3015, JSON[i].RTD_SI + 1);
            JSON[i].setData(data1, data2);
        }
    }

    private static void A_3000_ES(ElektronikonRequest QUESTIONS, List<object> JSON) {
        // not implemented
        /*
         try {
             var vData1 = QUESTIONS.getData(0x3113, 1);
             JSON.NRCOMPRESSORS = vData1.Byte(0);
             JSON.NRDRYERS = vData1.Byte(2);
             JSON.ACTIVE = (vData1.Byte(1) == 1);
             var data3 = QUESTIONS.getData(0x3113, 3);
             JSON.STATE = data3.UInt16(0);
             var data4 = QUESTIONS.getData(0x3113, 4);
             JSON.REGULATIONPRESSURE = data4.UInt32();
             var data5 = QUESTIONS.getData(0x3113, 5);
             JSON.CONTROLVSD = data5.Byte(2);

             for (var i = 0; i < JSON.NRCOMPRESSORS; i++) {
                 var vSlave = new Slave();
                 vSlave.MIN = QUESTIONS.getData(0x3113, 7 + 4 * i).UInt16(1);
                 vSlave.ACT = QUESTIONS.getData(0x3113, 8 + 4 * i).UInt16(0);
                 vSlave.MAX = QUESTIONS.getData(0x3113, 8 + 4 * i).UInt16(1);
                 vSlave.TYPE = QUESTIONS.getData(0x3113, 7 + 4 * i).Byte(1);
                 vSlave.MS = QUESTIONS.getData(0x3113, 7 + 4 * i).Byte(0);
                 vSlave.SUMMARY1 = QUESTIONS.getData(0x3113, 9 + 4 * i).UInt32();
                 vSlave.SUMMARY2 = QUESTIONS.getData(0x3113, 10 + 4 * i).UInt32();
                 JSON.COMPRESSORS[i] = vSlave;
             }
             for (var i = 0; i < JSON.NRDRYERS; i++) {
                 var vDryer = new Dryer();
                 vDryer.UPPERICON = QUESTIONS.getData(0x3113, 31 + 2 * i).Int16(1);
                 vDryer.LOWERICON = QUESTIONS.getData(0x3113, 31 + 2 * i).Int16(0);
                 vDryer.BARVALUE = QUESTIONS.getData(0x3113, 32 + 2 * i).Byte(0);
                 JSON.DRYERS[i] = vDryer;
             }
             if (JSON.NRCOMPRESSORS > 0) {
                 var vMasterBar = new MasterBar();
                 vMasterBar.LEVEL1 = QUESTIONS.getData(0x3114, 1).Int32();
                 vMasterBar.LEVEL2 = QUESTIONS.getData(0x3114, 2).Int32();
                 vMasterBar.ACT = QUESTIONS.getData(0x3114, 3).Int32();
                 vMasterBar.INRANGE = QUESTIONS.getData(0x3114, 4).Byte(1);
                 vMasterBar.PERCENTAGE = QUESTIONS.getData(0x3114, 4).Byte(0);
                 vMasterBar.METHODOFFILLING = QUESTIONS.getData(0x3114, 4).Byte(2);
                 vMasterBar.TYPE = QUESTIONS.getData(0x3114, 4).Byte(3); //0-->25/75 1-->0/100
                 JSON.COMPRESSORMASTERBAR = vMasterBar;
             }

             if (JSON.NRDRYERS > 0) {
                 var vDryerMasterBar = new DryerMasterBar();
                 vDryerMasterBar.LEVEL1 = QUESTIONS.getData(0x3114, 7).Int32();
                 vDryerMasterBar.LEVEL2 = QUESTIONS.getData(0x3114, 8).Int32();
                 vDryerMasterBar.ACT = QUESTIONS.getData(0x3114, 9).Int32();
                 vDryerMasterBar.INRANGE = QUESTIONS.getData(0x3114, 10).Byte(1);
                 vDryerMasterBar.PERCENTAGE = QUESTIONS.getData(0x3114, 10).Byte(0);
                 vDryerMasterBar.METHODOFFILLING = QUESTIONS.getData(0x3114, 10).Byte(2);
                 vDryerMasterBar.TYPE = QUESTIONS.getData(0x3114, 10).Byte(3); //0-->25/75 1-->0/100
                 JSON.DRYERMASTERBAR = vDryerMasterBar;
             }
         }
        //*/
    }
    private static void A_3000_SPL(ElektronikonRequest answers, List<ServicePlan> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            JSON[i].setData(answers.getData(0x3009, JSON[i].RTD_SI), answers.getData(0x3009, 1));
    }

    private static void A_3000_MS(ElektronikonRequest answers, List<ushort> JSON) {
        var vData = answers.getData(0x3001, 8);
        JSON[0] = vData.ToUInt16(0);
    }

    public static Question[] DataQuestions(JSONS vJSON) {
        ElektronikonRequest QUESTIONS = new ElektronikonRequest();
        AnalogInputs.Q_3000_AI(QUESTIONS, vJSON.ANALOGINPUTS);
        Q_3000_DI(QUESTIONS, vJSON.DIGITALINPUTS);
        Q_3000_CNT(QUESTIONS, vJSON.COUNTERS);
        Q_3000_CNV(QUESTIONS, vJSON.CONVERTERS);
        Q_3000_DO(QUESTIONS, vJSON.DIGITALOUTPUTS);
        Q_3000_CAI(QUESTIONS, vJSON.CALCULATEDANALOGINPUTS);

        Q_3000_SPR(QUESTIONS, vJSON.SPECIALPROTECTIONS);
        Q_3000_AO(QUESTIONS, vJSON.ANALOGOUTPUTS);
        Q_3000_SPM(QUESTIONS, vJSON.SPM2);
        Q_3000_ES(QUESTIONS);
        Q_3000_SPL(QUESTIONS, vJSON.SERVICEPLAN);
        Q_3000_MS(QUESTIONS);
        return QUESTIONS._requests.ToArray();
    }


    private static void Q_3000_DI(ElektronikonRequest QUESTIONS, List<DigitalInput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3003, JSON[i].RTD_SI);
    }

    private static void Q_3000_CNT(ElektronikonRequest QUESTIONS, List<Counter> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3007, JSON[i].RTD_SI);
    }

    private static void Q_3000_CNV(ElektronikonRequest QUESTIONS, List<Converter> JSON) {
        QUESTIONS.Add(0x3021, 5);
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3020 + JSON[i].RTD_SI, 1);
    }

    private static void Q_3000_DO(ElektronikonRequest QUESTIONS, List<DigitalOutput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3005, JSON[i].RTD_SI);
    }

    private static void Q_3000_CAI(ElektronikonRequest QUESTIONS, List<CalculatedAnalogInput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3004, JSON[i].RTD_SI);
    }

    private static void Q_3000_SPR(ElektronikonRequest QUESTIONS, List<SpecialProtection> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x300E, JSON[i].RTD_SI);
    }

    private static void Q_3000_AO(ElektronikonRequest QUESTIONS, List<AnalogOutput> JSON) {
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3006, JSON[i].RTD_SI);
    }

    private static void Q_3000_SPM(ElektronikonRequest QUESTIONS, List<SPM1> JSON) {
        for (var i = 0; i < JSON.Count; i++) {
            QUESTIONS.Add(0x3015, JSON[i].RTD_SI);
            QUESTIONS.Add(0x3015, JSON[i].RTD_SI + 1);
        }
    }

    private static void Q_3000_SPL(ElektronikonRequest QUESTIONS, List<ServicePlan> JSON) {
        QUESTIONS.Add(0x3009, 1);
        for (var i = 0; i < JSON.Count; i++)
            QUESTIONS.Add(0x3009, JSON[i].RTD_SI);
    }

    private static void Q_3000_MS(ElektronikonRequest QUESTIONS) {
        QUESTIONS.Add(0x3001, 8);
    }


    public static string GetRequestString(Question[] questions) {
        return GetRequestString(questions, 0, questions.Length);
    }

    public static string GetRequestString(Question[] questions, int from, int to) {
        StringBuilder sb = new StringBuilder();
        for (int idx = from; idx < to; idx += 1)
            sb.Append(questions[idx].Key);
        return sb.ToString();
    }

    // public IList<DataItem> GetRequests() {
    //     return _requests.AsReadOnly();
    // }

    public string GetRequestString() {
        return GetRequestString(0, Length);
    }

    public string GetRequestString(int from, int to) {
        StringBuilder sb = new StringBuilder();
        for (int idx = from; idx < to; idx += 1) 
            sb.Append(_requests[idx].Key);
        return sb.ToString();
    }
    public string GetDataString() {
        StringBuilder sb = new StringBuilder();
        foreach (Question item in _requests)
            if (item.Data.Str != "X")
                sb.Append(item + " ");
        return sb.ToString();
    }

    public string GetFullString() {
        StringBuilder sb = new StringBuilder();
        foreach (var item in _requests)
            sb.Append(item + " ");
        return sb.ToString();
    }


    public static JSONS ProcessConfig(ElektronikonRequest vQuestions) {
        var vJSON = new JSONS();
        AnalogInputs.A_2000_AI(vQuestions, vJSON.ANALOGINPUTS);
        A_2000_DI(vQuestions, vJSON.DIGITALINPUTS);
        A_2000_CNT(vQuestions, vJSON.COUNTERS);
        A_2000_CNV(vQuestions, vJSON.CONVERTERS);
        A_2000_DO(vQuestions, vJSON.DIGITALOUTPUTS);
        A_2000_CAI(vQuestions, vJSON.CALCULATEDANALOGINPUTS);

        A_2000_SPR(vQuestions, vJSON.SPECIALPROTECTIONS);
        A_2000_AO(vQuestions, vJSON.ANALOGOUTPUTS);
        A_2000_SPM(vQuestions, vJSON.SPM2);
        A_3000_ES(vQuestions, vJSON.ES); //!!! нет в чек боксах
        A_2000_SPL(vQuestions, vJSON.SERVICEPLAN);
        A_2000_MMT(vQuestions, vJSON.DEVICE); // !!! нет в чек боксах
        return vJSON;
    }


 

    private static void A_2000_DI(ElektronikonRequest vQuestions, List<DigitalInput> dIGITALINPUTS) {
        for (var i = 0x20b0; i < 0x2100; i++) {
            AnswerData data1 = vQuestions.getData(i, 1);
            if (data1.IsEmpty)
                continue;
            var byte0 = data1.ToByte(0);
            if (byte0 != 0) {
                var vDigitalInput = new DigitalInput {
                    MPL = data1.ToUInt16(1),
                    RTD_SI = i - 0x20b0 + 1
                };
                dIGITALINPUTS.Add(vDigitalInput);
            }
        }
    }

    private static void A_2000_CNT(ElektronikonRequest vQuestions, List<Counter> cOUNTERS) {
        for (var i = 1; i < 256; i++) {
            var data1 = vQuestions.getData(0x2607, i);
            if (data1.IsEmpty)
                continue;
            var byte0 = data1.ToByte(0);
            if (byte0 != 0) {
                var vCounter = new Counter {
                    MPL = data1.ToUInt16(1),
                    COUNTERUNIT = data1.ToByte(1),
                    RTD_SI = i
                };
                cOUNTERS.Add(vCounter);
            }
        }
    }

    private static void A_2000_CNV(ElektronikonRequest vQuestions, List<Converter> cONVERTERS) {
        for (var i = 0x2681; i < 0x2689; i++) {
            var data1 = vQuestions.getData(i, 1);
            if (data1.IsEmpty)
                continue;
            var byte0 = data1.ToByte(0);
            if (byte0 != 0) {
                var vConverter = new Converter {
                    CONVERTERTYPE = data1.ToByte(1),
                    CONVERTERDEVICETYPE = vQuestions.getData(i, 7).ToByte(0),
                    RTD_SI = i - 0x2681 + 1
                };
                cONVERTERS.Add(vConverter);
            }
        }
    }


    private static void A_2000_DO(ElektronikonRequest vQuestions, List<DigitalOutput> dIGITALOUTPUTS) {
        for (var i = 0x2100; i < 0x2150; i++) {
            var data1 = vQuestions.getData(i, 1);
            if (data1.IsEmpty)
                continue;
            var vStatus = data1.ToByte(0) != 0;
            if (vStatus) {
                var vDigitalOutput = new DigitalOutput {
                    MPL = vQuestions.getData(i, 1).ToUInt16(1),
                    RTD_SI = i - 0x2100 + 1
                };
                dIGITALOUTPUTS.Add(vDigitalOutput);
            }
        }
    }
    private static void A_2000_CAI(ElektronikonRequest vQuestions, List<CalculatedAnalogInput> cALCULATEDANALOGINPUTS) {
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


    private static void A_2000_SPR(ElektronikonRequest vQuestions, List<SpecialProtection> sPECIALPROTECTIONS) {
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

    private static void A_2000_AO(ElektronikonRequest vQuestions, List<AnalogOutput> aNALOGOUTPUTS) {
        for (var i = 0x2150; i < 0x2170; i++) {
            var data1 = vQuestions.getData(i, 1);
            if (data1.IsEmpty)
                continue;
            var vStatus = data1.Byte(0) != 0;
            if (vStatus) {
                var vAnalogOutput = new AnalogOutput {
                    MPL = vQuestions.getData(i, 1).UInt16(1),
                    OUTPUTTYPE = vQuestions.getData(i, 1).Byte(1),
                    DISPLAYPRECISION = vQuestions.getData(i, 3).Byte(3),
                    RTD_SI = i - 0x2150 + 1,
                };
                aNALOGOUTPUTS.Add(vAnalogOutput);

            }    
        }
    }

    private static void A_2000_SPM(ElektronikonRequest vQuestions, List<SPM1> sPM2) {
        for (var i = 0x2560; i < 0x2570; i++) {
            var data1 = vQuestions.getData(i, 1);
            if (data1.IsEmpty)
                continue;

            var vStatus = data1.Byte(0) != 0;
            if (vStatus) {
                var vSPM = new SPM1 {
                    MPL = vQuestions.getData(i, 1).UInt16(1),
                    RTD_SI = 2 * (i - 0x2560) + 1,
                };
                sPM2.Add( vSPM);
            }
        }

    }

    public static int GetServicePlanRTD_SI(int i) {
        return i % 2 == 0 ? (16 + i / 2) : (6 + (i - 1) / 2);
    }

    private static void A_2000_SPL(ElektronikonRequest QUESTIONS, List<ServicePlan> sERVICEPLAN) {
        for (var i = 1; i < 21; i++) {
            var datai = QUESTIONS.getData(0x2602, i);
            var vSTATICVALUE = datai.UInt32();
            if (vSTATICVALUE != 0) {
                var vServicePlan = new ServicePlan {
                    STATICVALUE = vSTATICVALUE,
                    RTD_SI = GetServicePlanRTD_SI(i),
                    LEVEL = Math.Ceiling(i / 2.0),
                    Type = (i % 2),
                };
                sERVICEPLAN.Add(vServicePlan);
            }
        }
    }

    private static void A_2000_MMT(ElektronikonRequest vQuestions, List<ushort> dEVICE) {
        byte data = vQuestions.getData(0x2001, 1).Byte(0);
        dEVICE.Add(data);
    }

    public static Question[] ConfigQuestions {
        get {
            ElektronikonRequest er = new ElektronikonRequest();
            AnalogInputs.Q_2000_AI(er);
            Q_2000_DI(er);
            Q_2000_CNT(er);
            Q_2000_CNV(er); // 1st
            Q_2000_DO(er);
            Q_2000_CAI(er);

            Q_2000_CNV(er); // 2nd for unknown reason !

            Q_2000_SPR(er);
            Q_2000_AO(er);
            Q_2000_SPM(er);
            Q_3000_ES(er);
            Q_2000_SPL(er);
            Q_2000_MMT(er);
            return er._requests.ToArray();
        }
    }



private static void Q_2000_DI(ElektronikonRequest er) {
        for (var i = 0x20b0; i < 0x2100; i++)
            er.Add(i, 1);
    }

    private static void Q_2000_CNT(ElektronikonRequest er) {
        for (var i = 1; i < 256; i++)
            er.Add(0x2607, i);
    }

    private static void Q_2000_CNV(ElektronikonRequest er) {
        for (var i = 0x2681; i < 0x2689; i++) {
            er.Add(i, 1);
            er.Add(i, 7);
        }
    }

    private static void Q_2000_DO(ElektronikonRequest er) {
        for (var i = 0x2100; i < 0x2150; i++)
            er.Add(i, 1);
    }

    private static void Q_2000_CAI(ElektronikonRequest er) {
        for (var i = 0x2090; i < 0x20b0; i++) {
            er.Add(i, 1);
            er.Add(i, 3);
        }
    }

    public AnswerData getData(int index, int subindex) {
        for (var i = 0; i < _requests.Count; i++) {
            var request = _requests[i];
            if ((request.Index == index) && (request.Subindex == subindex)) 
                return request.Data;            
        }
        return AnswerData.Empty;
    }

    private static void Q_2000_SPR(ElektronikonRequest er) {
        for (var i = 0x2300; i < 0x247F; i++)
            er.Add(i, 1);
    }

    private static void Q_2000_AO(ElektronikonRequest er) {
        for (var i = 0x2150; i < 0x2170; i++) {
            er.Add(i, 1);
            er.Add(i, 3);
        }
    }

    private static void Q_2000_SPM(ElektronikonRequest er) {
        for (var i = 0x2560; i < 0x2570; i++)
            er.Add(i, 1);
    }

    private static void Q_3000_ES(ElektronikonRequest er) {        
        er.Add(0x3113, 1);
        for (var i = 3; i <= 5; i++)
            er.Add(0x3113, i);
        for (var i = 7; i <= 30; i++)
            er.Add(0x3113, i);
        for (var i = 31; i <= 42; i++)
            er.Add(0x3113, i);
        for (var i = 1; i <= 18; i++)
            er.Add(0x3114, i);
    }

    private static void Q_2000_SPL(ElektronikonRequest er) {
        for (var i = 1; i < 21; i++)
            er.Add(0x2602, i);
    }

    private static void Q_2000_MMT(ElektronikonRequest er) {
        er.Add(0x2001, 1);
    }

}

