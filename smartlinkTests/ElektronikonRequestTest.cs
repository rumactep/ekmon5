using System;
using System.Collections.Generic;
using System.Linq;
using smartlink;
using Xunit;

namespace smartlinkTests; 

public class ElektronikonRequestTest {
    [Fact]
    public void TestElektronikonRequest() {
        ElektronikonRequest request = new ElektronikonRequest();
        request.AddQuestion(new DataItem(1, 2));
        request.AddQuestion(new DataItem(0x33, 0x44));
        request.AddQuestion(new DataItem(0x5555, 0x66));
        string requestString = request.GetRequestString();
        Assert.Equal("00010200212c15b342", requestString);
    }
    
    IEnumerable<string> GetOrder() {
        yield return "1";
        yield return "2";
        yield return "3";
        yield return "4";
        yield return "5";
    }
    [Fact]
    public void TestDictionaryOrder() {
        Dictionary<DataItem, string> dict = new Dictionary<DataItem, string>();
        dict.Add(new DataItem(11, 22), "2");
        dict.Add(new DataItem(1, 2), "1");
        dict.Add(new DataItem(1111, 2222), "4");
        dict.Add(new DataItem(22, 11), "3");

        Dictionary<DataItem, string>.Enumerator it1 = dict.GetEnumerator();
        var it2 = GetOrder().GetEnumerator();

        while (it1.MoveNext() && it2.MoveNext()) {
            Assert.Equal(it1.Current.Value, it2.Current);
        }
    }
}