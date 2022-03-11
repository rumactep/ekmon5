using smartlink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace smartlinkserver {
    class Worker {
        readonly ManualResetEvent _mainExitEvent;
        readonly ManualResetEvent _workEndedEvent;
        readonly CompressorInfo _info;
        readonly SlaveStorage _storage;

        public Worker(ManualResetEvent mainExitEvent, ManualResetEvent workEndedEvent, CompressorInfo info, SlaveStorage storage) {
            _mainExitEvent = mainExitEvent;
            _workEndedEvent = workEndedEvent;
            _info = info;
            _storage = storage;
        }
        public async void ThreadProc() {
            Console.WriteLine($"thread {_info} started");
            var reader = new QuestionReader {
                Logger = ConsoleLogger.Instance
            };
            try {
                reader.LoadLanguage("Russian.txt");
                var url = "http://" + _info.Cip + "/cgi-bin/mkv.cgi";
                Console.WriteLine($"ElektronikonReader reading url: {url}");
                var client = new HttpElektronikonClient(url);

                do {
                    try {
                        await reader.Run(client);
                    }
                    catch (HttpRequestException ex) {
                        Console.WriteLine($"{_info} " + ex);
                    }
                    Console.WriteLine("");
                } while (!_mainExitEvent.WaitOne(10000));
            }
            catch (Exception e) {
                Console.WriteLine($"{_info} Неизвестная ошибка! " + e);
            }
            Console.WriteLine($"thread {_info} stopped");
            _workEndedEvent.Set(); // событие того, что работа зкончена
        }
    }
}
