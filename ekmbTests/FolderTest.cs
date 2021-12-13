using System;
using NUnit.Framework;

namespace ekmbTests {
    public class Tests {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void TestLocal() {
            Console.WriteLine();
            var path = GetLocalFolderPath();
            Assert.AreEqual(@"C:\Users\vivanov\AppData\Local", path);
        }

        private static string GetLocalFolderPath() {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        [Test]
        public void TestTime() {
            string s = "15:57:36 3/12/2021";
            int i = ParseTime(s);
            Assert.AreEqual(15573, i);
        }

        private int ParseTime(string s) {
            bool b = DateTime.TryParse(s, out var datetime);
            if (b)
                return datetime.Hour * 1000 + datetime.Minute * 10 + datetime.Second / 10;
            return 0;
        }
    }
}