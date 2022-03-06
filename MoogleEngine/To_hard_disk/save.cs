using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Web;
using System.Runtime.Serialization.Json;
namespace MoogleEngine;
//Nombre de la carpeta es auxiliar
//En la carpeta Documents Info 
/* Carpeta Documents_Directory    con sus path  
   ../auxi/Documents_Info/Documets Directory  ruta 
*/

/*
public static class Save{

#region Serialize
  public static void Serialize(Stream stream,Saves_Doc save) // Guarda en disco la info de los documentos
    {

            DataContractJsonSerializer d = new DataContractJsonSerializer(typeof(List<string>));

    }
    #endregion
  





     #endregion

     #region Crear directorios
    public   static void Create_Directory(string dicc, string a,ref bool Is_Create)
       {  
                     string path=@"../auxi/";
                     path=path+a;
                     if (!Existis_Path(path))  //Comprobar exixtencia directorio
                     {
                          Directory.CreateDirectory(path); //Crea un directorio nuevo con ese path
                     }
                    else
                    {
                        Is_Create =true; //Indica que ya esta creado el directorio
                    }
       }

#endregion

#region Path
*/
/*
    public   static string Path(string Directory, string File)
       {
           string path=Directory+"/"+File;
           return path ;
       }
       
    public   static string Path_Aux(string file) //Pata crear doc
       {
           string path=@"../aux/";
           path=path+"/"+file+".bin";
          return path;
       }
       
      public   static string Path_Texts_Folder(string file)  //aPara comprobar si existe esa carpeta
      {

           string path=@"../auxi/Documents_Info/documents_words";
           path=path+"/"+file;
          return path;

      }

     */
/*
    public   static bool Existis_Path(string path)
       {
             if (Directory.Exists(path))
           {
               return true;
           }

           return false;
       }

#endregion





}
*/
