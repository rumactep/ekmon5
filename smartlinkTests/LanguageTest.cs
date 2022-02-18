using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using smartlink;

namespace smartlinkTests {
    public class FormatterHelperTest {
        [Fact]
        public void TestFormatterHelper() {
            short value = 20;
            byte dISPLAYPRECISION = 1;
            string answer = value.ToString("N" + $"{dISPLAYPRECISION}");
            //Assert.Equal
        }
    }
    public class LanguageTest {
        [Fact]
        public void TestLanguage() {
            var l = new Language();
            LanguageLoader loader = new LanguageLoader();
            string item1 = "MPL_7843$$Pacчeтнoe Дaвлeниe B Кoлoннe";
            string item2 = "CHECK_1$$Дaтчики";
            loader.AddString(item1, l);
            loader.AddString(item2, l);
            loader.AddString(item1, l);
            Assert.Equal("Pacчeтнoe Дaвлeниe B Кoлoннe", l.GetString("MPL", 7843));
            Assert.Equal("Дaтчики", l.GetString("CHECK", 1));
        }
        [Fact]
        public void TestLanguageStates() {
            var l = new Language();
            LanguageLoader loader = new LanguageLoader();
            string item3 = "MPL_2100_Oткpыт_Зaкpыт$$Aвapийнaя Ocтaнoвкa";
            loader.AddString(item3, l);
            Assert.Equal("Aвapийнaя Ocтaнoвкa", l.GetString("MPL", 2100));
            var s1 = l.GetString("MPL", 2100, 1);
            Assert.Equal("Oткpыт", s1);
            var s2 = l.GetString("MPL", 2100, 2);
            Assert.Equal("Зaкpыт", s2);
        }
        [Fact]
        public void TestLanguageEmpty() {
            var l = new Language();
            var s1 = l.GetString("MPL", 2100);
            Assert.Equal("type:MPL, key:2100", s1);
            var s2 = l.GetString("MPL", 2100, 2);
            Assert.Equal("type:MPL, key:2100", s2);
        }
        [Fact]
        public void TestLanguageEmpty2() {
            var l = new Language();
            LanguageLoader loader = new LanguageLoader();
            string item1 = "MPL_7843$$Pacчeтнoe Дaвлeниe B Кoлoннe";
            loader.AddString(item1 , l);
            var s1 = l.GetString("MPL", 2100);
            Assert.Equal("key:2100", s1);
            var s2 = l.GetString("MPL", 2100, 2);
            Assert.Equal("key:2100, index:2", s2);
        }
    }

}
