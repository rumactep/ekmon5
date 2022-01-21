using System.Collections.Generic;
using System.Text;

public class ElektronikonRequest {

	private List<DataItem> _requests = new List<DataItem>();
	
	public ElektronikonRequest addQuestion(DataItem dataItem) {
		_requests.Add(dataItem);
		return this;
	}
	
	public IList<DataItem> GetRequests() {
		return _requests.AsReadOnly();
	}
	
	public string GetRequestString() {
		StringBuilder sb = new StringBuilder();
		_requests.ForEach(r => sb.Append(r));
		return sb.ToString();
	}	
}
