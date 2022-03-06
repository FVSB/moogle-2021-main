namespace MoogleEngine;

public class SearchItem
{
    public SearchItem(string title, string snippet, float score)
    {
        this.Title = title;
        this.Snippet = snippet;
        this.Score = score;
    }

    public string Title { get; private set; }

    public string Snippet { get; private set; }

    public float Score { get; set; }

     public override bool Equals(object obj)
    {
        if (obj == null) return false;
        SearchItem objAsPart = obj as SearchItem;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public override int GetHashCode()
    {
        return Score.GetHashCode();
    }
    public bool Equals(SearchItem other)
    {
        if (other == null) return false;
        return (this.Score.Equals(other.Score));
    }
}


public class SearchItemComparer : IComparer<SearchItem>
{
    public int Compare(SearchItem x, SearchItem y)
    {
        
        if (x.Score < y.Score) 
            return 1;

        if (x.Score > y.Score) 
            return -1;

        return 0;
    }
}
