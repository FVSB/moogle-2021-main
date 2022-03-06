namespace MoogleEngine;
public static class Sort
{

    #region Campos
    
    public static Dictionary<string, List<string>> Where_Is_the_Word = new Dictionary<string, List<string>> { };
                  //       root word    doc
    public static Dictionary<(string, string), int> Words_Count = new Dictionary<(string, string), int> { };
    //palabras raices con la cant de repeticiones
   
    public static Dictionary<string, List<string>> Words_Text = new Dictionary<string, List<string>> { };
                        //  doc      root word
   #endregion



    #region  Clasificar 
   private static void Words_Clasification(string word, string document)

    {
        word = Words.GetWord(word, document);// Devuelve palabra raiz
        Add_Items(word, document);
       
        List<string> Keys =Auxiliar_Class.Documents_names;// nombre doc

        int h = Keys.IndexOf(document);

        if (!Where_Is_the_Word.ContainsKey(word))  //Para conocer en cuantos elementos esta repetidos
        {

           List<string>y=new List<string>{document};

           Where_Is_the_Word.TryAdd(word,y);

        }
        else
        {
           Where_Is_the_Word[word].Add(document);
        }

        if ((!Words_Count.ContainsKey((document, word))) && word != " ")
        {

            int x = 1;

            Words_Count.Add((document, word), x);
        }
        else
        {
            Words_Count[(document, word)]++;
        }







    }
    #endregion

   private  static void Add_Items(string word, string Document)
    {
        if (!Words_Text.ContainsKey(Document))  //añade al dicc doc --> List palabras
        {
            List<string> a = new List<string> { };
            a.Add(word);
            Words_Text.Add(Document, a);

        }

        else
        {
            Words_Text[Document].Add(word);
        }




    }
   




    private static void Start(List<string> words, string document) 
    {

        for (int i = 0; i < words.Count; i++)
        {
            Words_Clasification(words[i], document);
        }

    }



    #region  Start
     //Metodo portal
    public static void Start()
    {
        Dictionary<string, List<string>> words = Auxiliar_Class.Original_Words;
        List<string> keys = Auxiliar_Class.Documents_names;
        for (int i = 0; i < Auxiliar_Class.Documents_Count; i++)
        {
            string k = keys[i];

            List<string> x = words[k];

            Start(x, k);

        }

       

    }
    #endregion



}