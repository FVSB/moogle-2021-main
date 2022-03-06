
using System.Drawing;
using System.Text;
namespace MoogleEngine;
public class Search_Class
{

    // Entrada de la consulta metodo director de ella 

    #region Buscar
    public static (List<SearchItem>, string) Search(string query)
    //snipper       suggestion

     
    {
       
        List<string> documents_name =  Control_Class.Documents_names;  //Nombre de los doc
          
         Operators.Operator_Set(query);  //Envia al metodo portal de la clase Operator

        SortedList<string, double> dicc = Query_Class.Score(query); //Cos simititude query / text

    
        string[] text = new string[Auxiliar_Class.Documents_Count];// Matriz long cant de textos

        double[] scores = new double[Auxiliar_Class.Documents_Count];//Para devolver el score

        List<SearchItem> items = new List<SearchItem> { };  //Lista que devuelve los resultados


        int dicc_c = dicc.Count - 1;

        List<string> query_words = Auxiliar_Class.QueryWords;
        //Inicializar el metodo que controla la query
        for (int i = 0; i < documents_name.Count; i++)
        {
            string doc_n = documents_name[i];
            double d = dicc[doc_n];
            if (d > 0)
            {
                int pos = dicc_c - dicc.IndexOfKey(doc_n);
                text[pos] = doc_n;
                double scor = dicc[doc_n];
                scores[pos] = scor;

            }
        }

        for (int i = 0; i < text.Length; i++)
        {
            string document = text[i];
            float sco = (float)scores[i];
            if (document != null)
            {
                string snippet = Snippet_Class.snippet(document, query_words);

                SearchItem o = new SearchItem(document, snippet, sco);
                items.Add(o);
            }

        }
        
        string suggestion = "Successful Search :🤪 ";
        Control(ref items,ref suggestion);
       
        Clear_class.Clear();  //Metodo portal clase Clear se llama despues de cada consulta para 
        // limpiar las variables globales que intervienen en la consulta

        return (items, suggestion);
    }


    #endregion

    #region  Control Query
    private static void Control(ref List<SearchItem> items,ref string suggestion)
    {
        List<string> NO_Text = Auxiliar_Class.NO_Text; //Los doc que se eliminaran de los resultados

        List<Up_Scores_Texts> Up = Operators.score_dis; //Textos que sera Upgradeados sus scores
        //   operador ~

      if (items.Count!=0)
      {
          
       
    if (Up!=null)
    {
         for (int i = 0; i < items.Count; i++) // Aumentar el score del texto
        {
            SearchItem item = items[i];
            string doc = item.Title;

            Up_Scores_Texts u = new Up_Scores_Texts(doc,0);
            u.Document = doc;
            int index = Up.IndexOf(u);
            
            if (index>-1)
            {
                  u = Up[index];
                double score = u.Score;
                item.Score =item.Score* (float)score;
                 items[i]=item;
            }
              
        }
    }
       
    
    if (NO_Text!=null)
    {
         for (int i = 0; i < items.Count; i++) //operador !
        {
            SearchItem item = items[i];
            string doc = item.Title;
            if (NO_Text.Contains(doc))
            {
                items.RemoveAt(i);
               i--;

            }
        }
    }
       
        
        
         if (items.Count>1) 
         {
             items.Sort(new SearchItemComparer());
         }
        
      }
      if(items.Count==0)
      {
          string a="La consulta no arroja resultados";
          SearchItem s=new SearchItem("Advertencia",a,1);
          suggestion="Realice una nueva consulta";
          items.Add(s);
      }
      else
      {
           Suggestion(ref suggestion);
      }   
           
    }


    #endregion
    static void Suggestion(ref string suggestion)
    {
        HashSet<(string, string)> s = LevD.Suggestion;
       
     
        if (s.Count != 0)
        {
            StringBuilder temp = new StringBuilder();
            temp.AppendLine("Se incluyen resultados de :"); // Si no se encontro su palabra raiz en ningun texto
            for (int i = 0; i < s.Count; i++)
            {
                (string, string) t = s.ElementAt(i);
                string remove = t.Item1;
                string change = t.Item2;
                temp.AppendJoin(" ", change);
            }
            suggestion = temp.ToString();
        }



    }





}