
// implements Comparable<DataItem>, Serializable
public class DataItem {

	//private const long serialVersionUID = -3868942999072733261L;

	public int Index {get; set;}
	public int SubIndex { get; set; }


	public DataItem(int index, int subIndex) {
		Index = index;
		SubIndex = subIndex;
	}

	public int CompareTo(DataItem di) {
		if (Index.CompareTo(di.Index) == 0) 
			return SubIndex.CompareTo(di.SubIndex);
		
		
		return Index.CompareTo(di.Index);
	}
	
	public override string ToString() {
		return string.Format("%04X%02X", Index, SubIndex);
	}

	public override int GetHashCode() {
		const int prime = 31;
		int result = 1;
		result = prime * result + Index.GetHashCode();
		result = prime * result + SubIndex.GetHashCode();
		return result;
	}

	public override bool Equals(object? obj) {
		if ((obj == null) || !GetType().Equals(obj.GetType())) {
			return false;
		}
		else {
			DataItem p = (DataItem)obj;
			return (Index == p.Index) && (SubIndex == p.SubIndex);
		}
	}

}
