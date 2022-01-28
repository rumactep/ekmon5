using System;
using System.Collections.Generic;

namespace smartlink;

public class KeyComparer : IComparer<DataItem> {
    public int Compare(DataItem? x, DataItem? y) {
        if (x == null || y == null)
            return 0;
        return (x.Index.CompareTo(y.Index) == 0) ? x.SubIndex.CompareTo(y.SubIndex) : x.Index.CompareTo(y.Index);
    }
}

public class DataItem {

    //private const long serialVersionUID = -3868942999072733261L;

    public int Index {get; }
    public int SubIndex { get; }
    
    public DataItem(int index, int subIndex) {
        Index = index;
        SubIndex = subIndex;
    }

    public int CompareTo2(object? dio) {
        DataItem? di = dio as DataItem;
        return Index.CompareTo(di?.Index) == 0 ? SubIndex.CompareTo(di?.SubIndex) : Index.CompareTo(di?.Index);
    }
	
    public override string ToString() {
        // выводим в 16-ричном виде
        return $"{Index:x4}{SubIndex:x2}";
    }

    public override int GetHashCode() {
        const int prime = 31;
        int result = 1;
        result = prime * result + Index.GetHashCode();
        result = prime * result + SubIndex.GetHashCode();
        return result;
    }

    public override bool Equals(object? obj) {
        if (obj == null || GetType() != obj.GetType())
            return false;

        DataItem p = (DataItem)obj;
        return Index == p.Index && SubIndex == p.SubIndex;
    }

}