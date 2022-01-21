package orithya.atlascopco.smartlink;

import java.nio.ByteBuffer;
import java.util.Map;
import java.util.Optional;

public class ReadAnalogInputs {

	public static void main(String... arguments) throws Exception {
		new ReadAnalogInputs().run();
	}
	
	public void run() throws Exception {
		ElektronikonClient ec = new ElektronikonClient("http://192.168.100.100/cgi-bin/mkv.cgi");
		ElektronikonRequest er = new ElektronikonRequest();
		
	    for(int i = 0x2010; i < 0x2090; i++)
	    {
	    	er.addQuestion(new DataItem(i, 1));
	    	er.addQuestion(new DataItem(i, 4));
	    	er.addQuestion(new DataItem(i, 6));
	    }

	    Map<DataItem, Optional<ByteBuffer>> s = ec.ask(er);
		s.entrySet().forEach(es -> System.out.println(String.format("%s -> %s", es.getKey(), es.getValue())));
	}
	
}
