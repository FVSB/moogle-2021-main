

using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace MoogleEngine;

public class Tf_Idf_class
{
    #region  Campos
    public static Dictionary<string, Dictionary<string, float>> tf_words = new Dictionary<string, Dictionary<string, float>> { };
    //    doc          word     tf
    public static Dictionary<string, double> Query_tf_idf = new Dictionary<string, double> { };
    public static Dictionary<(string, string), double> Tf_Idf_instance = new Dictionary<(string, string), double> { };

    static Dictionary<string, float> Idf_instance = new Dictionary<string, float> { };
    //             Root_Word Idf

    public static HashSet<string> bug_words = new HashSet<string> { };
    //aca se ponen las palabras que alteran el tf idf
    private static Dictionary<string, (List<string>, int)> Control_score = new Dictionary<string, (List<string>, int)> { };
    //              word        doc        count rep
    public static Dictionary<(string, string), double> Tf_Idf_last_of_bugs = new Dictionary<(string, string), double> { };

    #endregion

    #region tf idf Documents
    public static void Tf_Idf_Documents(Dictionary<(string, string), int> Words_Repeticions)
    {

        Dictionary<string, float> Idf = Idf_instance;


        List<(string, string)> Keys = Sort.Words_Count.Keys.ToList<(string, string)>();

        for (int i = 0; i < Keys.Count; i++) //por la cant de palabras 
        {
            (string, string) tu=Keys[i];
            string word =tu.Item2;
            string document = tu.Item1;
            if (Idf_instance.ContainsKey(word))  //Si el dicc de idf contiene la palabra
            {

                float tf = calculate_tf(Words_Repeticions[tu], Keys.Count, word, document);
                float idf = (Idf_instance[word]);
                double tf_idf = (tf * idf);

                Control_Scores(word, tf, tf_idf, document); //control de palabras 
                // que no aportan relevancia como las conjunciones

                Tf_Idf_instance.Add(Keys[i], tf_idf);
            }

        }



    }



    #endregion

    #region Idf

   private static float Idf(int Cant_Textos, int Repeticiones)

    {
        double f = (double)1 + (Cant_Textos / Repeticiones);
        float idf = ((float)Math.Log10(1 + f));

        return idf;
    }

    #endregion

    #region Tf
    private static float Tf(int Repeticiones, int Palabras_en_texto)
    {

        float a = (1 + ((float)Math.Log2(1 + Repeticiones)));

        float b = Palabras_en_texto > 1 ? ((float)Math.Log2(Palabras_en_texto)) : 1;

        float tf = a / b;

        return tf;

    }

    #endregion

    #region Control the conjuntions  
    //Controlo que el tf sea bajo
    
    private static void Control_Scores(string word, float tf, double tf_idf, string document)
    {
        if (word.Length < 4)
        {


            if (tf > 0.1 && tf_idf > 0.10)
            {

                if (!Control_score.ContainsKey((word)))
                {
                    List<string> x = new List<string> { document };

                    Control_score.Add(word, (x, 1));
                }
                else
                {
                    (List<string>, int) a = Control_score[word];
                    (List<string>, int) b = a;
                    b.Item1.Add(document);
                    b.Item2++;
                    Control_score[word] = b;

                }

            }

        }

    }
    //Se llama en el metodo start para restablecer el tf idf de estos
    private static void Control_Scores()
    {
        SortedList<string, int> counts = new SortedList<string, int> { };

        List<string> keys = Control_score.Keys.ToList<string>();

        float tf_idf = (float)0.001;//New tf_idf

        int porcent = (int)(Auxiliar_Class.Documents_Count * 0.65);//s se repiten en mas del 65% de 
        // estos con score alto se le rebaja el score se llamaran bug words

        for (int i = 0; i < keys.Count; i++)
        {
            string word = keys[i]; //Palabra con alto tf

            (List<string>, int) a = Control_score[word]; //sacamos donde puedan estar
            int x = a.Item2;
            List<string> text = a.Item1; //lista de textos donde se repiten
            if (x >= porcent)
            {
                for (int j = 0; j < text.Count; j++) //por cada documento bajamos su tf idf
                {
                    (string, string) b = (text[j], word);

                    if (!Tf_Idf_last_of_bugs.ContainsKey(b))
                    {
                        Tf_Idf_last_of_bugs.Add(b, Tf_Idf_instance[b]);
                    }


                    if (Tf_Idf_instance.ContainsKey(b))
                    {
                        Tf_Idf_instance[b] = tf_idf;
                    }


                }

                if (!bug_words.Contains(word))//Se añade a la listsa wue ña contiene para despues usar en el snipper
                {
                    bug_words.Add(word);
                }
            }

        }





    }










    #endregion

    #region calculate tf
    // Aca se calcula el tf y se guarda en un dicc
    // cuando se guarden las infos de estos para una carga instanteanea
    // se guardara el tf pq en caso de modificarse un texto 
    // se tendra que volver a calcular el idf y el tf
    private static float calculate_tf(int Repeticiones, int Palabras_en_texto, string word, string document)
    {
        float temp = Tf(Repeticiones, Palabras_en_texto);
        if (!tf_words.ContainsKey(document))
        {
            Dictionary<string, float> a = new Dictionary<string, float> { };
            a.Add(word, temp);
            tf_words.TryAdd(document, a);
        }
        else
        {
            tf_words[document].TryAdd(word, temp);

        }


        return temp;


    }


    #endregion

    #region Query
    public static double[] Query(Dictionary<string, int> words, int words_count, List<string> keys)  //Para el tf idf del query
    {
        int Keys_Count = keys.Count;
        double[] query = new double[Keys_Count];

        for (int i = 0; i < Keys_Count; i++)
        {
            string word = keys[i];

            int repetitions = words[word];
            if (!Query_tf_idf.ContainsKey(word))
            {
                double tf_idf = Tf(repetitions, words_count) * Idf(1, 1);

                Query_tf_idf.Add(word, tf_idf);
                query[i] = tf_idf;
            }




        }

        return query;

    }


    #endregion
   //Metodo portal
    #region Start
      public static void Start()
    {

        Start_Tf_Idf_documents();
        Tf_Idf_Documents(Sort.Words_Count);
        Control_Scores();
    }


    static void Start_Tf_Idf_documents()
    {
        int Cant_Textos = Auxiliar_Class.Documents_Count;  //Cant de textos



        Dictionary<string, List<string>> X = Sort.Where_Is_the_Word;   //Dicc con la cant de raices

        List<string> keys = X.Keys.ToList<string>(); //Lista de las  raices

        for (int i = 0; i < keys.Count; i++)
        {
            string Keys_w = keys[i]; //Word

            int Repeticiones = X[Keys_w].Count;

            if (Repeticiones > 0)
            {
                Idf_instance.Add(Keys_w, Idf(Cant_Textos, Repeticiones));


               
            }

        }



    }



    #endregion

    #region Clear
    public static void Clear_Tf_Idf()
    {

        Tf_Idf_class.Query_tf_idf.Clear();




    }





    #endregion































}

