using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Windows;

namespace MoogleEngine;
public class Read
{

    #region  Campo
                          // doc     original words
    public static Dictionary<string, List<string>> Original_Words = new Dictionary<string, List<string>> { };

    public static int Cant_Textos;

    #endregion



    #region Archivos //Aca se lee la carpeta para conocer el nombre y cant de archivos

    //Este metodo lee los nombres de los archivos de la carpeta
    //Entrega a estos en un array
    public static string[] Files()
    {
        //Metodo para leer lo que contiene la carpeta
        string path = Path.Join(Environment.CurrentDirectory, "..", "Content");
        string[] files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);
        Cant_Textos = files.Length;
        return files;

    }

    #endregion


    #region Read Txts

    static void Read_Documents(string[] Documents)
    {


        for (int i = 0; i < Documents.Length; i++) // Itera para guardar las palabras en una lista
        {
            StreamReader reader = new StreamReader(Documents[i]);
            string temp = reader.ReadToEnd();

            List<string> content = Words.Divison_Words(temp, true);

            string doc_full_name = Documents[i]; //Nombre del docu con su direccion

            Add_Items(content, doc_full_name, temp);

        }

        static void Add_Items(List<string> content, string document, string original_document)
        {

            (string, bool) a = Control_Class.Names_Coincidances(document, original_document);
            string temp = a.Item1;

            if ((a.Item2 == true) || (a.Item2 == false && temp != null))
            {

                Original_Words.Add(temp, content);//Palabras del texto


            }

        }




    }

    #endregion


    #region Start
    public static void Start()
    {
        string[] documents = Files();

        Read_Documents(documents);
    }
    #endregion




}






































































































