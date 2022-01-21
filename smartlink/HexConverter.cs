
public class HexConverter {

	public static string ToHexString(byte[] array) {
		string s = System.Text.Encoding.ASCII.GetString(array, 0, array.Length);
		//string s = System.Text.Encoding.UTF8.GetString(array, 0, array.Length);
		//return DatatypeConverter.printHexBinary(array);
		return s;
	}

	public static byte[] toByteArray(string str) {
		byte[] buffer = System.Text.Encoding.ASCII.GetBytes(str);
		
		//return DatatypeConverter.parseHexBinary(str);
		return buffer;
	}
	
}
