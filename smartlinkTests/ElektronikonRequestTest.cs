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
        request.AddQuestion(new DataItem(33, 44));
        request.AddQuestion(new DataItem(5555, 66));
        string requestString = request.GetRequestString();
        Assert.Equal("000102003344555566", requestString);
    }
}