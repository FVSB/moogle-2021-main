using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace MoogleEngine;

public static class Words
{
    #region Campos
           // Para guardar la palabras que se usan en Add_Items 
           // con (Char,int) proximamente este tambien funcionara
           // con el hashset de las palabras raices      
    private static  HashSet<string> tengo = new HashSet<string> { };

    //Palabras con tildes se añadira la disminucion del score en 
    // caso que la palabra de la consulta tenga o no tilde 
    // necesario para casos como papá y papa
    public static SortedList<string, string> Tildes = new SortedList<string, string> { };
    
    // en este por (char,int) donde en la por cada char y su longitud se 
    // tiene el conjunto de palabras que cumplen estos requisitos
    // necesario para Levenshtein_Distance
    public static Dictionary<(char, int), HashSet<string>> dicc = new Dictionary<(char, int), HashSet<string>> { };
                 
       // Aca se tiene un Lista ordenada con el documento y el objeto de la clase
       // tupples que tienen (string root_word, string derivate, int length) 
       // como informacion a guardar
       // se compara igualdad por palabras derivadas iguales
       // se compara si es mayor o menor por el length
              
    public static SortedList<string, HashSet<Tupples>> Derivadas_G = new SortedList<string, HashSet<Tupples>> { };
    

    // Si una palabra ha pasado por el steaming 

    public static Dictionary<string, string> Memory = new Dictionary<string, string> { };
    
    #endregion

    #region Words
    public static string Normalize_words(string word)  //Este es para la query_Class 
    {
        word.ToLower();
        if (word != null && word != "" && word != " " && word != "\n" && word != "\r")
        {
            string c = Signos(word);
            if (c != string.Empty && c != " ")
            {
                return c;
            }
        }

        return null!;

    }

    public static string GetWord(string word)// devuelve una palabra normalizada y su raiz
    {
        if (Memory.ContainsKey(word))
        {
            string temp=Memory[word];
            return temp;
        }
        
        word = Quitar_Acentos(Signos(word));
        if (word.Length >= 7)
        {
            word = Get_Root_Word(word);
        }

        return word;
    }
    public static string GetWord(string word, string document)// metodo para la clase sort
    // los metodos auxiliares Add_Items son necesarios para poder cargar las variables de campo

    {
        word.ToLower();

        if (Memory.ContainsKey(word))
        {
            string root_word = Memory[word];
            Derivadas(root_word, word, document);
            return root_word;
        }
        if (word != null && word != "" && word != " ")
        {
            string temp = Quitar_Acentos(word);
            temp = Signos(temp);
            string root_word = Get_Root_Word(temp);

            Add_Items(temp);

            temp = root_word;
            Derivadas(root_word, temp, document);

            Memory.TryAdd(temp, root_word);

            return root_word;



        }
        return word!;

    }


    private static string Get_Root_Word(string word) //devuelve una palabra raiz
    {

        if (Memory.ContainsKey(word))
        {
            return Memory[word];
        }
        if (word.Length > 7)
        {
            return Raiz(word);
        }
        return word;

    }

    #endregion



    #region Derivadas  
    private static void Derivadas(string root, string derivate, string document)

    {
        HashSet<Tupples> temp = new HashSet<Tupples> { };
        Tupples tupple = new Tupples(root, derivate, derivate.Length);

        if (!Derivadas_G.ContainsKey(document))
        {
            temp.Add(tupple);
            Derivadas_G.Add(document, temp);
        }
        else
        {
           

                Derivadas_G[document].Add(tupple);

            

        }




    }

    #endregion

    #region  Acentos

    private static string Quitar_Acentos(string word)
    {
        if (Tildes.ContainsKey(word))
        {
            return Tildes[word];
        }
        int h = word.GetHashCode();
        string temp = Regex.Replace(word.Normalize(System.Text.NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
        int x = temp.GetHashCode();
        if (h != x && !(Tildes.ContainsKey(word)))
        {
            Tildes.Add(word, temp);
        }
        return temp;
    }


    #endregion

    #region Signos
    /* Se auxilia de la biblioteca using System.Text.RegularExpressions;
      para el uso de Regex las regular expresion 
    */
    private static string Signos(string word)
    {
        string patron = @"[^\w]"; //Eliminar todo lo que no sea letra o digitos

        Regex regex = new Regex(patron);
        string temp = regex.Replace(word, "");
        int i = temp.IndexOf('_');
        while (i > -1)
        {
            temp = temp.Remove(i, 1);
            i = temp.IndexOf('_'); //_ lo reconoce como caracter 
        }
        return temp;

    }


    #endregion

    #region Hallar la raiz de la palabra
    // Las raices seran solo para las palabras de mas de 7 caracteres 
    // aca se prueba si tiene cada prefijo y sufijo
    // Este metodo se actualizara por uno mas eficiente
  

    static string Raiz(string palabra)

    {
       

        string temp = palabra; //string para almacenar la palabra
        string temp2 = "";
        string[] Prefijos = { "sub", "ante", "pre" }; //Poner mas sufijos

        string[] Terminaciones = { "ades", "ado", "ido", "to", "se", "ar", "er", "ir", "a", "e", "i", "o", "u", "s" };//poner en orden de cant de caracteres  de mayor a menor

        for (int ii = 0; ii < Terminaciones.Length; ii++) //Quitar sufijos

        {
            string terminaciones = Terminaciones[ii];
            int Long = terminaciones.Length;
            if (palabra.EndsWith(terminaciones))
            {
                int remove = palabra.Length - Long;
                temp = palabra.Remove(remove);
                break;
            }

        }
        for (int iii = 0; iii < Prefijos.Length; iii++)  //Quitar prefijos
        {
            string prefijo = Prefijos[iii];
            int Long2 = prefijo.Length;
            if (palabra.StartsWith(prefijo) && palabra.Length > prefijo.Length)
            {
                int remove = palabra.Length - Long2;
                temp2 = temp.Substring(Long2);
                return temp2;
            }


        }
        string set = temp;

        return temp;

    }
    #endregion

    #region Division phases
    // Divide los string por espacios o por simbolos


    public static List<string> Divison_Words(string x,bool only_spaces) // only_spaces 
    // para true si solo se quieren dividir los espacios
    // false si se quieren tb hacer split por los símbolos
    {
                     
         if (only_spaces)
         {
             return x.ToLower().Split(new char[]{' ','\n','\r'}, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
         }             
        
        
        string patron = @"[^\w]";
        Regex regex=new Regex(patron);
        List<string>content=regex.Split(x.ToLower()).ToList<string>();
       string[]remove={String.Empty,"\n","\r",null!};
        for (int i = 0; i <remove.Length; i++)
        {
             int index=content.IndexOf(remove[i]);
             while(index>-1)
             {   content.RemoveAt(index);
                 index=content.IndexOf(remove[i]);
                 
             }
        }
       
    
        
        return content;

    }





    #endregion
   // Metodos auxiliares
    #region Add_items
    private static void Add_Items(string word)
    {
         

        int length = word.Length;
        (char, int) tupple;
        tupple.Item2 = length;
        char[] c = word.ToCharArray();
        if (!tengo.Contains(word))
       {    
        for (int i = 0; i < c.Length; i++)
        {
            char t = c[i];
            tupple.Item1 = t;
           
            if (!dicc.ContainsKey(tupple))
            {
                HashSet<string> n = new HashSet<string> { word };
                dicc.Add(tupple, n);
                tengo.Add(word);
            }
            else
            {
                 dicc[tupple].Add(word);
                 tengo.Add(word);
            }
               
            
        }
     }

    }
    

    #endregion




}















































































































































































































































































































































