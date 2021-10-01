using System;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;

namespace ekread {
    public class Ekdata {
        public Ekdata(string serial, string machinestate, string pressure, string temperature, string dewpoint, string worktime, string startcount, string serviceplan1) {
            Serial = serial;
            Machinestate = machinestate;
            Pressure = pressure;
            Temperature = temperature;
            Dewpoint = dewpoint;
            Worktime = worktime;
            Startcount = startcount;
            Serviceplan1 = serviceplan1;
        }

        public override string ToString() {
            return
                $"serial: {Serial}, machinestate: {Machinestate}, pressure: {Pressure}, temperature: {Temperature}, dewpoint: {Dewpoint}, worktime: {Worktime}, startcount: {Startcount}, serveceplan1: {Serviceplan1}";            
        }

        public string Serial { get; set; }
        public string Machinestate { get; set; }
        public string Pressure { get; set; }
        public string Temperature { get; set; }
        public string Dewpoint { get; set; }
        public string Worktime { get; set; }
        public string Startcount { get; set; }
        public string Serviceplan1 { get; set; }
    }
    class Ekreader {
        public const int TIMEOT_OPENING = 70;
        private readonly IWebDriver _driver;
        public Ekreader(IWebDriver driver) {
            _driver = driver;            
        }

        public Ekdata ReadData() {
            string serial = ReadSerial();
            if (string.IsNullOrEmpty(serial)) {
                Console.WriteLine("error: serial is empty, exiting.");
                return null;
            }
            string machinestate = Readmachinestate();
            string pressure = ReadPressure();
            string temperature = ReadDischargeTemperature();
            string dewpoint = ReadDewPoint();
            string worktime = ReadWorktime();
            string startcount = ReadStartcount();
            string serviceplan1 = ReadServiceplan1();
            return new Ekdata(serial, machinestate, pressure, temperature, dewpoint, worktime, startcount, serviceplan1);
        }

        string ReadSerial() {
            IWebElement serial = _driver.FindElement(By.Id("serial"));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.Elapsed < TimeSpan.FromSeconds(TIMEOT_OPENING) && string.IsNullOrEmpty(serial.Text)) 
                serial = _driver.FindElement(By.Id("serial"));
            string result = serial.Text.Replace("Serial Number : ", "");
            return result.Replace("Cepийный Hoмep: ", "");            
        }

        string Readmachinestate() {
            // <td id="MACHINESTATER0C1">Cocтoяниe Maшины</td>
            // <td id = "MACHINESTATER0C2" > Зaгpyзкa </ td >
            IWebElement header = _driver.FindElement(By.Id("MACHINESTATER0C1"));
            if (header.Text == "Cocтoяниe Maшины" || header.Text == "Machine Status") {
                IWebElement value = _driver.FindElement(By.Id("MACHINESTATER0C2"));
                return value.Text;
            }
            else {
                return string.Empty;
            }
        }

        string ReadWorktime() {
            IWebElement header = _driver.FindElement(By.Id("COUNTERSR0C1"));
            if (header.Text != "Чacы Paбoты" && header.Text != "Running Hours")
                return string.Empty;
            IWebElement value = _driver.FindElement(By.Id("COUNTERSR0C2"));
            return value.Text;
        }

        string ReadStartcount() {
            // <td id="COUNTERSR1C1">Чиcлo Пycкoв Moтopa</td>
            IWebElement header = _driver.FindElement(By.Id("COUNTERSR1C1"));
            if (header.Text == "Чиcлo Пycкoв Moтopa" || header.Text == "Motor Starts") {
                IWebElement value = _driver.FindElement(By.Id("COUNTERSR1C2"));
                return value.Text;
            }
            else {
                header = _driver.FindElement(By.Id("COUNTERSR2C1"));
                if (header.Text == "Чиcлo Пycкoв Moтopa" || header.Text == "Motor Starts")
                    return _driver.FindElement(By.Id("COUNTERSR2C2")).Text;
                else 
                    return string.Empty;
            }
        }


        string ReadServiceplan1() {
            // <td id="SERVICEPLANR0C1" style="width: 110px;">4000</td>
            try {
                IWebElement header = _driver.FindElement(By.Id("SERVICEPLANR0C1"));
                if (header.Text.StartsWith("4")) {
                    IWebElement value = _driver.FindElement(By.Id("SERVICEPLANR0C3LEVEL"));
                    return value.Text;
                }
                else {
                    header = _driver.FindElement(By.Id("SERVICEPLANR1C1"));
                    if (header.Text.StartsWith("4")) {
                        IWebElement value = _driver.FindElement(By.Id("SERVICEPLANR1C3LEVEL"));
                        return value.Text;
                    }
                    else {
                        header = _driver.FindElement(By.Id("SERVICEPLANR2C1"));
                        if (header.Text.StartsWith("4")) {
                            IWebElement value = _driver.FindElement(By.Id("SERVICEPLANR2C3LEVEL"));
                            return value.Text;
                        }
                        return "-";
                    }
                }
            }
            catch (NoSuchElementException ex) {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        string ReadPressure() {
            try {
                IWebElement header = _driver.FindElement(By.Id("ANALOGINPUTSR0C1"));
                if (header.Text != "Дaвлeниe Ha Bыxoдe" && header.Text != "Compressor Outlet")
                    return string.Empty;
                IWebElement value = _driver.FindElement(By.Id("ANALOGINPUTSR0C2"));
                return value.Text;
            }
            catch (NoSuchElementException ex) {
                Console.WriteLine(ex.ToString());
                return "-";
            }
        }

        string ReadDischargeTemperature() {
            try {
                IWebElement header = _driver.FindElement(By.Id("ANALOGINPUTSR1C1"));
                if (header.Text != "Bыxoд Cтyпeни" && header.Text != "Element Outlet")
                    return string.Empty;
                IWebElement value = _driver.FindElement(By.Id("ANALOGINPUTSR1C2"));
                return value.Text;
            }
            catch (NoSuchElementException ex) {
                Console.WriteLine(ex);
                return "-";
            }
        }

        string ReadDewPoint() {
            try {
                IWebElement header = _driver.FindElement(By.Id("ANALOGINPUTSR2C1"));
                if (header.Text != "Toчкa Pocы Ocyшитeля" && header.Text != "Dryer PDP")
                    return "-";
                IWebElement value = _driver.FindElement(By.Id("ANALOGINPUTSR2C2"));
                return value.Text;
            }
            catch (NoSuchElementException ex) {
                Console.WriteLine(ex);
                return "-";
            }
        }

        bool WriteSerial(string value) {
            try {
                IWebElement element = _driver.FindElement(By.Id("serial"));
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                while (stopwatch.Elapsed < TimeSpan.FromSeconds(TIMEOT_OPENING) && element == null) { 
                    element = _driver.FindElement(By.Id("serial"));
                    Thread.Sleep(100);
                }
                if (element == null)
                    return false;
                element.Clear();
                element.SendKeys(value);
                Thread.Sleep(500);
                return true;
            }
            catch (NoSuchElementException ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        void WritePressure(string value) {
            IWebElement element = _driver.FindElement(By.Id("pressure"));
            element.Clear();
            element.SendKeys(value);
        }
        void WriteTemperature(string value) {
            IWebElement element = _driver.FindElement(By.Id("temperature"));
            element.Clear();
            element.SendKeys(value);
        }
        void WriteDewpoint(string value) {
            IWebElement element = _driver.FindElement(By.Id("dewpoint"));
            element.Clear();
            element.SendKeys(value);
        }
        void WriteWorktime(string value) {
            IWebElement element = _driver.FindElement(By.Id("worktime"));
            element.Clear();
            element.SendKeys(value);
        }
        void WriteStartcount(string value) {
            IWebElement element = _driver.FindElement(By.Id("startcount"));
            element.Clear();
            element.SendKeys(value);
        }
        void WriteServiceplan1(string value) {
            IWebElement element = _driver.FindElement(By.Id("serviceplan1"));
            element.Clear();
            element.SendKeys(value);
        }
        void WriteSubmit() {
            IWebElement element = _driver.FindElement(By.Id("serviceplan1"));
            element.Submit();
        }

        public bool SaveDataToFrio(Ekdata data) {
            if (!WriteSerial(data.Serial)) {
                Console.WriteLine("error: serial is empty, exiting.");
                return false;
            }
            // Serial Pressure Temperature Dewpoint Worktime Startcount Serviceplan1

            WritePressure(data.Pressure);
            WriteTemperature(data.Temperature);
            WriteDewpoint(data.Dewpoint);
            WriteWorktime(data.Worktime);
            WriteStartcount(data.Startcount);
            WriteServiceplan1(data.Serviceplan1);
            WriteSubmit();
            return true;
        }
    }
}
