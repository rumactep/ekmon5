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

    public void SetData(int i, string substring) {
        _requests[i].Data = substring;
    }
}