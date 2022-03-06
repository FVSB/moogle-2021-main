using System.Drawing;
using System.Text;
namespace MoogleEngine;
public class Snippet_Class
{

    #region Campos
    static Dictionary<string, List<string>> dicc = Auxiliar_Class.Texts_Words;
    //doc     root words
    static Dictionary<string, List<string>> o_w = Auxiliar_Class.Original_Words;
    //         doc     original word

    #endregion
    //Devolver string

    // Devuelve la primera ocurrencia de la palabra 
    // En proximas actualizaciones se añadira la posibilidad de devolver la mas cercana
    // Devuelve hasta 40 palabrass 
    public static string snippet(string document, List<string> words_to_search)
    {
        List<string> original_words = o_w[document];

        List<string> words_steaming = Auxiliar_Class.Texts_Words[document];
        int index = 0;

        int i = 40;

        StringBuilder temp = new StringBuilder(); //StringBuilder para formar el snippet
        HashSet<int> lis = new HashSet<int> { }; 
        List<int> To_upper = new List<int> { }; // Indice donde esta la consulta para ponerla en 
        // mayus
        while (i > -1 && index < words_to_search.Count)
        {
            string search = words_to_search[index];

            if (!Auxiliar_Class.Bug_words.Contains(search) || words_to_search.Count == 1)// si no es una bug word
            {
                HashSet<int> a = Snipper(search, " ", original_words, words_steaming, ref i, ref To_upper);
                lis.UnionWith(a);

                index++;

            }


        }
        Snippet(lis, original_words, To_upper, ref temp);
        return temp.ToString();
    }

    static HashSet<int> Snipper(string search, string search_ori, List<string> words, List<string> words_steaming, ref int i, ref List<int> word_query_pos)
    {

        HashSet<int> snipper = new HashSet<int> { };
        int index = words_steaming.IndexOf(search);
        if (index == -1)
        {

            return snipper;
        }
        int start = index - 11 > 0 ? index - 10 : 0;
        int wall = words.Count;
        int stop = start + 20 < wall ? start + 20 : wall;

        while (start < stop && i > -1)
        {
            
            //Inspecciona si la palabra es la de la busqueda si es ella la pone en mayuscula
            
            if (!snipper.Contains(start))
            {
                snipper.Add(start);
            }
            ++start;
            word_query_pos.Add(index);
            i--;
        }



        return snipper;


    }

     // Aca se la pasa la posiciones y completa la palabra
     // generalmente la palabra quedara encuadrada y ademas en caso
     // de dos palabras de la consulta quedar cerca 
     // como Hola Mundo   en la respuesta quedara HOLA MUNDO ...
    private static void Snippet(HashSet<int> lis, List<string> words, List<int> spaces, ref StringBuilder temp)
    {
        temp.Append("(...)");
        for (int i = 0, j = 1; i < lis.Count && j < lis.Count; i++, j++)
        {
            int index = lis.ElementAt(i);
            int indexpro = lis.ElementAt(j);
            string a = spaces.Contains(index) ? words[index].ToUpper() : words[index];
            temp.Append(" " + a);
            if (!(index + 1 == indexpro))
            {
                temp.AppendJoin(" ", "(...)");
            }

            if (j == lis.Count - 1)
            {
                temp.AppendJoin(" ", "(...)");
            }



        }







    }







}