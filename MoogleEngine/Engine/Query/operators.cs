using System.Text;
namespace MoogleEngine;

public static class Operators
{
 #region  Campo
    private static Dictionary<string, List<string>> W_is_word = Auxiliar_Class.Where_Is_the_Word;
    public static List<string> Textos_no_resultados = new List<string> { };

    public static List<Up_Score> Up_score = new List<Up_Score> { };

    public static  List<Up_Scores_Texts>score_dis=new List<Up_Scores_Texts>{};
   
    private static  bool use_synonyms=false; //Se cuantifica tambien al uso de palabras cercanas o sinonimos

    public static bool Max_Score=false; // A todos los cuantificados por * son los de mayor relevancia
 #endregion 

#region  Entrada
    public static void Operator_Set(string query)
    {
        //Primero busca si hay algun (operator(lo que cuantifica) ) que es donde deben de estar los 
        //operadores de no ser asi continua con normalidad 
        
         StringBuilder temp =new StringBuilder(query.Length);
        bool open =false;
        int count = 0;
        bool only=false; // solo devolver lo cuantificado por ello
        char caracter = ' ';
        int acumuladoSco = 0;
        bool re_start=false;
        if (re_start)
        {
            open=false;
            only=false;
        }
       
        List<(string, char)> operators = new List<(string, char)> { };
       
        if (query.Length>0&&query[0]=='&')
        {
            use_synonyms=true; //Cuantifica si el uso es para sinonimos o no 
        }
        
        for (int i = 0; i < query.Length; i++)
        {
            
            
            char c = query[i];
            if (c=='?')
            {
                only=true;
            }
            if (c == '~' || c == '!' || c == '^' || c == '*'||c=='+')
            {
                caracter = c;
                if (c == '*')
                {
                    acumuladoSco++;
                   
                }
                if (acumuladoSco>0&&c=='+')
                {
                    Max_Score=true;
                }
            }
            if (c == '(')
            {
               open=true;
            }
            if (open)
            {
               temp.Append(c);
            }
            if (c == ')')
            {
                count++;
                if (count == 2)
                {
                    
                    string a = temp.ToString();
                    temp.Clear();
                    open=false;
                   
                    if (caracter == '!')
                    {
                        Dont_Exits(a);
                         re_start=true;
                    }
                    else if (caracter == '*'||caracter=='+')
                    {
                        Up_Score(a, acumuladoSco);
                        acumuladoSco = 0;
                         re_start=true;
                    }
                    else if(caracter=='~')
                    {
                        Near(a,only);
                        re_start=true;
                    }
                    else if (caracter=='^')
                    {
                        Exists(a,only);
                    }
                }
            }

        }


    }
#endregion
#region !
 //Operador que no puede existir en la query
    static void Dont_Exits(string query)
    {
          Textos_no_resultados.AddRange(Where(query,false).Item1);
    }
   
    #endregion
    #region *
    //Metodos donde suben el score
    static void Up_Score(string query, int acumulado)
    {
        float sco = (float)(acumulado * 1.75);
       
      
            HashSet<string>s=Where(query,false).Item2;//Los nombres de los documentos
             
           for (int i = 0; i <s.Count; i++)
           {
               string a=s.ElementAt(i);
               Up_Score t = new Up_Score(a,sco);
               Up_score.Add(t);
           }


       Up_score.Sort(new Up_ScoreComparerScores());
    }

    #endregion
#region ^
    private static void Exists(string query,bool only)
    {
        List<string> docu_names = Auxiliar_Class.Documents_names;
        HashSet<string> t = Where(query,only).Item1;
        for (int i = 0; i < docu_names.Count; i++)
        {
           string doc=docu_names[i];
            if (!t.Contains(doc))
            {
                Textos_no_resultados.Add(doc);
            }
           
        }

    }

 #endregion

  #region ~ operator
  
    private static void Near(string query,bool only)
    {
        List<string> docu_names = Auxiliar_Class.Documents_names;
         
         (HashSet<string>,HashSet<string>)tu= Where(query,true);
         
         HashSet<string> documents_to_search=tu.Item1;
          
         
       if (Control(documents_to_search))
       {
        HashSet<string> words =tu.Item2; //palabras

        if (documents_to_search.Count==1)
        {
            Up_Scores_Texts up =new Up_Scores_Texts  (documents_to_search.ElementAt(0),100);
        
            score_dis.Add(up);
        }
        else
        {  
        Up_Scores_Texts(words,documents_to_search);
            
        }

       
       }
        else
        {
           //Añadir aca en la clase de excepciones
        }
         
         if (only)
         {
             Exists(query,only);
         }
    }
    private static bool Control (HashSet<string>control)
    {
             if (control.Count==0)
             {
                return false; 
             }

           if (control.Count==1&&control.ElementAt(0)==null)
           {
              
              return false;
           }
      return true;

    }

     private static void Up_Scores_Texts(HashSet<string> words, HashSet<string> documents)
    {
            
        Dictionary<string, List<string>> texts_words = Auxiliar_Class.Texts_Words;
        for (int i = 0; i < documents.Count; i++)  //Por cada documento
        {
            List<Word_Index> index = new List<Word_Index> { };
             

            string doc = documents.ElementAt(i);

            List<string> x = texts_words[doc];
            int count = x.Count;
            for (int j = 0; j < words.Count; j++)  //Por cada palabra
            {
               
                string word = words.ElementAt(j);
                Word_Index w_i = new Word_Index(word,0);
                int last_index_of = x.IndexOf(word);
                int index_of = 0;
                do
                {
                    index_of = x.IndexOf(word); //Word_Index es un objeto que se ordena por los indices para hacer mas viable las
                            w_i.Index=index_of;                   //busquedas
                    index.Add(w_i);
                } while (index_of != last_index_of);
            }
              double d=  Distance(index,doc,count);
              Up_Scores_Texts up=new Up_Scores_Texts(doc,d);
            
              score_dis.Add(up);
            
        }
        score_dis.Sort(new Up_Scores_Texts_Score_Comparer());
        
    }

    private static double Distance(List<Word_Index> index, string document, int count)
    {

        index.Sort(new WORDS_INDEX_COMPARER());
        double temp = 0;
       
        for (int i = 0; i < index.Count; i++)
        {
     
          Word_Index   w = index[i];
            int a = w.Index;
            temp +=(double) Math.Pow(a,2);
        }   
        temp = (double)Math.Sqrt(temp);
        temp = (double)(temp / count);
        double e=Math.E;
        temp=(double) (1/Math.Pow(e,temp));
        return temp;
    }

#endregion
#region Aux

  // Metodo clave en esta clase ya que desde la informacion que brinda este las palabras
  // y los textos donde estan (same_text ==true es que tienen que estar en los mismos)
  // El resto de metodos solo añade a lista negras de doc o a elevar scores acorde con la info
  // que brinda este metodo
         //  doc                 words
 private static (HashSet<string>,HashSet<string>) Where(string query,bool Sames_texts)  //Retorna el lugar donde estan los textos
    {  
                                                                      // que tienen que coincidir en los mismos textos
       //Controlar si el uso de sinonimos esta permitido
        HashSet<string>words=new HashSet<string>{}; 

        HashSet<string>temp=new HashSet<string>{}; //Contiene los doc donde esta la palabra
        
        HashSet<string>interception=Auxiliar_Class.Documents_names.ToHashSet<string>();

        List<string> q = Words.Divison_Words(query,false);
        
        HashSet<string>bug=Auxiliar_Class.Bug_words;

        List<string> docu_names = Auxiliar_Class.Documents_names;

          bool Continue=true; //Cierra el ciclo While

        for (int i = 0; i < q.Count; i++)
        {
            Continue=true;

            string a = q[i];
            if (a == null)
            {
                continue;
            }
            a = Words.Normalize_words(a);//No le quita acentos

            string z = a;  //Este es por si hay que usarlo con el leves..

            if (bug.Contains(a)){continue;}  //Si contiene una palabra bug continuara
           
           bool f=true; //Bool que cierra el do while el operador esta en la linea final
              
            do
            {
                a = z!;
                        bool c=string.IsNullOrEmpty(a);
                        
                if (W_is_word.ContainsKey(a)&&c)
                {
                      List<string>x=W_is_word[a];
                    temp.UnionWith(x.ToHashSet<string>());
                    interception.IntersectWith(x.ToHashSet<string>());
                    Continue=false;
                    words.Add(a);
                    continue;
                }
                else if (Words.Memory.ContainsKey(a!)&&a!=null)
                {          a=Words.Memory[a];
                       List<string>x=W_is_word[a];
                    temp.UnionWith(x.ToHashSet<string>());  //Añade donde estan los textos
                    interception.IntersectWith(x.ToHashSet<string>());
                    Continue =false;
                    words.Add(a);
                    continue;   
                }
                else
                {

                    string  x =To_Lev(a!);
                     z=x;
                    if (x == null)
                    {
                       Continue=false;
                    }


                }
                        f=(a != z||z!=null)?true:false;// cerrar el ciclo
            } while (f&&Continue);


        }
         if (Sames_texts)
         {
             return(interception,words);
         }

        return (temp,words);
    }

// Si no se encuenta la palabra en el corpus se envia a  LevenshteinDistance
// se utiliza la de mayor score
private static string To_Lev(string word)  //solo tiene que aparecer la root para que de la busqueda
{

    List<L_Words>l=LevD.Levenshtein_Distance(word);//
     if (l==null||l.Count==0)
     {
         return null!;
     }
     string temp=null!;
    for (int i = 0; i <l.Count; i++)
    {
        temp=l[i].Word;
        temp=Words.GetWord(temp);
      HashSet<string> r=  Auxiliar_Class.Root_words;
        if (r.Contains(temp))
        {
            break;
        }
    }


         
      

     return temp; 

     

}


#endregion

 #region Clear
 //Para que despues de cada query se inicializa todas las variables de campo
    public static void Clear()
    {
        Textos_no_resultados.Clear();
         score_dis.Clear();
        Up_score.Clear();
        Max_Score=false;

    }
#endregion




}