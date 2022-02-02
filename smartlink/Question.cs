using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace smartlink;

public class DataItemComparer : IComparer<Question> {
    public int Compare(Question? x, Question? y) {
        if (x == null || y == null)
            return 0;
        return (x.Index.CompareTo(y.Index) == 0) ? x.Subindex.CompareTo(y.Subindex) : x.Index.CompareTo(y.Index);
    }
}

public class HexConverter {
    public static string ToHexString(byte[] array) {

        string s = System.Text.Encoding.ASCII.GetString(array, 0, array.Length);
        //string s = System.Text.Encoding.UTF8.GetString(array, 0, array.Length);
        return s;
    }

    public static byte[] ToByteArray(string str) {
        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(str);
        return buffer;
    }
}

public class AnswerData {
    public static AnswerData Empty = new AnswerData(string.Empty);
    public string Str { get; set; }
    public AnswerData(string str) {
        Str = str; 
    }

    public override string ToString() => Str; 

    public bool IsEmpty { get { return string.IsNullOrEmpty(Str) || Str == "X";} }

    public byte ToByte(int reverseindex) {
        return ToByte(Str, reverseindex);
    }
    public static byte ToByte(string str, int reverseindex) {
        Debug.Assert(str.Length == 8);
        if (str == "X" || string.IsNullOrEmpty(str))
            throw new Exception($"AnswerData.Str={str} is invalid");
        //  reversed 3  2  1  0
        //  bytes    01 23 45 67
        //  index    0  2  4  6
        var index = 6 - 2 * reverseindex;
        var s = str.Substring(index, 2);
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

    public ushort ToUInt16(int reverseindex) {
        return ToUInt16(Str, reverseindex);
    }

    public static ushort ToUInt16(string str, int reverseindex) {
        Debug.Assert(str.Length == 8);
        if (str == "X" || string.IsNullOrEmpty(str))
            throw new Exception($"AnswerData.Str={str} is invalid");
        var index = 4 - 4 * reverseindex;
        string s = str.Substring(index, 4);
        return Convert.ToUInt16(s, 16);
    }
}

public class Question {
    public int Index {get; }
    public int Subindex { get; }
    public AnswerData Data { get; set; } = AnswerData.Empty;

    public Question(int index, int subIndex) {
        Index = index;
        Subindex = subIndex;
    }

    public Question(int index, int subIndex, string data) {
        Index = index;
        Subindex = subIndex;
        Data = new AnswerData(data);
    }

    public int CompareTo2(object? dio) {
        Question? di = dio as Question;
        return Index.CompareTo(di?.Index) == 0 ? Subindex.CompareTo(di?.Subindex) : Index.CompareTo(di?.Index);
    }
	
    public string Key =>
        // выводим в 16-ричном виде
        $"{Index:x4}{Subindex:x2}";

    public override string ToString() {
        // выводим в 16-ричном виде
        return $"{Index:x4}{Subindex:x2}:{Data}";
    }

    public override int GetHashCode() {
        const int prime = 31;
        int result = 1;
        result = prime * result + Index.GetHashCode();
        result = prime * result + Subindex.GetHashCode();
        return result;
    }

    public override bool Equals(object? obj) {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Question p = (Question)obj;
        return Index == p.Index && Subindex == p.Subindex;
    }

}