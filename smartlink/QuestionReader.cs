using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace smartlink;

public class QuestionReader {
    public async Task<ElektronikonRequest> Run(IElektronikonClient client) {
        var list = ElektronikonRequest.SettingsQuestions;
        ElektronikonRequest vQuestionsAll = await ReadSettings(list, client);

        ElektronikonRequest vQuestions = vQuestionsAll.SparseQuestions();

        var vJSON = new JSON();

        return vQuestions;


        A_2000_AI(vQuestions, vJSON.ANALOGINPUTS);
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

        /* create_checkbox('ANALOGINPUTS', 1, vJSON.ANALOGINPUTS);
        create_checkbox('ANALOGOUTPUTS', 2, vJSON.ANALOGOUTPUTS);
        create_checkbox('COUNTERS', 3, vJSON.COUNTERS);
        create_checkbox('CONVERTERS', 4, vJSON.CONVERTERS);
        create_checkbox('DIGITALINPUTS', 6, vJSON.DIGITALINPUTS);
        create_checkbox('DIGITALOUTPUTS', 7, vJSON.DIGITALOUTPUTS);
        create_checkbox('SPECIALPROTECTIONS', 8, vJSON.SPECIALPROTECTIONS);
        create_checkbox('SERVICEPLAN', 9, vJSON.SERVICEPLAN);
        create_checkbox('CALCULATEDANALOGINPUTS', 10, vJSON.CALCULATEDANALOGINPUTS);
        create_checkbox('SPM', 11, vJSON.SPM2);

        create_tables();
        refresh_data(); */


    }

    private void A_2000_MMT(ElektronikonRequest vQuestions, object dEVICE) {
        throw new NotImplementedException();
    }

    private void A_2000_SPL(ElektronikonRequest vQuestions, object sERVICEPLAN) {
        throw new NotImplementedException();
    }

    private void A_3000_ES(ElektronikonRequest vQuestions, object eS) {
        throw new NotImplementedException();
    }

    private void A_2000_SPM(ElektronikonRequest vQuestions, object sPM2) {
        throw new NotImplementedException();
    }

    private void A_2000_AO(ElektronikonRequest vQuestions, object aNALOGOUTPUTS) {
        throw new NotImplementedException();
    }

    private void A_2000_SPR(ElektronikonRequest vQuestions, object sPECIALPROTECTIONS) {
        throw new NotImplementedException();
    }

    private void A_2000_CAI(ElektronikonRequest vQuestions, object cALCULATEDANALOGINPUTS) {
        throw new NotImplementedException();
    }

    private void A_2000_DO(ElektronikonRequest vQuestions, object dIGITALOUTPUTS) {
        throw new NotImplementedException();
    }

    private void A_2000_CNV(ElektronikonRequest vQuestions, object cONVERTERS) {
        throw new NotImplementedException();
    }

    private void A_2000_CNT(ElektronikonRequest vQuestions, object cOUNTERS) {
        throw new NotImplementedException();
    }

    private void A_2000_DI(ElektronikonRequest vQuestions, object dIGITALINPUTS) {
        throw new NotImplementedException();
    }

    private void A_2000_AI(ElektronikonRequest questions, List<AnalogInput> aNALOGINPUTS) {
        for (var i = 0x2010; i < 0x2090; i++) {
            AnswerData data1 = questions.getData(i, 1);
            var byte0 = data1.ToByte(0);
            if (byte0 != 0) {
                var data4 = questions.getData(i, 4);
                var vAnalogInput = new AnalogInput {
                    MPL = data1.ToUInt16(1),
                    INPUTTYPE = data1.ToByte(1),
                    DISPLAYPRECISION = data4.ToByte(3),
                    RTD_SI = i - 0x2010 + 1
                };
                aNALOGINPUTS.Add(vAnalogInput);
            }
            else {
                // TODO: invalidate data
            }
        }
    }

    public static async Task<ElektronikonRequest> ReadSettings(Question[] questions, IElektronikonClient client) {
        ElektronikonRequest request = new ElektronikonRequest();
        // Elektronikon kontroller can process max 1000 questions. Otherwise it can hang 
        const int step1000 = 1000;
        for (int idx = 0; idx < questions.Length; idx += step1000) {
            int to = Math.Min(idx + step1000, questions.Length);
            string questionsString = ElektronikonRequest.GetRequestString(questions, idx, to);
            string answersString = await client.AskAsync(questionsString);
            for (int iQ = idx, iA = 0; iQ < to; iQ++) {
                var question = questions[iQ];
                if (answersString != null && answersString[iA] != 'X') {
                    string substring = answersString.Substring(iA, 8);
                    var newQuestion = new Question(question.Index, question.Subindex, substring);
                    request.Add(newQuestion);
                    iA += 8;
                }
                else {
                    var newQuestion = new Question(question.Index, question.Subindex, "X");
                    request.Add(newQuestion);
                    iA++;
                }
            }
        }
        return request;
    }
}

public class JSON {
    public JSON() {
    }

    public List<AnalogInput> ANALOGINPUTS { get; set; } = new List<AnalogInput>();
    public List<object> DIGITALINPUTS { get; set; } = new List<object> { };
    public List<object> COUNTERS { get; set; } = new List<object> { };
    public List<object> CONVERTERS { get; set; } = new List<object> { };
    public List<object> DIGITALOUTPUTS { get; set; } = new List<object> { };
    public List<object> CALCULATEDANALOGINPUTS { get; set; } = new List<object> { };
    public List<object> SPECIALPROTECTIONS { get; set; } = new List<object> { };
    public List<object> ANALOGOUTPUTS { get; set; } = new List<object> { };
    public List<object> SPM2 { get; set; } = new List<object> { };
    public List<object> ES { get; set; } = new List<object> { };
    public List<object> SERVICEPLAN { get; set; } = new List<object> { };
    public List<object> DEVICE { get; set; } = new List<object> { };
}

public class AnalogInput {
    public ushort MPL { get; set; }
    public byte INPUTTYPE { get; set; }
    public byte DISPLAYPRECISION { get; set; }
    public int RTD_SI { get; set; }
}