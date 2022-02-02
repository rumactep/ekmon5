using System;
using System.Threading.Tasks;
using smartlink;
using Xunit;
using Xunit.Abstractions;

namespace smartlinkTests;

public class ReadAnalogInputsTestIElektronikonClient {
    private readonly ITestOutputHelper _testOutputHelper;

    public ReadAnalogInputsTestIElektronikonClient(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestGetRequestString4() {
        var items = ElektronikonRequest.SettingsQuestions;
        string questionString = ElektronikonRequest.GetRequestString(items);
        Assert.Equal(LocalElektronikonClient.QuestionFull, questionString);
    }

    [Fact]
    public async void TestSendReceive() {
        //var reader = new QuestionReader();
        var client = new LocalElektronikonClient();
        var items = ElektronikonRequest.SettingsQuestions;
        var request = await QuestionReader.ReadSettings(items, client);
        Assert.Equal(1, client.Ask1);
        Assert.Equal(1, client.Ask2);
        Assert.Equal(0, client.AskOther);
        _testOutputHelper.WriteLine(request.GetFullString());
    }
}