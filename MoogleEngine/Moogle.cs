namespace MoogleEngine;
public static class Moogle
{
    public static SearchResult Query(string query) {
        (List<SearchItem>,string)tupple=Search_Class.Search(query);
     SearchItem[]items=tupple.Item1.ToArray();
            string Suggestion=tupple.Item2;
        return new SearchResult(items,Suggestion) ;
    }
}
