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
        request.Add(new DataItem(1, 2));
        request.Add(new DataItem(0x33, 0x44));
        request.Add(new DataItem(0x5555, 0x66));
        string requestString = request.GetRequestString();
        Assert.Equal("000102003344555566", requestString);
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
        var dict = new SortedDictionary<DataItem, string>(new DataItemComparer()) {
            { new DataItem(0x11, 0x22), "2" },
            { new DataItem(0x1, 0x2), "1" },
            { new DataItem(0x1111, 0x2222), "4" },
            { new DataItem(0x22, 0x11), "3" }
        };

        using var it1 = dict.GetEnumerator();
        using var it2 = GetOrder().GetEnumerator();

        while (it1.MoveNext() && it2.MoveNext()) {
            Assert.Equal(it1.Current.Value, it2.Current);
            //Console.WriteLine($"{it1.Current.Value}, {it2.Current}");
        }
    }
}