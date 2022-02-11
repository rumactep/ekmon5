using System;
using System.Threading.Tasks;
using smartlink;
using smartlink.JsonData;
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
        var items = ElektronikonRequest.ConfigQuestions;
        string questionString = ElektronikonRequest.GetRequestString(items);
        Assert.Equal(ElektronikonClientStub.QuestionFull, questionString);
    }

    /*
    [Fact] public void TestGetRequestStringData() {
        var json = ElektronikonRequest.ProcessConfig();
        var dataQuestions = ElektronikonRequest.DataQuestions(json);
    } //*/

    [Fact]
    public async void TestSendReceiveSettings() {
        //var reader = new QuestionReader();
        var client = new ElektronikonClientStub();
        var items = ElektronikonRequest.ConfigQuestions;
        var config = await QuestionReader.SendReceive(items, client, NoLogger.Instance);
        Assert.Equal(1, client.Ask1);
        Assert.Equal(1, client.Ask2);
        Assert.Equal(0, client.AskOther);
    }
    
    [Fact]
    public async void TestSendReceiveDatas() {
        //var reader = new QuestionReader();
        var client = new ElektronikonClientStub();
        var items = ElektronikonRequest.ConfigQuestions;
        var config = await QuestionReader.SendReceive(items, client, NoLogger.Instance);
        Assert.Equal(1, client.Ask1);
        Assert.Equal(1, client.Ask2);
        Assert.Equal(0, client.AskOther);
        ElektronikonRequest sparsedConfig = config.SparseQuestions();
        JSONS json = ElektronikonRequest.ProcessConfig(sparsedConfig);
        var dataQuestions = ElektronikonRequest.DataQuestions(json);
        var datas = await QuestionReader.SendReceive(dataQuestions, client, NoLogger.Instance);
        Assert.Equal(1, client.Ask1);
        Assert.Equal(1, client.Ask2);
        Assert.Equal(0, client.AskOther);
        ElektronikonRequest.ProcessData(datas, json);
#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.
        Assert.Equal(2, json.ANALOGINPUTS.Count);
        Assert.Equal(0, json.ANALOGOUTPUTS.Count);
        Assert.Equal(0, json.CALCULATEDANALOGINPUTS.Count);
        Assert.Equal(1, json.CONVERTERS.Count);
        Assert.Equal(12, json.COUNTERS.Count);
        Assert.Equal(1, json.DEVICE.Count);
        Assert.Equal(4, json.DIGITALINPUTS.Count);
        Assert.Equal(6, json.DIGITALOUTPUTS.Count);
        // ES not realised yet
        Assert.Equal(7, json.SERVICEPLAN.Count);
        Assert.Equal(3, json.SPECIALPROTECTIONS.Count);
        Assert.Equal(0, json.SPM2.Count);
#pragma warning restore xUnit2013 // Do not use equality check to check for collection size.

    }
}