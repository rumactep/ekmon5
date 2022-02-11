using System.Collections.Generic;

namespace smartlink;

public class DataItemComparer : IComparer<Question> {
    public int Compare(Question? x, Question? y) {
        if (x == null || y == null)
            return 0;
        return x.Index.CompareTo(y.Index) == 0
            ? x.Subindex.CompareTo(y.Subindex)
            : x.Index.CompareTo(y.Index);
    }
}

public class Question {
    public Question(int index, int subIndex) {
        Index = index;
        Subindex = subIndex;
    }

    public Question(int index, int subIndex, string data) {
        Index = index;
        Subindex = subIndex;
        Data = new AnswerData(data);
    }

    public int Index { get; }
    public int Subindex { get; }
    public AnswerData Data { get; set; } = AnswerData.Empty;

    public string Key =>
        // выводим в 16-ричном виде
        $"{Index:x4}{Subindex:x2}";

    public int CompareTo2(object? dio) {
        var di = dio as Question;
        return Index.CompareTo(di?.Index) == 0
            ? Subindex.CompareTo(di?.Subindex)
            : Index.CompareTo(di?.Index);
    }

    public override string ToString() {
        // выводим в 16-ричном виде
        return $"{Index:x4}{Subindex:x2}:{Data}";
    }

    public override int GetHashCode() {
        const int prime = 31;
        var result = 1;
        result = prime * result + Index.GetHashCode();
        result = prime * result + Subindex.GetHashCode();
        return result;
    }

    public override bool Equals(object? obj) {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var p = (Question)obj;
        return Index == p.Index && Subindex == p.Subindex;
    }
}