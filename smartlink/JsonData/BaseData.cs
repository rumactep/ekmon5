using System;
using System.Collections.Generic;

namespace smartlink.JsonData;

public interface IVisitor {
    void VisitAnalogInputs(IViewCreator creator, List<AnalogInput> list);
    void VisitDigitalInputs(List<DigitalInput> list);
    void VisitCounters(List<Counter> list);
    void VisitConverters(List<Converter> list);
    void VisitDigitalOutputs(List<DigitalOutput> list);
    void VisitCalculatedAnalogInputs(List<CalculatedAnalogInput> list);
    void VisitSpecialProtections(List<SpecialProtection> list);
    void VisitAnalogOutputs(List<AnalogOutput> list);
    void VisitSpms(List<SPM> list);
    void VisitServicePlans(List<ServicePlan> list);
    void VisitDevices(List<ushort> list);
}

public interface IPartWriter {
    void Write(string str);
    void WriteLine(string str);
    string Text { get; }
}

public class StringPartWriter : IPartWriter {
    public string Text { get; private set; } = string.Empty;

    public void Write(string str) {
        Text += str;
    }

    public void WriteLine(string str) {
        Text += str;
        Text += Environment.NewLine;
    }
}

public class VisitorHelper<T> {
    public static void VisitItems(IPartWriter partWriter, List<T> items) {
        if (items.Count == 0)
            return;
        partWriter.WriteLine(items.GetType().ToString());
        foreach (T item in items) 
            partWriter.Write(item!.ToString()!);
        partWriter.WriteLine("");
    }
}

public class StringVisitor : IVisitor {
    private readonly IPartWriter _partWriter;
    private readonly Language _language;
    public string Text => _partWriter.Text;

    public StringVisitor(IPartWriter partWriter, Language language) {
        _partWriter = partWriter;
        _language = language;
    }

    public void VisitAnalogInputs(IViewCreator creator, List<AnalogInput> list) {
        if (list.Count == 0)
            return;
        _partWriter.WriteLine(list.GetType().ToString());
        foreach (AnalogInput item in list)
            _partWriter.Write(creator.CreateView(item, _language).GetString());
            //_partWriter.Write(item!.ToString()!);
        _partWriter.WriteLine("");
    }

    public void VisitDigitalInputs(List<DigitalInput> items) {
        if (items.Count == 0)
            return;
        _partWriter.WriteLine(items.GetType().ToString());
        foreach (var item in items)
            _partWriter.Write(item.ToString() );
        _partWriter.WriteLine("");
    }

    public void VisitCounters(List<Counter> list) {
        VisitorHelper<Counter>.VisitItems(_partWriter, list);
    }

    public void VisitConverters(List<Converter> list) {
        VisitorHelper<Converter>.VisitItems(_partWriter, list);
    }

    public void VisitDigitalOutputs(List<DigitalOutput> list) {
        VisitorHelper<DigitalOutput>.VisitItems(_partWriter, list);
    }

    public void VisitCalculatedAnalogInputs(List<CalculatedAnalogInput> list) {
        VisitorHelper<CalculatedAnalogInput>.VisitItems(_partWriter, list);
    }

    public void VisitSpecialProtections(List<SpecialProtection> list) {
        VisitorHelper<SpecialProtection>.VisitItems(_partWriter, list);
    }

    public void VisitAnalogOutputs(List<AnalogOutput> list) {
        VisitorHelper<AnalogOutput>.VisitItems(_partWriter, list);
    }

    public void VisitSpms(List<SPM> list) {
        VisitorHelper<SPM>.VisitItems(_partWriter, list);
    }

    public void VisitServicePlans(List<ServicePlan> list) {
        VisitorHelper<ServicePlan>.VisitItems(_partWriter, list);
    }

    public void VisitDevices(List<ushort> list) {
        VisitorHelper<ushort>.VisitItems(_partWriter, list);
    }
}



public abstract class BaseData {
    public AnswerData Data { get; set; } = AnswerData.Empty;
    public void setData(AnswerData answerData) {
        Data = answerData;
    }
}