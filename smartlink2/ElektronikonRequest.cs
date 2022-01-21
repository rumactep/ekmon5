package orithya.atlascopco.smartlink;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class ElektronikonRequest {

	private List<DataItem> requests = new ArrayList<>();
	
	public ElektronikonRequest addQuestion(DataItem dataItem) {
		this.requests.add(dataItem);
		return this;
	}
	
	public List<DataItem> getRequests() {
		return Collections.unmodifiableList(this.requests);
	}
	
	public String getRequestString() {
		StringBuilder sb = new StringBuilder();
		requests.forEach(r -> sb.append(r));
		return sb.toString();
	}
	
}
