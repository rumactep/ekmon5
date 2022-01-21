using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class ElektronikonClient {

	private readonly string Url;
	private readonly HttpClient client;
	// private readonly HttpPost post;
	
	public ElektronikonClient(string url) {
		Url = url;
		client = new HttpClient();
		//this.post = new HttpPost(this.url);
	}
	
	public async Task<Dictionary<DataItem, byte[]>> Ask(ElektronikonRequest request) {
		/*
		try {
			var urlParameters = new Dictionary<string, string> {
				{ "QUESTION", request.GetRequestString() }
			};

			var parametersContent = new FormUrlEncodedContent(urlParameters);
			var response = await client.PostAsync(Url, parametersContent);

			string answer = await response.Content.ReadAsStringAsync();

			var results = new Dictionary<DataItem, byte[]>();



			int c;
			char[] charBuf;
			int requestIndex = 0;
			//reader.mark(1);
			byte[] result;
			while ((c = reader.Read()) != -1) {
				if (c != 'X') {
					charBuf = new char[8];
					//reader.reset();
					reader.Read(charBuf);
					string sbuf = new string(charBuf);
					result = HexConverter.toByteArray(sbuf);
				} else {
					result = Array.Empty<byte>();
				}
				results.Add(new DataItem(requestIndex, 0), result); 
				requestIndex++;
			}
			
			return results;
		} catch (Exception e) {
			Console.WriteLine(e.Message);
		} //*/
		
		return new Dictionary<DataItem, byte[]>();
	}
	
}
