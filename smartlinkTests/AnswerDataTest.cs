using smartlink;
using System.Text;
using Xunit;

namespace smartlinkTests;

public class AnswerDataTest {
    [Fact]
    public void TestToByte() {
        AnswerData data = new AnswerData("ab89cd67");
        Assert.Equal(0x67, data.ToByte(0));
        Assert.Equal(0xCD, data.ToByte(1));
        Assert.Equal(0x89, data.ToByte(2));
        Assert.Equal(0xAB, data.ToByte(3));
    }

    [Fact]
    public void TestToUInt16() {
        AnswerData data = new AnswerData("ab89cd67");
        Assert.Equal(0xCD67, data.ToUInt16(0));
        Assert.Equal(0xab89, data.ToUInt16(1));
    }
}