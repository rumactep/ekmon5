package orithya.atlascopco.smartlink;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.TreeMap;

import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.HttpClientBuilder;
import org.apache.http.message.BasicNameValuePair;

import com.google.common.base.Charsets;

public class ElektronikonClient {

	private final String url;
	private final HttpClient client;
	private final HttpPost post;
	
	public ElektronikonClient(String url) {
		this.url = url;
		this.client = HttpClientBuilder.create().build();
		this.post = new HttpPost(this.url);
	}
	
	public Map<DataItem, Optional<ByteBuffer>> ask(ElektronikonRequest request) {
		try {
			List<NameValuePair> urlParameters = new ArrayList<>();
			urlParameters.add(new BasicNameValuePair("QUESTION", request.getRequestString()));
			this.post.setEntity(new UrlEncodedFormEntity(urlParameters));
	
			HttpResponse response = client.execute(post);	
			BufferedReader reader = new BufferedReader(new InputStreamReader(response.getEntity().getContent(), Charsets.US_ASCII));
	
			Map<DataItem, Optional<ByteBuffer>> results = new TreeMap<>();
			int c;
			char[] charBuf;
			int requestIndex = 0;
			reader.mark(1);
			Optional<ByteBuffer> result;
			while ((c = reader.read()) != -1) {
				if (c != 'X') {
					charBuf = new char[8];
					reader.reset();
					reader.read(charBuf);
					String sbuf = new String(charBuf);
					result = Optional.of(ByteBuffer.wrap(HexConverter.toByteArray(sbuf)));
				} else {
					result = Optional.empty();
				}
				results.put(request.getRequests().get(requestIndex), result);
				
				reader.mark(1);
				requestIndex++;
			}
			
			return results;
		} catch (Exception e) {
			e.printStackTrace();
		}
		
		return Collections.emptyMap();
	}
	
}
