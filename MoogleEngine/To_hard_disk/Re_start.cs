namespace MoogleEngine;
public static class Re_Start
{
    // Upss en obras 
    // Espero que en la sgt actualizacion hayan muchos metodos jjj
    public static HashSet<string>Root_words=new HashSet<string>{};

 private static void Root_Word()
{
   Dictionary<string, List<string>>dicc= Auxiliar_Class.Texts_Words;
   List<string>doc_names=Auxiliar_Class.Documents_names;

    for (int i = 0; i < doc_names.Count; i++)
    {
        string doc=doc_names[i];

        HashSet<string>root=dicc[doc].ToHashSet<string>();

        Root_words.UnionWith(root);
    }



}



public static void Start()
{

    Root_Word();

}




}