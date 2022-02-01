using System;
using System.Collections.Generic;
using System.Text;

namespace smartlink;

public class ElektronikonRequest {

    //private readonly SortedDictionary<DataItem, string> _requests = new(new DataItemComparer());
    private readonly List<DataItem> _requests = new();
    public int Length => _requests.Count;

    public void Add(DataItem dataItem) {
        _requests.Add(dataItem);
    }

    public void Add(int index, int subindex) {
        _requests.Add(new DataItem(index, subindex));
    }
    
    public static string GetRequestString(DataItem[] questions) {
        StringBuilder sb = new StringBuilder();
        for (int idx = 0; idx < questions.Length; idx += 1)
            sb.Append(questions[idx].Key);
        return sb.ToString();
    }

    public static string GetRequestString(DataItem[] questions, int from, int to) {
        StringBuilder sb = new StringBuilder();
        for (int idx = from; idx < to; idx += 1)
            sb.Append(questions[idx].Key);
        return sb.ToString();
    }

    // public IList<DataItem> GetRequests() {
    //     return _requests.AsReadOnly();
    // }

    public string GetRequestString() {
        return GetRequestString(0, Length);
    }

    public string GetRequestString(int from, int to) {
        StringBuilder sb = new StringBuilder();
        for (int idx = from; idx < to; idx += 1) 
            sb.Append(_requests[idx].Key);
        return sb.ToString();
    }
    public string GetDataString() {
        StringBuilder sb = new StringBuilder();
        foreach (var item in _requests)
            if (item.Data != "X")
                sb.Append(item + " ");
        return sb.ToString();
    }

    public string GetFullString() {
        StringBuilder sb = new StringBuilder();
        foreach (var item in _requests)
            sb.Append(item + " ");
        return sb.ToString();
    }

    public void SetData(int i, string substring) {
        _requests[i].Data = substring;
    }

    public static DataItem[] SettingsQuestions {
        get {
            ElektronikonRequest er = new ElektronikonRequest();
            Q_2000_AI(er);
            Q_2000_DI(er);
            Q_2000_CNT(er);
            Q_2000_CNV(er); // 1st
            Q_2000_DO(er);
            Q_2000_CAI(er);

            Q_2000_CNV(er); // 2nd for some reason !

            Q_2000_SPR(er);
            Q_2000_AO(er);
            Q_2000_SPM(er);
            Q_3000_ES(er);
            Q_2000_SPL(er);
            Q_2000_MMT(er);
            return er._requests.ToArray();
        }
    }

    private static void Q_2000_AI(ElektronikonRequest er) {
        for (var i = 0x2010; i < 0x2090; i++) {
            er.Add(i, 1);
            er.Add(i, 4);
        }
    }

    private static void Q_2000_DI(ElektronikonRequest er) {
        for (var i = 0x20b0; i < 0x2100; i++)
            er.Add(i, 1);
    }

    private static void Q_2000_CNT(ElektronikonRequest er) {
        for (var i = 1; i < 256; i++)
            er.Add(0x2607, i);
    }

    private static void Q_2000_CNV(ElektronikonRequest er) {
        for (var i = 0x2681; i < 0x2689; i++) {
            er.Add(i, 1);
            er.Add(i, 7);
        }
    }

    private static void Q_2000_DO(ElektronikonRequest er) {
        for (var i = 0x2100; i < 0x2150; i++)
            er.Add(i, 1);
    }

    private static void Q_2000_CAI(ElektronikonRequest er) {
        for (var i = 0x2090; i < 0x20b0; i++) {
            er.Add(i, 1);
            er.Add(i, 3);
        }
    }

    private static void Q_2000_SPR(ElektronikonRequest er) {
        for (var i = 0x2300; i < 0x247F; i++)
            er.Add(i, 1);
    }

    private static void Q_2000_AO(ElektronikonRequest er) {
        for (var i = 0x2150; i < 0x2170; i++) {
            er.Add(i, 1);
            er.Add(i, 3);
        }
    }

    private static void Q_2000_SPM(ElektronikonRequest er) {
        for (var i = 0x2560; i < 0x2570; i++)
            er.Add(i, 1);
    }

    private static void Q_3000_ES(ElektronikonRequest er) {
        er.Add(0x3113, 1);
        for (var i = 3; i <= 5; i++)
            er.Add(0x3113, i);
        for (var i = 7; i <= 30; i++)
            er.Add(0x3113, i);
        for (var i = 31; i <= 42; i++)
            er.Add(0x3113, i);
        for (var i = 1; i <= 18; i++)
            er.Add(0x3114, i);
    }

    private static void Q_2000_SPL(ElektronikonRequest er) {
        for (var i = 1; i < 21; i++)
            er.Add(0x2602, i);
    }

    private static void Q_2000_MMT(ElektronikonRequest er) {
        er.Add(0x2001, 1);
    }
}