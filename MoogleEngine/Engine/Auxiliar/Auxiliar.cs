 
namespace MoogleEngine;
 using System.Drawing;
    public class Auxiliar_Class
    {

      // Esta clase solo se encarga de pasar la referencia del objeto de campo de cada clase
      // Llamemoslo intermediario
        #region Archivos
        public static int Documents_Count{get{return Documents_names.Count;}}
          //Cant de archivos
        #endregion

        #region Read Documents
        public static Dictionary<string,List<string>> Original_Words{get{return Read.Original_Words;}}
           //Devuelve las palabras originales pasadas por split
        #endregion
        
        #region Documents_Name
        public static List<string> Documents_names{get{return Control_Class.Documents_names;}}
          // Retorna el nombre de los documentos sin las direcciones
    
        #endregion

         #region Control =texts  = texts_name
  
          public static HashSet<string>documents_original_names{get{return Control_Class.documents_original_names;}}

          public static Dictionary<string,int>doc_name_repe{get{return Control_Class.doc_name_repe;}}

          public static Dictionary<string,List<string>>doc_coincidences{get{return Control_Class.doc_coincidences;}}

         #endregion
        
         #region Clasificar
         public static Dictionary<string,List<string>>Where_Is_the_Word{get{return Sort.Where_Is_the_Word;}}
         
         
         #endregion

         #region  Words
         public static Dictionary<(char,int),HashSet<string>>TO_le_words{get{return Words.dicc;}}
         public static Dictionary<string,List<string>> Texts_Words{get{return Sort.Words_Text;}}
        
         public static SortedList<string,HashSet<Tupples>>Derivadas{get{return Words.Derivadas_G;}}
         //Derivadas se incluira el dia de la exposicion
         public static  HashSet<string>Root_words{get{return Re_Start.Root_words;}}
         
         #endregion

        
         #region  Tf-Idf
        public static Dictionary<string,double>Query_tf_idf{get{return Tf_Idf_class.Query_tf_idf;}}
        public static Dictionary<(string,string),double> Tf_Idf{get{return Tf_Idf_class.Tf_Idf_instance;}}

        public static Dictionary<string,Dictionary<string,float>>Tf{get {return Tf_Idf_class.tf_words;}}  
          //Aca dejo las palabras muy repetidas

          #region Bugs_Words
            public static HashSet<string>Bug_words{get{return Tf_Idf_class.bug_words;}}

            public static Dictionary<(string,string),double>Last_Tf_Idf_bug_words{get{return Tf_Idf_class.Tf_Idf_last_of_bugs;}}

           #endregion


         #endregion

         #region Query
          
           public static Dictionary<string,List<string>>Query_Root_Der{get{return Query_Class.query_derivadas;}}
                                  //root     derivates
          public static List<string>QueryWords{get{return Query_Class.query_words;}}

          public static List<string>Original_query{get{return Query_Class.Original_query;}}

         #endregion

        #region Operators
        
        public static  List<string> NO_Text{get{return Operators.Textos_no_resultados;}}

        #endregion
       
       #region Levenshtein
 
          public static Dictionary<string, HashSet<L_Words>> Levenstein_words{get{return LevD.Levenstein_words;}}

       #endregion
    }
