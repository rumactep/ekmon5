package orithya.atlascopco.smartlink;

import javax.xml.bind.DatatypeConverter;

public class HexConverter {

	public static String toHexString(byte[] array) {
	    return DatatypeConverter.printHexBinary(array);
	}

	public static byte[] toByteArray(String string) {
	    return DatatypeConverter.parseHexBinary(string);
	}
	
}
