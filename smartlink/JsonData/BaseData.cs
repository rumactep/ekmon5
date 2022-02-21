using System;
using System.Collections.Generic;

namespace smartlink.JsonData;

public interface IVisitor {
    void VisitAnalogInputs(IViewCreator creator, List<AnalogInput> list);
    void VisitDigitalInputs(IViewCreator creator, List<DigitalInput> list);
    void VisitCounters(IViewCreator creator, List<Counter> list);
    void VisitConverters(IViewCreator creator, List<Converter> list);
    void VisitDigitalOutputs(IViewCreator creator, List<DigitalOutput> list);
    void VisitCalculatedAnalogInputs(IViewCreator creator, List<CalculatedAnalogInput> list);
    void VisitSpecialProtections(IViewCreator creator, List<SpecialProtection> list);
    void VisitAnalogOutputs(IViewCreator creator, List<AnalogOutput> list);
    void VisitSpms(IViewCreator creator, List<SPM> list);
    void VisitServicePlans(IViewCreator creator, List<ServicePlan> list);
    void VisitDevices(IViewCreator creator, List<ushort> list);
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

/*
public class VisitorHelper<T> {
    public static void VisitItems(IPartWriter partWriter, List<T> items) {
        if (items.Count == 0)
            return;
        partWriter.WriteLine(items.GetType().ToString());
        foreach (T item in items) 
            partWriter.Write(item!.ToString()!);
        partWriter.WriteLine("");
    }
} //*/

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

    public void VisitDigitalInputs(IViewCreator creator, List<DigitalInput> items) {
        if (items.Count == 0)
            return;
        _partWriter.WriteLine(items.GetType().ToString());
        foreach (var item in items)
            _partWriter.Write(creator.CreateView(item, _language).GetString());
        _partWriter.WriteLine("");
    }

    public void VisitCounters(IViewCreator creator, List<Counter> items) {
        if (items.Count == 0)
            return;
        _partWriter.WriteLine(items.GetType().ToString());
        foreach (var item in items)
            _partWriter.Write(creator.CreateView(item, _language).GetString());
        _partWriter.WriteLine("");
    }

    public void VisitConverters(IViewCreator creator, List<Converter> items) {
        if (items.Count == 0)
            return;
        _partWriter.WriteLine(items.GetType().ToString());
        foreach (var item in items)
            _partWriter.Write(creator.CreateView(item, _language).GetString());
        _partWriter.WriteLine("");
    }

    public void VisitDigitalOutputs(IViewCreator creator, List<DigitalOutput> items) {
        if (items.Count == 0)
            return;
        _partWriter.WriteLine(items.GetType().ToString());
        foreach (var item in items)
            _partWriter.Write(creator.CreateView(item, _language).GetString());
        _partWriter.WriteLine("");
    }

    public void VisitCalculatedAnalogInputs(IViewCreator creator, List<CalculatedAnalogInput> items) {
        if (items.Count == 0)
            return;
        _partWriter.WriteLine(items.GetType().ToString());
        foreach (var item in items)
            _partWriter.Write(creator.CreateView(item, _language).GetString());
        _partWriter.WriteLine("");
    }

    public void VisitSpecialProtections(IViewCreator creator, List<SpecialProtection> items) {
        if (items.Count == 0)
            return;
        _partWriter.WriteLine(items.GetType().ToString());
        foreach (var item in items)
            _partWriter.Write(creator.CreateView(item, _language).GetString());
        _partWriter.WriteLine("");
    }

    public void VisitAnalogOutputs(IViewCreator creator, List<AnalogOutput> items) {
        if (items.Count == 0)
            return;
        _partWriter.WriteLine(items.GetType().ToString());
        foreach (var item in items)
            _partWriter.Write(creator.CreateView(item, _language).GetString());
        _partWriter.WriteLine("");
    }

    public void VisitSpms(IViewCreator creator, List<SPM> items) {
        if (items.Count == 0)
            return;
        _partWriter.WriteLine(items.GetType().ToString());
        foreach (var item in items)
            _partWriter.Write(creator.CreateView(item, _language).GetString());
        _partWriter.WriteLine("");
    }

    public void VisitServicePlans(IViewCreator creator, List<ServicePlan> items) {
        if (items.Count == 0)
            return;
        _partWriter.WriteLine(items.GetType().ToString());
        foreach (var item in items)
            _partWriter.Write(creator.CreateView(item, _language).GetString());
        _partWriter.WriteLine("");
    }

    public void VisitDevices(IViewCreator creator, List<ushort> items) {
        if (items.Count == 0)
            return;
        _partWriter.WriteLine(items.GetType().ToString());
        foreach (var item in items)
            _partWriter.Write(creator.CreateView(item, _language).GetString());
        _partWriter.WriteLine("");
    }
}



public abstract class BaseData {
    public AnswerData Data { get; set; } = AnswerData.Empty;
    public void setData(AnswerData answerData) {
        Data = answerData;
    }
}