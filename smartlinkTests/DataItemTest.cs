using smartlink;
using Xunit;

namespace smartlinkTests; 

public class DataItemTest {
    [Fact]
    public void TestToString() {
        var item = new DataItem(8208, 1);
        var str = item.ToString();
        Assert.Equal("201001", str);
    }
}