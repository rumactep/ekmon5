using System;
using System.Threading.Tasks;

namespace smartlink;

public class QuestionReader {
    public async Task<ElektronikonRequest> Run(DataItem[] questions, IElektronikonClient client) {
        return await ReadSettings(questions, client);

/*
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

        create_checkbox('ANALOGINPUTS', 1, vJSON.ANALOGINPUTS);
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

    private static async Task<ElektronikonRequest> ReadSettings(DataItem[] questions, IElektronikonClient client) {
        ElektronikonRequest request = new ElektronikonRequest();
        const int step1000 = 1000;
        for (int idx = 0; idx < questions.Length; idx += step1000) {
            int to = Math.Min(idx + step1000, questions.Length);
            string questionsString = ElektronikonRequest.GetRequestString(questions, idx, to);
            string answersString = await client.AskAsync(questionsString);
            for (int iQ = idx, iA = 0; iQ < to; iQ++) {
                var question = questions[iQ];
                if (answersString != null && answersString[iA] != 'X') {
                    string substring = answersString.Substring(iA, 8);
                    var newQuestion = new DataItem(question.Index, question.SubIndex, substring);
                    request.Add(newQuestion);
                    iA += 8;
                }
                else {
                    var newQuestion = new DataItem(question.Index, question.SubIndex, "X");
                    request.Add(newQuestion);
                    iA++;
                }
            }
        }
        return request;
    }
}