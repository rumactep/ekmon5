using System;
using System.Diagnostics;

namespace smartlink;

public class AnswerData {
    public static readonly AnswerData Empty = new(string.Empty);

    public AnswerData(string str) {
        Str = str;
    }

    public string Str { get; }

    public bool IsEmpty => string.IsNullOrEmpty(Str) || Str == "X";

    public override string ToString() {
        return Str;
    }

    public byte Byte(int reverseindex) {
        return ToByte(reverseindex);
    }

    public byte ToByte(int reverseindex) {
        return ToByte(Str, reverseindex);
    }

    public static byte ToByte(string str, int reverseindex) {
        Debug.Assert(str.Length == 8);
        if (str == "X" || string.IsNullOrEmpty(str))
            throw new Exception($"AnswerData.Str='{str}' is invalid");
        //  reversed 3  2  1  0
        //  bytes    01 23 45 67
        //  index    0  2  4  6
        int index = 6 - 2 * reverseindex;
        string s = str.Substring(index, 2);
        return Convert.ToByte(s, 16);
    }

    /*
    this.Int32 = function() {
        if (this.DATA == "X")
            throw new Error();
        var v = parseInt(this.DATA, 16);
        if (v >>> 31)
            v = -2147483648 + (v & 0x7FFFFFFF);
        return v;
    } //*/

    public ushort UInt16(int reverseindex) {
        Debug.Assert(Str.Length == 8);
        if (Str == "X" || string.IsNullOrEmpty(Str))
            throw new Exception($"AnswerData.Str={Str} is invalid");
        int index = 4 - 4 * reverseindex;
        string s = Str.Substring(index, 4);
        return Convert.ToUInt16(s, 16);
    }

    public short Int16(int reverseindex) {
        Debug.Assert(Str.Length == 8);
        if (Str == "X" || string.IsNullOrEmpty(Str))
            throw new Exception($"AnswerData.Str={Str} is invalid");
        int index = 4 - 4 * reverseindex;
        string s = Str.Substring(index, 4);
        return Convert.ToInt16(s, 16);
        //if (v >>> 15)
        //    v = -32768 + (v & 0x00007FFF);
        //return v;
    }
    
    public int Int32() {
        Debug.Assert(Str.Length == 8);
        if (Str == "X" || string.IsNullOrEmpty(Str))
            throw new Exception($"AnswerData.Str={Str} is invalid");
        return Convert.ToInt32(Str, 16);
    }

    public uint UInt32() {
        Debug.Assert(Str.Length == 8);
        if (Str == "X" || string.IsNullOrEmpty(Str))
            throw new Exception($"AnswerData.Str={Str} is invalid");
        return Convert.ToUInt32(Str, 16);
    }
}