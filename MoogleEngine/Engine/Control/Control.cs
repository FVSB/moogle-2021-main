namespace MoogleEngine;
public static class Control_Class
{
    #region Campo


    public static List<string> Documents_names = new List<string> { };

    //Nombre originales de los documentos
    public static HashSet<string> documents_original_names = new HashSet<string> { };

    public static Dictionary<string, int> doc_name_repe = new Dictionary<string, int> { };


    //guarda el hash de cada documento pero se tiene que generar cada vez que se abra el programa
    private static Dictionary<string, int> hash = new Dictionary<string, int> { };

    //el mismo texto pero diferente titulo

    public static Dictionary<string, List<string>> doc_coincidences = new Dictionary<string, List<string>> { };

    #endregion
    //En esta clase se comprueba que no existan coincidencias con los titulos
    #region Control Texts


    public static (string, bool) Names_Coincidances(string document_name, string word)
    {
        if (String.IsNullOrEmpty(word)) //doc vacio
        {
            return (null!, false);
        }

        string document = Documents_Names.Documents_Name(document_name);
        char[] c = { '\n', '\r' };

        string y = word.Trim(c);

        int words_hash = y.GetHashCode();
        //las listas solas no generan el mismo hashcode
        if (hash.ContainsKey(document))
        {
            int a = hash[document];

            if (words_hash == a)
            {

                return (null!, false); //retornar null y false pq son el mismp documento y el mismo nombre

            }

            if (!doc_name_repe.ContainsKey(document)) //verificar nombres iguales contenido distinto
            {
                doc_name_repe.Add(document, 1);
            }
            if (doc_name_repe.ContainsKey(document))
            {
                doc_name_repe[document]++;
            }
            int repe = doc_name_repe[document];

            string b = document + repe.ToString();
            Add_Items(b);
            Add_Items(b, words_hash);
            return (b, false); // indica que se cambio el nombre del doc
        }

        if (hash.ContainsValue(words_hash))  //diferentes nombres igual texto
        {
            var myKey = hash.FirstOrDefault(x => x.Value == words_hash).Key;
            string x = myKey.ToString();
            if (!doc_coincidences.ContainsKey(x)) //agregarlo al dicc de igual texto !=nombre
            {
                List<string> a = new List<string> { };//para guardar las coincidencias
                a.Add(document);

                doc_coincidences.Add(x, a);

            }

            if (doc_coincidences.ContainsKey(x))
            {
                doc_coincidences[x].Add(document);
            }

            return (null!, false);
        }
        Add_Items(document);
        Add_Items(document, words_hash);
        return (document, true);  //el doc es unico y es no hay copias

    }



    #endregion

    #region Add Items
    private static void Add_Items(string document)
    {
        if (!Documents_names.Contains(document))
        {
            Documents_names.Add(document);
        }


    }

    private static void Add_Items(string document, int hashcode)
    {
        if (!hash.ContainsKey(document))
        {
            hash.Add(document, hashcode);
        }

    }
    #endregion






}


