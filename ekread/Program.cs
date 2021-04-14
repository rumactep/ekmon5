using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace ekread {
    public class IpValidator {
        public static void MakeHttpIfNeed(ref string arg) {
            if (!arg.StartsWith("http://") && arg.StartsWith("https://"))
                arg = "http://" + arg;
        }
        public static bool IsValidIp(string arg) {

            Regex ip1 = new Regex(@"\b\d{1,3}\.\b");
            MatchCollection matches1 = ip1.Matches(arg);

            Regex ip4 = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            MatchCollection matches4 = ip4.Matches(arg);

            return matches1.Count == 3 && matches4.Count == 1;
        }
        public static bool IsCommented(string arg) {
            return arg.StartsWith("#") || arg.StartsWith(";") || arg.StartsWith("//");
        }

    }
    public class Program {

        static void Main(string[] args) {
            const string settingsFilename = "ekreadips.txt";
            List<string> ips = new List<string>();
            if (args.Length == 0) {
                // берем список ip из файла ekreadips.txt
                ips = ReadIpsFromFile(settingsFilename);
                ips.RemoveAll(IpValidator.IsCommented);
                ips.RemoveAll(li => !IpValidator.IsValidIp(li));
                Console.WriteLine("прочитано {0} ip из файла настроек", ips.Count);
                if (ips.Count == 0)
                    return;
            }
            else if (args.Length == 1) {
                string arg = args[0];
                IpValidator.MakeHttpIfNeed(ref arg);
                if (IpValidator.IsValidIp(arg)) {
                    IpValidator.MakeHttpIfNeed(ref arg);
                    ips.Add(arg);
                }
                else {
                    Console.WriteLine("Использование:  ekread <ip>    ");
                    return;
                }
            }
            else {
                // error
                Console.WriteLine("Программа ekread предназначена для чтения данных мониторинга воздушных компрессоров типа Elektronikon (ALUP и Atlas Copco)");
                Console.WriteLine("Использование:  ekread ip    ");
                Console.WriteLine("если ip не указан, список ip берется из файла {0}", settingsFilename);
                return;
            }

            IWebDriver driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Ekreader.TIMEOT_OPENING/8.0);

            foreach (string url in ips) {
                ProcessIp(driver, url);
            }
            Thread.Sleep(2000);

#if DEBUG2
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
#endif
            
            driver.Quit();
        }

        static void ProcessIp(IWebDriver driver, string url) {
            var frioUrl = "http://192.168.11.9/maint/frio/frio_order_air.php";
            Thread.Sleep(500);
            try {
                Console.WriteLine("");
                Console.WriteLine("======= Чтение: {0}", url);
                driver.Navigate().GoToUrl(url);
            }
            catch (Exception ex) {
                Console.WriteLine("======= Ошибка соединения с ip {0}", url);
                Console.WriteLine(ex.ToString());
                return;
            }
            Thread.Sleep(500);
            Ekreader reader = new Ekreader(driver);
            Ekdata data = reader.ReadData();
            if (data == null) {
                Console.WriteLine("Error: Не удалось прочитать данные с сервера {0}", url);
                return;
            }
            Console.WriteLine("======= Успешно прочитан {0}, получены данные: {1}", url, data);
            Thread.Sleep(500);
            try {
                driver.Navigate().GoToUrl(frioUrl);
            }
            catch (Exception ex) {
                Console.WriteLine("======= Ошибка соединения с ip {0}", url);
                Console.WriteLine(ex.ToString());
            }
            Thread.Sleep(500);

            if (!reader.SaveDataToFrio(data)) {
                Console.WriteLine("======= Error: Не удалось записать данные в сервер {0}", frioUrl);
                return;
            }
            Thread.Sleep(500);
            Console.WriteLine("======= Записано из {0} в {1}", url, frioUrl);
            Thread.Sleep(500);
        }


        private static List<string> ReadIpsFromFile(string settingsFilename) {
            return new List<string>(File.ReadLines(settingsFilename));
            /*http://192.168.11.207/", "http://192.168.11.208/","http://192.168.11.209/", "http://192.168.11.210/", "http://192.168.11.211/", "http://192.168.11.212/", "http://192.168.11.221/" */
        }
    }
}
