

using System;
using System.Collections.Generic;

public class ReadAnalogInputs {

	public static void main() {
		new ReadAnalogInputs().run();
	}
	
	public async void run() {
		// 192.168.11.208/cgi-bin/mkv.cgi
		ElektronikonClient ec = new ElektronikonClient("192.168.100.100/cgi-bin/mkv.cgi");
		
		ElektronikonRequest er = new ElektronikonRequest();
		
	    for (int i = 0x2010; i < 0x2090; i++) {
	    	er.addQuestion(new DataItem(i, 1));
	    	er.addQuestion(new DataItem(i, 4));
	    	er.addQuestion(new DataItem(i, 6));
	    }

		Dictionary<DataItem, byte[]> items = await ec.Ask(er);

		
		foreach (var item in items) {
			Console.WriteLine($"{item.Key} -> {item.Value}"); 
		}

	}
	
}
