using System;
using System.Threading;
using System.Threading.Tasks;
using smartlink.JsonData;
using Newtonsoft.Json;

namespace smartlink {
    public class QuestionReader {
        Language _language = new();

        public void LoadLanguage(string languagefilename) {
            var loader = new LanguageLoader();
            loader.LoadLanguage(languagefilename, _language);
            Logger.Log($"loaded {_language.StringCount} lines from {languagefilename}");
        }

        public ILogger Logger { get; set; } = ILogger.Null;



        //TODO: разделить на 2 метода загрузку настроек и загрузку текущей информации
        public async Task Run(IElektronikonClient client) {
            var list = ElektronikonRequest.ConfigQuestions;
            ElektronikonRequest config = await SendReceive(list, client, Logger);
            ElektronikonRequest sparsedConfig = config.SparseQuestions();
            JSONS json = ElektronikonRequest.ProcessConfig(sparsedConfig);
            var dataQuestions = ElektronikonRequest.DataQuestions(json);
            ElektronikonRequest answers = await SendReceive(dataQuestions, client, Logger);
            ElektronikonRequest.ProcessData(answers, json);
            ProcessView(json);
        }

        private void ProcessView(JSONS vJSON) {
            StringVisitor visitor = new StringVisitor(new StringPartWriter(), _language);
            vJSON.Accept(visitor);
            string str = visitor.Text;
            string strjson = JsonConvert.SerializeObject(vJSON);

            Logger.Log(strjson);
        }

        public static async Task<ElektronikonRequest> SendReceive(Question[] questions, IElektronikonClient client, ILogger logger) {

            ElektronikonRequest request = new ElektronikonRequest();

            // Elektronikon kontroller can process max 1000 questions. Otherwise it can hang        
            const int step1000 = 1000;

            for (int idx = 0; idx < questions.Length; idx += step1000) {
                int to = Math.Min(idx + step1000, questions.Length);
                string questionsString = ElektronikonRequest.GetRequestString(questions, idx, to);
                //logger.Log("questionsString:", questionsString);

                // for each 6 chars of question we receive 8 chars of answer or 'X'
                string answersString = await client.AskAsync(questionsString);
                //logger.Log("answersString:", answersString);
                for (int iQ = idx, iA = 0; iQ < to; iQ++) {
                    Question question = questions[iQ];
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
            return request;
        }
    }

    public interface ILogger {
        void Log(string message);
        void Log(string prefix, string message);

        public static ILogger Null => NoLogger.Instance;

        private sealed class NoLogger : ILogger {
            private static readonly Lazy<NoLogger> instance = new(() => new NoLogger());
            public static ILogger Instance => instance.Value;

            //public static readonly NoLogger Instance = new NoLogger();
            public void Log(string message) { }
            public void Log(string prefix, string message) { }
        }

    }

    public class ConsoleLogger : ILogger {
        public static readonly ConsoleLogger Instance = new ConsoleLogger();
        public void Log(string message) {
            Console.WriteLine(message);
        }
        public void Log(string prefix, string message) {
            Console.WriteLine(prefix + " " + message);
        }
    }

}