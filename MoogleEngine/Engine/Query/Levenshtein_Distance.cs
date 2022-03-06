using System.Collections.Generic;
namespace MoogleEngine;
public class LevD

{
    #region Campo
    public static HashSet<(string, string)> Suggestion = new HashSet<(string, string)> { };
    //Palabra Original     Palabra con mayor coincidencia
    public static Dictionary<string, HashSet<L_Words>> Levenstein_words = new Dictionary<string, HashSet<L_Words>> { };
    //Palabra Original     Palabras con coincidencia

    #endregion
    //Entrada del la distancia de levenstein
    #region Entrada
    public static List<L_Words> Levenshtein_Distance(string original_query)
    {

        if (original_query.Length < 3)//si tiene menos de 3 caracteres se devuelve null
        {
            return null!;
        }
        List<L_Words> e = Exists(original_query);
        if (e.Count != 0)
        {
            return e;
        }

        int[] error = palabra(original_query); //Solo se devolveran palabras que
        // puedan tener un 75% de error (adiccion o omision de letras)

        int length = original_query.Length;

        Dictionary<(char, int), HashSet<string>> dicc = Auxiliar_Class.TO_le_words;

        HashSet<string> list = Union(original_query, error, length, dicc);

        List<L_Words> suggestion = (Score(list, original_query));

        suggestion = IS_in_This_Texts(suggestion);

        suggestion.Sort(new L_Words_Score_Comparer());

        return suggestion;  //Retornar el score y la palabra



    }
    #endregion

    #region  Exits
    // Comprueba si se ha realizado la consulta con anterioridad
    private static List<L_Words> Exists(string word)
    {
        List<L_Words> list = new List<L_Words> { };
        if (Levenstein_words.ContainsKey(word))
        {
            list = Levenstein_words[word].ToList<L_Words>();
            list.Sort(new L_Words_Score_Comparer());
            return list;
        }


        return IS_in_This_Texts(list);

    }
// Comprueba que este en los texto
// en estos momentos siempre estara en el corpus
// pero con las proximas actualizaciones no siempre sera asi
    private static List<L_Words> IS_in_This_Texts(List<L_Words> l)
    {
        List<L_Words> a = new List<L_Words> { };

        Dictionary<(string, string), double> dicc = Auxiliar_Class.Tf_Idf;

        List<string> docu_names = Auxiliar_Class.Documents_names;

        for (int i = 0; i < l.Count; i++)
        {
            bool c = true;

            string temp = l[i].Word;

            temp = Words.GetWord(temp);

            for (int j = 0; j < docu_names.Count && c == true; j++)
            {
                string doc = docu_names[j];
                (string, string) tu = (doc, temp);
                if (dicc.ContainsKey(tu))
                {
                    a.Add(l[i]);
                    c = false;
                }

            }
        }
        return a;

    }

    #endregion

    #region Conjuntos de HashSet
    //Usamos el metodo UnionWith() de los HashSet para tener en nuestro
    // conjunto solo las palabras que cumplan con los requisitos
    private static HashSet<string> Union(string word, int[] error, int length, Dictionary<(char, int), HashSet<string>> dicc)
    {
        char[] array = word.ToArray();
        int s = length - error[0];
        (char, int) q = (array[0], s);
        HashSet<string> temp = dicc[q];
        if (dicc.ContainsKey(q))
        {


            for (int j = 0; j < error.Length; j++)
            {
                int l = length - error[j];
                q.Item2 = l;

                HashSet<string> w = Union_2(word, array, l, dicc);

                temp.UnionWith(w);
            }

        }


        return temp;

    }

    private static HashSet<string> Union_2(string word, char[] array, int index, Dictionary<(char, int), HashSet<string>> dicc)
    {

        (char, int) q = (array[0], index);
        List<char> tengo = new List<char> { array[0] };
           HashSet<string> temp =new HashSet<string>{};
        if (dicc.ContainsKey(q))
        {
          temp = dicc[q];
        }
       

        for (int i = 1; i < array.Length; i++)
        {
            char c = array[i];
            q.Item1 = c;
            if (tengo.Contains(c))
            {
                continue;
            }
            if (dicc.ContainsKey(q))
            {
                HashSet<string> tem = dicc[q];
                temp.UnionWith(tem);
                tengo.Add(c);
            }
        }


        return temp;

    }
    #endregion

    #region  Auxiliar
    // Nos da el marguen de error de omision y adiccion de caracteres 
    private static int[] palabra(string word)
    {
        int error = (int)(word.Length * (0.25));
        int[] x = new int[(error * 2) + 1];
        for (int i = 0, j = -error; i < x.Length; i++, j++)
        {
            x[i] = j;
        }
        return x;
    }

// Metodo director del score 
// Cuando llega el cjto de palabras
// este envia el metodo  LevenshteinDistance todas las palabras
// organiza a estas por score
// si se tiene mas del 95% de coincidencia se devolvera la lista instantaneamente
    private static List<L_Words> Score(HashSet<string> x, string y)
    {
        List<L_Words> v = new List<L_Words> { };
        float score = 0;
        string temp = string.Empty;

        for (int i = 0; i < x.Count; i++)
        {
            string a = x.ElementAt(i);
            float t = 1 - LevenshteinDistance(a, y);
            if (t > score)
            {
                score = t;
                temp = a;
                L_Words l = new L_Words(temp, score);
                v.Add(l);
                if (t >= 0.95)
                {
                    v.Sort(new L_Words_Score_Comparer());
                    Add_Items(v, y);
                    return v; //coincidencia muy alta
                }
            }

        }

        if (score < 0.55)
        {
            v.Clear();
            L_Words l = new L_Words(y, -1);
            v.Add(l);
            return v;
        }
        v.Sort(new L_Words_Score_Comparer());
        Add_Items(v, y);
        return v;




    }

    #endregion
    #region  Add Items
    private static void Add_Items(List<L_Words> l, string word)
    {
        l.Sort(new L_Words_Score_Comparer());
        if (l.Count > 3)
        {
            int stop = (l.Count - 3);
            l.RemoveRange(3, stop);   //Solo se queda con las 3 primera opciones
        }
        HashSet<L_Words> temp = l.ToHashSet<L_Words>();

        if (!Levenstein_words.ContainsKey(word))
        {
            Levenstein_words.TryAdd(word, temp);
        }
        else
        {
            Levenstein_words[word].UnionWith(temp);
        }

        if (l.Count > 1)
        {
            string suggestion = l.ElementAt(0).Word;
            (string, string) sugges = (word, suggestion);
            Suggestion.Add(sugges);// Aca se suma la sugerencia de la de mayor score
        }


    }



    #endregion



    #region Levenshtein
    private static float LevenshteinDistance(string s, string t)
    {
        float porcentaje = 0;

        // d es una tabla con m+1 renglones y n+1 columnas
        int costo = 0;
        int m = s.Length;
        int n = t.Length;
        int[,] d = new int[m + 1, n + 1];

        // Verifica que exista algo que comparar
        if (n == 0) return m;
        if (m == 0) return n;

        // Llena la primera columna y la primera fila.
        for (int i = 0; i <= m; d[i, 0] = i++) ;
        for (int j = 0; j <= n; d[0, j] = j++) ;


        /// recorre la matriz llenando cada unos de los pesos.
        /// i columnas, j renglones
        for (int i = 1; i <= m; i++)
        {
            // recorre para j
            for (int j = 1; j <= n; j++)
            {
                /// si son iguales en posiciones equidistantes el peso es 0
                /// de lo contrario el peso suma a uno.

                costo = (s[i - 1] == t[j - 1]) ? 0 : 1;
                d[i, j] = System.Math.Min(System.Math.Min(d[i - 1, j] + 1,  //Eliminacion
                              d[i, j - 1] + 1),                             //Insercion 
                              d[i - 1, j - 1] + costo);                     //Sustitucion
            }
        }

        /// Calcula el porcentaje de cambios en la palabra.
        if (s.Length > t.Length)
            porcentaje = ((float)d[m, n] / (float)s.Length);
        else
            porcentaje = ((float)d[m, n] / (float)t.Length);
        return porcentaje;
    }

    #endregion
    #region  Clear

    public static void Clear()
    {

        Suggestion.Clear();

    }

    #endregion

}