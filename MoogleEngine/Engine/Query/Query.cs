
namespace MoogleEngine;
public class Query_Class
{

    #region Campo

    private static List<Up_Score> Up_score_operator = Operators.Up_score;
    private static Dictionary<string, float> Leves_words_sc = new Dictionary<string, float> { };

    private static Dictionary<string, List<L_Words>> Levens_words = new Dictionary<string, List<L_Words>> { };
    //Este es para la palabra que se modifico y los siguientes scores
    public static int Query_length;
    private static int Query_length_without_bugs;  //dimension del query sin palabras irrelevantes
    public static List<string> query_words = new List<string> { };
    public static Dictionary<string, List<string>> query_derivadas = new Dictionary<string, List<string>> { };
    public static List<string> Original_query = new List<string> { };  //Tiene el query original


    #endregion

    #region  Entrada
    // Metodo portal
    public static Dictionary<string, int> Query(string query)  //Repeticiones de la query para su tf idf
    {

        List<string> temp = Words.Divison_Words(query, false); // Sepera las palabras por los signos

        Query_length = temp.Count;

        Dictionary<string, int> Query_Repetions = new Dictionary<string, int> { };

        List<string> query_Keys = new List<string> { };

        for (int i = 0; i < temp.Count; i++)
        {
            string query_temp = temp[i];

            string x = Words.Normalize_words(query_temp);// Sin acentos

            if (x != " ")
            {
                Original_query.Add(x); //Añade las palabras sin acentos
            }

            x = Words.GetWord(x);

            Control_Query(ref x, query_temp); //Controla si la palabra esta en algun texto 

            if (!Auxiliar_Class.Bug_words.Contains(x) || temp.Count == 1)// si no es una bug word
            {
                Add_Item(temp[i], x);//Añade las raices y sus derivadas

                if (!Query_Repetions.ContainsKey(x))
                {
                    Query_Repetions.Add(x, 1);
                    query_Keys.Add(x);
                    continue;
                }
                Query_Repetions[x]++;
            }


        }
        query_words = query_Keys;
        Query_length_without_bugs = Query_Repetions.Count;
        return Query_Repetions;


    }

    #endregion

    #region Score
    public static SortedList<string, double> Score(string query)// Cos simititude query
    {
        SortedList<string, double> score = new SortedList<string, double> { };

        int Documents_Count = Auxiliar_Class.Documents_Count;

        Dictionary<string, int> r = Query(query);

        List<string> DocumentsName = Auxiliar_Class.Documents_names;

        double[,] Tf_idf_matrix = new double[Documents_Count, Query_length_without_bugs];

        double[] p = tf_idf(r);

        double[,] z = VectorD(query_words, Tf_idf_matrix);


        Tf_idf_matrix = z;

        double score_t = double.MinValue;

        double[] temp = new double[Query_length_without_bugs];
        for (int i = 0; i < z.GetLength(0); i++) //por las palabras
        {
            string doc = DocumentsName[i];
            for (int k = 0; k < z.GetLength(1); k++)//por los doc
            {

                temp[k] = Tf_idf_matrix[i, k];

            }
            score_t = Vectorizer.score(p, temp);
            score.Add(doc, score_t); //Guardar en un dicc para despues leerlo

        }

        return score;
    }


    static void Add_Item(string word, string root)  // Palabras derivadas de la consulta
    {
        if (query_derivadas.ContainsKey(root))
        {
            if (!query_derivadas[root].Contains(word))
            {
                query_derivadas[root].Add(word);
                query_derivadas[root].Sort(new StringLengthComparer()); //organizar de mayor a menor
            }
        }
        else
        {
            List<string> x = new List<string> { word };
            query_derivadas.Add(root, x);
        }


    }



    #endregion

    #region query tf idf
   private static double[] tf_idf(Dictionary<string, int> x)

    {
        return Tf_Idf_class.Query(x, query_words.Count, query_words);//  Tf_idf_de la consulta

    }

    #endregion

    #region vectorizer

   private static double[,] VectorD(List<string> query, double[,] matrix) 
    /* Vectorizar el espacio vectorial de las palabras de la consulta a su imagen en los
      documentos  */
    {
        //Iterar para conocer su tf_idf en cada texto

        for (int i = 0; i < query.Count; i++)
        {
            matrix = TF_idf_Texts(query[i], matrix, i);
        }
        return matrix;
    }


    #endregion

    #region Buscar por cada texto

    static double[,] TF_idf_Texts(string query, double[,] matrix, int word_pos)
    {

        List<string> Documents_name = Auxiliar_Class.Documents_names;

        Dictionary<(string, string), double> Tf_idf_Words = Auxiliar_Class.Tf_Idf;

        for (int i = 0; i < Documents_name.Count; i++)
        {
            string doc = Documents_name[i];

            if (Tf_idf_Words.ContainsKey((doc, query)))  
                                                        
            {//control es por si hay que hacer Levenshtein y rebajar el score
                double tf_idf = Control_Score(query, doc, Tf_idf_Words, word_pos);

                matrix[i, word_pos] = tf_idf;

            }

            else
            {

                matrix[i, word_pos] = 0; //aca llega solo si no esta en ese texto o si no hay coincidencias
                // en la actualizacion con los sinonimos seran muy raros los casos en que este sea cero 
                // a menos que el operador busqueda explicita sea activado 
            }


        }


        return matrix;

    }





    #endregion

    #region Control Score
    // Aca surgue la magia donde si se ha tenido que buscar una palabra coincidente
    // o si se ha agregado el operador * para elevar el score de esta palabra/as
    private static double Control_Score(string word, string document, Dictionary<(string, string), double> Tf_idf_Words, int word_pos)
    {

        float control = 1;
        bool Is_Change = false;
        if (Leves_words_sc.ContainsKey(word))
        {
            control = Leves_words_sc[word];
            Is_Change = true;
        }

        Up_Score up = new Up_Score(word, 0);
        float oper = 1;

        int index = Up_score_operator.IndexOf(up);

        if (index > -1)
        {
            Up_Score z = Up_score_operator[index];
            oper = z.count;

        }
        double d = Tf_idf_Words[(document, word)];

        double o = Similar_Word(word, document, Is_Change);
        o = o == 0 ? 0.1 : o;
        return d * control * oper * o;
    }




      // Envia si exite con respecto a la palabra raiz de la consulta y sus derivadas
      // la coincidencia en cada texto 
      // En la sgt actualizacion se añadira variacion respecto a si todas la derivas se encuentran en el
      // texto
    private static float Similar_Word(string word, string document, bool Is_Change) //Aca se busca si hay coindencia con las derivadas y con la de mayor es esa
    {
        // Devuelve 1 si exite al menos una derivada
        // Y en caso de ser una palabra encontrada pues con las de 
        // mayor similitud devuelta y ese sera su score
        SortedList<string, HashSet<Tupples>> s = Auxiliar_Class.Derivadas;

        HashSet<Tupples> temp = s[document];

        if (!Is_Change)//Si no se tuvo que transformar
        {
            if (s.ContainsKey(document))
            {


                List<string> de = query_derivadas[word];

                for (int i = 0; i < de.Count; i++)
                {
                    string der = de[i];
                    Tupples t = new Tupples(word, der, der.Length);
                    if (temp.Contains(t))
                    {
                        return 1;
                    }
                }

            }
        }
        if (Is_Change)
        {
            List<L_Words> list = Levens_words[word];

            for (int i = 0; i < list.Count; i++)
            {
                L_Words l = list[i];

                Tupples tu = new Tupples(word, l.Word, l.Word.Length);
                if (temp.Contains(tu))
                {
                    return l.Score; //si contiene alguna de las de mayor coincidencia retorna ese score
                }
            }


        }

        return (float)0.85;


    }


    #endregion

    #region  Levenshtein Query
    private static void Control_Query(ref string root_query, string origina_q) //Manda la palabra al  al Levenshtein_Distance para verificar esta en raiz
    {
        HashSet<string> w = Auxiliar_Class.Root_words;

        if (!w.Contains(root_query))
        {
            //devolver la palabra raiz  //Enviar la original
            List<L_Words> list = LevD.Levenshtein_Distance(origina_q);

            if (list.Count > 0)
            {
                root_query = list[0].Word;

                root_query = Words.GetWord(root_query);
                Levens_words.TryAdd(root_query, list);//Añadir al dicc de Leves_Words
                if (query_derivadas.ContainsKey(origina_q)) //Cambia la root hacia la nueva
                {
                    List<string> deriv = query_derivadas[origina_q];
                    query_derivadas.Remove(origina_q);
                    query_derivadas.TryAdd(root_query, deriv);


                }
            }

        }


    }





    #endregion


    #region Clean query

    public static void Clear_query()
    {
        query_words.Clear();

        query_derivadas.Clear();

        Query_length = 0;

        Query_length_without_bugs = 0;

        Levens_words.Clear();

        Leves_words_sc.Clear();

    }



    #endregion





}