using smartlink.JsonData;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace smartlink;
class Worker {
    readonly ManualResetEvent _mainExitEvent;
    readonly ManualResetEvent _workEndedEvent;
    readonly CompressorInfo _info;
    readonly SlaveStorage _storage;

    const int READ_INTERVAL = 10000;

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
            // IP/Russian.txt
            // IP/Machine.txt  
            /* FileVer = 1.0
            Model = ALLEG22_12
            Generation = 5
            Serial = CAI897523
            OS = 1900523096
            App = 1900525449
            WebApp = 1900524830
            RegulationType = 12 */
            var url = "http://" + _info.Cip + "/cgi-bin/mkv.cgi";
            Console.WriteLine($"ElektronikonReader reading url: {url}");
            var client = new HttpElektronikonClient(url);
            JSONS? json = null;
            // загрузка настроек
            do {
                try {
                    json = await reader.LoadSettings(client);
                    break;
                }
                catch (HttpRequestException ex) {
                    Console.WriteLine($"{_info} " + ex);
                }
                catch (ArgumentOutOfRangeException ex) {
                    Console.WriteLine($"{_info} " + ex);
                }
            } while (!_mainExitEvent.WaitOne(READ_INTERVAL));

            if (!_mainExitEvent.WaitOne(1)) {
                // загрузка актуальных данных
                do {
                    try {
                        await reader.RefreshData(client, json!, _info);

                        // если сюда дошли значит нормально прочитали данные, можно обновить время и данные.
                        FillDataToStorage(json!, _storage);
                        //LogStorageCurrentData(_storage);
                    }
                    catch (HttpRequestException ex) {
                        Console.WriteLine($"{_info} " + ex);
                    }
                } while (!_mainExitEvent.WaitOne(READ_INTERVAL));
            }
        }
        catch (Exception e) {
            Console.WriteLine($"{_info} Неизвестная ошибка! " + e);
            ResetStorageCurrentData(_storage);
        }
        Console.WriteLine($"thread stopped {_info}");
        _workEndedEvent.Set(); // событие того, что работа закончена
    }

    private static void LogStorageCurrentData(SlaveStorage storage) {
        var number = storage.Number;
        var time = storage.Time;
        var pressure = storage.Pressure;
        var temperature = storage.Temperature;
        var workstate = storage.WorkState;
        var flow = storage.Flow;
        var (t1, t2, t3) = (time / 1000, (time % 1000) / 10, (time % 10) * 10);
        string strtime = $"{t1}:{t2}:{t3}";
        Console.WriteLine($"CurrentData: №{number}, time={strtime}, pressure={pressure}бар, temperature={temperature}°С, workstate={workstate}, flow={flow}%");
    }

    private void ResetStorageCurrentData(SlaveStorage storage) {
        storage.Time = GetCurrentTime();
        storage.Pressure = 0;
        storage.Temperature = 0;
        storage.WorkState = 0;
        storage.Flow = 0;
    }

    // we get only few data
    public class WorkVisitor : IVisitor {
        SlaveStorage _storage;
        public WorkVisitor(SlaveStorage storage) {
            _storage = storage;
        }

        public void VisitAnalogInputs(IViewCreator creator, List<AnalogInput> list) {
            if (list.Count == 0)
                return;
            foreach (AnalogInput item in list) {
                if (item.RTD_SI == 1) {
                    _storage.Pressure = (float)((item.getValue() / 100) / 10.0);
                }

                // у этих компрессоров температура выхода ступени находится в параметре RTD_SI==2
                if (_storage.Number == 14 || _storage.Number == 16) {
                    if (item.RTD_SI == 2) 
                        _storage.Temperature = item.getValue() / 10;
                }
                else {
                    if (item.RTD_SI == 3)
                        _storage.Temperature = item.getValue() / 10;
                }            
            }
        }

        public void VisitAnalogOutputs(IViewCreator creator, List<AnalogOutput> list) {
        }

        public void VisitCalculatedAnalogInputs(IViewCreator creator, List<CalculatedAnalogInput> list) {
        }

        public void VisitConverters(IViewCreator creator, List<Converter> list) {
            if (list.Count == 0)
                return;
            var item = list[0];
            // считаем, что если у нас есть частотник, то он только один частотник и первый в списке, расход в процентах
            ushort value = item.getFlow();
            _storage.Flow = value;
        }

        public void VisitCounters(IViewCreator creator, List<Counter> list) {
        }

        public void VisitDevices(IViewCreator creator, List<ushort> list) {
            if (list.Count == 0)
                return;
            ushort value = list[0];
            // считаем, что если у нас есть состояние машины, то оно одно и первое в списке
            _storage.WorkState = value;
        }

        public void VisitDigitalInputs(IViewCreator creator, List<DigitalInput> list) {
        }

        public void VisitDigitalOutputs(IViewCreator creator, List<DigitalOutput> list) {
        }

        public void VisitServicePlans(IViewCreator creator, List<ServicePlan> list) {
        }

        public void VisitSpecialProtections(IViewCreator creator, List<SpecialProtection> list) {
        }

        public void VisitSpms(IViewCreator creator, List<SPM> list) {
        }
    }

    private static void FillDataToStorage(JSONS json, SlaveStorage storage) {
        storage.Time = GetCurrentTime();
        WorkVisitor visitor = new WorkVisitor(storage);
        json.ANALOGINPUTS.Visit(visitor);
        json.CONVERTERS.Visit(visitor);
        json.DEVICE.Visit(visitor);
    }

    private static ushort GetCurrentTime() {
        var now = DateTime.Now;
        return (ushort)(now.Hour * 1000 + now.Minute * 10 + now.Second / 10);
    }
}
