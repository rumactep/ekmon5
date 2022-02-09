using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace smartlink;

public class QuestionReader {
    public ILogger Logger { get; set; } = NoLogger.Instance;
    public async Task Run(IElektronikonClient client) {        
        var list = ElektronikonRequest.ConfigQuestions;
        
        ElektronikonRequest config = await SendReceive(list, client);
        ElektronikonRequest sparsedConfig = config.SparseQuestions();
        Logger.Log("sparsed config:", sparsedConfig.GetRequestString());

        JSON json = ElektronikonRequest.ProcessConfig(sparsedConfig);

        string strlog = json.ToString()!;
        Logger.Log("json:", strlog);
        //create_tables();
        var dataQuestions = ElektronikonRequest.DataQuestions(json);
        ElektronikonRequest answers = await SendReceive(dataQuestions, client);
        ElektronikonRequest.ProcessData(answers, json);
        strlog = json.ToString()!;
        Logger.Log("json:", strlog);

        ProcessView(json);
    }

    private void ProcessView(JSON vJSON) {
        Console.WriteLine(vJSON);
    }

    public static async Task<ElektronikonRequest> SendReceive(Question[] questions, IElektronikonClient client, ILogger? logger = null) {
        if (logger == null) 
            logger = NoLogger.Instance;
            
        ElektronikonRequest request = new ElektronikonRequest();
        // Elektronikon kontroller can process max 1000 questions. Otherwise it can hang 
        
        const int step1000 = 1000;
        for (int idx = 0; idx < questions.Length; idx += step1000) {
            int to = Math.Min(idx + step1000, questions.Length);
            string questionsString = ElektronikonRequest.GetRequestString(questions, idx, to);
            logger.Log("questionsString:", questionsString);

            // for each 6 chars of question we receive 8 chars of answer
            string answersString = await client.AskAsync(questionsString);
            if (answersString != null) {
                logger.Log("answersString:", answersString);
                for (int iQ = idx, iA = 0; iQ < to; iQ++) {
                    var question = questions[iQ];
                    if (iA >= answersString.Length)
                        // wrong or partial answer. I don't know what to do
                        break;
                    if (answersString[iA] != 'X' && iA < answersString.Length + 8) {
                        string substring = answersString.Substring(iA, 8);
                        var newQuestion = new Question(question.Index, question.Subindex, substring);
                        request.Add(newQuestion);
                        iA += 8;
                    }
                    else {
                        //var newQuestion = new Question(question.Index, question.Subindex, "X");
                        //request.Add(newQuestion);
                        iA++;
                    }
                }
            }
        }
        return request;
    }
}

public interface ILogger {
    void Log(string message);
    void Log(string prefix, string message);
}

public class ConsoleLogger : ILogger{
    public static readonly ConsoleLogger Instance = new ConsoleLogger();
    public void Log(string message) {
        Console.WriteLine(message);
    }
    public void Log(string prefix, string message) {
        Console.WriteLine(prefix + " " + message);
    }
}
public class NoLogger : ILogger {
    public static readonly NoLogger Instance = new NoLogger();
    public void Log(string message) { }
    public void Log(string prefix, string message) {}
}
