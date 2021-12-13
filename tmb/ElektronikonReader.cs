using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace ek2mb {
    public class ElektronikonReader {
        private IWebDriver _driver;
        private bool _goodBrowser = false;
        private const int IMPLICIT_WAIT = 5;
        private const int TIMEOT_OPENING = 90;
        public SlaveStorage Storage { get; }

        public ElektronikonReader(SlaveStorage storage, int delay) {
            Storage = storage;
            Delay = delay;
        }

        public int Delay { get; set; }

        public static void MakeHttpIfNeed(ref string arg) {
            if (!arg.StartsWith("http://") && !arg.StartsWith("https://"))
                arg = "http://" + arg;
        }

        void TryNavigate(string url) {
            try {
                _driver.Navigate().GoToUrl(url);
                _goodBrowser = true;
            }
            catch (WebDriverException ex) {
                Console.WriteLine("{0}, {1}", url, ex);
                _goodBrowser = false;
            }
            catch (Exception ex) {
                Console.WriteLine("{0}, {1}", url, ex);
                _goodBrowser = false;
            }
        }

        public void ReadDataThreadAsync() {
            Thread.Sleep(Delay * 2000);
            try {
                string url = Storage.Info.Cip;
                MakeHttpIfNeed(ref url);

                while (true) {
                    if (_driver == null) {
                        _driver = GetWebDriver();
                        for (int i = 0; i < 10; i++)
                            Thread.Sleep(1000);
                    }
                    else {
                        if (_goodBrowser)
                            ReadData(url, Storage);
                        else
                            TryNavigate(url);

                        for (int i = 0; i < 10; i++) {
                            Thread.Sleep(1000);
                        }

                        Task.Delay(1000);
                    }
                }
            }
            catch (WebDriverException e) {
                Console.WriteLine(
                    "==========================================================================");
                Console.WriteLine(e);
                throw;
            }
        }

        // ParseWorkState: Зaгpyзкa
        ushort ParseWorkState(string str) {
            // "Возможны символы некириллицы!"
            switch (str) {
                case "Зaгpyзкa":
                    return 4;
                case "Загрузка":
                    return 4;
                case "Разгрузка":
                    return 3;                
                case "Пoдaчa Питaния ECO":
                    return 5;
                default:
                    Console.WriteLine($"ParseWorkState: {str}");
                    // "0 - (неизвестно, нет связи) 1 - авария, 2 - стоп, 3 - разгрузка"
                    return 0;
            }
        }

        // ParseFlow: 90 %
        ushort ParseFlow(string str) {
            str = str.Replace(" %", "");
            if (UInt16.TryParse(str, out ushort res))
                return res;
            Console.WriteLine($"ParseFlow: {str}");
            return 0;
        }

        // ParsePressure: 8.9 bar
        float ParsePressure(string str) {
            string s2 = str.Replace(" bar", "");
            bool b = float.TryParse(s2, NumberStyles.Any, CultureInfo.InvariantCulture,
                out float res);
            if (b)
                return res;
            else {
                Console.WriteLine($"ParsePressure: {str}");
                return 0.0f;
            }
        }

        // ParseTemperature: 88 °C
        float ParseTemperature(string str) {
            str = str.Replace(" °C", "");
            if (float.TryParse(str, out float res))
                return res;
            Console.WriteLine($"ParseTemperature: {str}");
            return 0.0f;
        }

        string ReadWorkState() {
            // <td id = "MACHINESTATER0C1">Cocтoяниe Maшины</td>
            // <td id = "MACHINESTATER0C2">Зaгpyзкa</td >
            IWebElement header = _driver.FindElement(By.Id("MACHINESTATER0C1"));
            if (header.Text == "Cocтoяниe Maшины" || header.Text == "Machine Status")
                return _driver.FindElement(By.Id("MACHINESTATER0C2")).Text;
            return "";
        }

        string ReadFlow() {
            // <td id = "FLOWR1C1">Pacxoд</td>
            // <td id = "FLOWR1C3">90 %</td>
            try {
                IWebElement header = _driver.FindElement(By.Id("FLOWR1C1"));
                // !!! один расход с русским буквами, другой с английскими
                if (header.Text == "Расход" || header.Text == "Flow" || header.Text == "Pacxoд")
                    return _driver.FindElement(By.Id("FLOWR1C3")).Text;
            }
            catch (NoSuchElementException) {
                return "";
            }

            return "";
        }

        string ReadPressure() {
            // <td id = "ANALOGINPUTSR0C1">Дaвлeниe Ha Bыxoдe</td>
            // <td id = "ANALOGINPUTSR0C2">9.9 bar</td>
            try {
                IWebElement header = _driver.FindElement(By.Id("ANALOGINPUTSR0C1"));
                if (header.Text == "Дaвлeниe Ha Bыxoдe" || header.Text == "Compressor Outlet")
                    return _driver.FindElement(By.Id("ANALOGINPUTSR0C2")).Text;
            }
            catch (NoSuchElementException) {
                return "";
            }
            return "";
        }

        string ReadTemperature() {
            // <td id = "ANALOGINPUTSR1C1">Bыxoд Cтyпeни</td>
            // <td id = "ANALOGINPUTSR1C2">68 °C</td>
            try {
                IWebElement header = _driver.FindElement(By.Id("ANALOGINPUTSR1C1"));
                if (header.Text == "Bыxoд Cтyпeни" || header.Text == "Element Outlet")
                    return _driver.FindElement(By.Id("ANALOGINPUTSR1C2")).Text;
            }
            catch (NoSuchElementException) {
                return "";
            }
            return "";
        }

        private string ReadTime() {
            // <span id="load_time">Cтpaницa Oбнoвлeнa: 15:52:1  3/12/2021</span>
            IWebElement element = _driver.FindElement(By.Id("load_time"));
            if (element.Text.Contains("Cтpaницa Oбнoвлeнa: ") || element.Text.Contains("Page displayed at ")) {
                string time = element.Text.Replace("Cтpaницa Oбнoвлeнa: ", "")
                    .Replace("Page displayed at ", "");
                return time;
            }
            return "0:0:0 0/0/0";
        }

        string ReadSerial() {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            IWebElement serial = _driver.FindElement(By.Id("serial"));
            var elapsed = stopwatch.Elapsed;
            var timeoutOpening = TimeSpan.FromSeconds(TIMEOT_OPENING);
            string serialText = serial.Text;
            while (elapsed < timeoutOpening && string.IsNullOrEmpty(serialText)) {
                serial = _driver.FindElement(By.Id("serial"));
                Thread.Sleep(50);
            }

            string result = serial.Text.Replace("Serial Number : ", "");
            return result.Replace("Cepийный Hoмep: ", "");
        }


        float GetNextCyclic(float f, float min, float max) {
            f++;
            if (f > max)
                f = min;
            return f;
        }

        ushort GetNextCyclic(ushort f, ushort min, ushort max) {
            f++;
            if (f > max)
                f = min;
            return f;
        }

        private void ReadData(string url, SlaveStorage storage) {

            try {
                string readSerial = ReadSerial();
                // убедились что страница с парметрами компрессорами открылась и нормально загрузилась
                string workState = ReadWorkState();
                string flow = ReadFlow();
                string pressure = ReadPressure();
                string temperature = ReadTemperature();
                string time = ReadTime();

                storage.WorkState = ParseWorkState(workState);
                storage.Flow = ParseFlow(flow);
                storage.Pressure = ParsePressure(pressure);
                storage.Temperature = ParseTemperature(temperature);
                ushort storageTime = ParseTime(time);
                storage.Time = storageTime;

                // storage.WorkState = GetNextCyclic(storage.WorkState, 0, 5);
                // storage.Flow = GetNextCyclic(storage.Flow++, 1, 99);
                // storage.Pressure = GetNextCyclic(storage.Pressure++, 0.0f, 10.0f);
                // storage.Temperature = GetNextCyclic(storage.Temperature, 1, 120);
                //
            }
            catch (WebDriverException ex) {
                Console.WriteLine(ex);
                _goodBrowser = false;

                storage.WorkState = 0;
                storage.Flow = 0;
                storage.Pressure = 0.0f;
                storage.Temperature = 0;
            }
        }

        private ushort ParseTime(string time) {
            bool b = DateTime.TryParse(time, out var datetime);
            if (b)
                return (ushort) (datetime.Hour * 1000 + datetime.Minute * 10 + datetime.Second / 10);
            return 0;
        }

        private static IWebDriver GetWebDriver() {
            //ChromeOptions chromeOptions = new ChromeOptions();
            // chromeOptions1.AddArgument("headless");
            // chromeOptions1.AddArgument("no-sandbox");
            //chromeOptions.AddArgument("window-size=1420,1080");
            // chromeOptions.AddArgument("disable-gpu");


            var options = new FirefoxOptions {
                BrowserExecutableLocation = GetLocalFolderPath() + @"\Mozilla Firefox\firefox.exe",
                Profile = new FirefoxProfile(),
                // LogLevel = FirefoxDriverLogLevel.Debug
                LogLevel = FirefoxDriverLogLevel.Default
            };
            options.AddArguments("--disable-gpu");
            // options.AddArguments("--headless");
            IWebDriver driver = new FirefoxDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(IMPLICIT_WAIT);
            return driver;
        }

        private static string GetLocalFolderPath() {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        public static void StaticReadDataThreadAsync(object o) {
            ElektronikonReader reader = (ElektronikonReader) o;
            reader.ReadDataThreadAsync();            
        }
    }
}