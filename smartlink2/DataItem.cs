package orithya.atlascopco.smartlink;

import java.io.Serializable;

public class DataItem implements Comparable<DataItem>, Serializable {

	private static final long serialVersionUID = -3868942999072733261L;

	private final Integer index;
	private final Integer subIndex;

	public DataItem(Integer index, Integer subIndex) {
		super();
		this.index = index;
		this.subIndex = subIndex;
	}

	public Integer getIndex() {
		return this.index;
	}

	public Integer getSubIndex() {
		return this.subIndex;
	}

	@Override
	public int compareTo(DataItem di) {
		if (this.index.compareTo(di.getIndex()) == 0) {
			return this.subIndex.compareTo(di.getSubIndex());
		}
		
		return this.index.compareTo(di.getIndex());
	}
	
	public String toString() {
		return String.format("%04X%02X", this.index, this.subIndex);
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + ((index == null) ? 0 : index.hashCode());
		result = prime * result + ((subIndex == null) ? 0 : subIndex.hashCode());
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		DataItem other = (DataItem) obj;
		if (index == null) {
			if (other.index != null)
				return false;
		} else if (!index.equals(other.index))
			return false;
		if (subIndex == null) {
			if (other.subIndex != null)
				return false;
		} else if (!subIndex.equals(other.subIndex))
			return false;
		return true;
	}

}
