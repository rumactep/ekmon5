using System.Collections.Generic;
using System.Text;

namespace smartlink; 

public class ElektronikonRequest {

    private readonly List<DataItem> _requests = new();
    // private readonly OrderedDictionary<DataItem, byte[]> _requests = new();

    public ElektronikonRequest AddQuestion(DataItem dataItem) {
        _requests.Add(dataItem);
        return this;
    }
	
    // public IList<DataItem> GetRequests() {
    //     return _requests.AsReadOnly();
    // }
	
    public string GetRequestString() {
        StringBuilder sb = new StringBuilder();
        _requests.ForEach(r => sb.Append(r));
        return sb.ToString();
    }	
}