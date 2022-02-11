using System;
using System.Collections.Generic;
using System.Text;
using smartlink.JsonData;

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
        DigitalInputs.A_3000_DI(answers, json.DIGITALINPUTS);
        Counters.A_3000_CNT(answers, json.COUNTERS);
        Converters.A_3000_CNV(answers, json.CONVERTERS);
        DigitalOutputs.A_3000_DO(answers, json.DIGITALOUTPUTS);
        CalculatedAnalogInputs.A_3000_CAI(answers, json.CALCULATEDANALOGINPUTS);

        SpecialProtections.A_3000_SPR(answers, json.SPECIALPROTECTIONS);
        AnalogOutputs.A_3000_AO(answers, json.ANALOGOUTPUTS);
        SPMs.A_3000_SPM(answers, json.SPM2);
        A_3000_ES(answers, json.ES);
        ServicePlans.A_3000_SPL(answers, json.SERVICEPLAN);
        Devices.A_3000_MS(answers, json.DEVICE);
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

    public static Question[] DataQuestions(JSONS vJSON) {
        ElektronikonRequest QUESTIONS = new ElektronikonRequest();
        AnalogInputs.Q_3000_AI(QUESTIONS, vJSON.ANALOGINPUTS);
        DigitalInputs.Q_3000_DI(QUESTIONS, vJSON.DIGITALINPUTS);
        Counters.Q_3000_CNT(QUESTIONS, vJSON.COUNTERS);
        Converters.Q_3000_CNV(QUESTIONS, vJSON.CONVERTERS);
        DigitalOutputs.Q_3000_DO(QUESTIONS, vJSON.DIGITALOUTPUTS);
        CalculatedAnalogInputs.Q_3000_CAI(QUESTIONS, vJSON.CALCULATEDANALOGINPUTS);

        SpecialProtections.Q_3000_SPR(QUESTIONS, vJSON.SPECIALPROTECTIONS);
        AnalogOutputs.Q_3000_AO(QUESTIONS, vJSON.ANALOGOUTPUTS);
        SPMs.Q_3000_SPM(QUESTIONS, vJSON.SPM2);
        Q_3000_ES(QUESTIONS);
        ServicePlans.Q_3000_SPL(QUESTIONS, vJSON.SERVICEPLAN);
        Devices.Q_3000_MS(QUESTIONS);
        return QUESTIONS._requests.ToArray();
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
        DigitalInputs.A_2000_DI(vQuestions, vJSON.DIGITALINPUTS);
        Counters.A_2000_CNT(vQuestions, vJSON.COUNTERS);
        Converters.A_2000_CNV(vQuestions, vJSON.CONVERTERS);
        DigitalOutputs.A_2000_DO(vQuestions, vJSON.DIGITALOUTPUTS);
        CalculatedAnalogInputs.A_2000_CAI(vQuestions, vJSON.CALCULATEDANALOGINPUTS);

        SpecialProtections.A_2000_SPR(vQuestions, vJSON.SPECIALPROTECTIONS);
        AnalogOutputs.A_2000_AO(vQuestions, vJSON.ANALOGOUTPUTS);
        SPMs.A_2000_SPM(vQuestions, vJSON.SPM2);
        A_3000_ES(vQuestions, vJSON.ES); //!!! нет в чек боксах
        ServicePlans.A_2000_SPL(vQuestions, vJSON.SERVICEPLAN);
        Devices.A_2000_MMT(vQuestions, vJSON.DEVICE); // !!! нет в чек боксах
        return vJSON;
    }


    public static int GetServicePlanRTD_SI(int i) {
        return i % 2 == 0 ? (16 + i / 2) : (6 + (i - 1) / 2);
    }

    public static Question[] ConfigQuestions {
        get {
            ElektronikonRequest er = new ElektronikonRequest();
            AnalogInputs.Q_2000_AI(er);
            DigitalInputs.Q_2000_DI(er);
            Counters.Q_2000_CNT(er);
            Converters.Q_2000_CNV(er); // 1st
            DigitalOutputs.Q_2000_DO(er);
            CalculatedAnalogInputs.Q_2000_CAI(er);

            Converters.Q_2000_CNV(er); // 2nd for unknown reason !

            SpecialProtections.Q_2000_SPR(er);
            AnalogOutputs.Q_2000_AO(er);
            SPMs.Q_2000_SPM(er);
            Q_3000_ES(er);
            ServicePlans.Q_2000_SPL(er);
            Devices.Q_2000_MMT(er);
            return er._requests.ToArray();
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
}

