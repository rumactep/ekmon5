using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace smartlink;

public class QuestionReader {
    public async Task Run(IElektronikonClient client) {
        
        var list = ElektronikonRequest.FormSettingsQuestions;

        ElektronikonRequest questionsSettings = await SendReceive(list, client);

        ElektronikonRequest questionsData = questionsSettings.SparseQuestions();
        var vJSON = new JSON();
        ElektronikonRequest.ProcessSettings(questionsData, vJSON);
        
        //create_tables();
        ElektronikonRequest answersData = await SendReceive(ElektronikonRequest.FormDataQuestions(vJSON), client);
        ElektronikonRequest.ProcessData(answersData, vJSON);

        ProcessView(vJSON);
    }

    private void ProcessView(JSON vJSON) {
        Console.WriteLine(vJSON);
    }


    public static async Task<ElektronikonRequest> SendReceive(Question[] questions, IElektronikonClient client) {
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
