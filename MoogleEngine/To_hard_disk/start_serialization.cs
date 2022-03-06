using System.IO;
using System.Runtime.Serialization;
namespace MoogleEngine;

/*
public static class Serialization
{

    //Guardar el nombre de los archivos

    const string Ext = ".dll";
    public static Dictionary<string, DateTime> Last_Changes = new Dictionary<string, DateTime> { };
    public static Dictionary<string, HashSet<string>> Path_saves = new Dictionary<string, HashSet<string>> { };
    #region  Save documents
    private static void Save_Documents()
    {
        List<string> docu_names = Auxiliar_Class.Documents_names;

        Dictionary<string, List<string>> doc_root = Auxiliar_Class.Texts_Words; //Root words

        SortedList<string, List<Tupples>> doc_derivates = Auxiliar_Class.Derivadas;

        Dictionary<string, Dictionary<string, float>> doc_tf = Auxiliar_Class.Tf;

        Dictionary<string, List<string>> doc_orig_words = Auxiliar_Class.Original_Words;

        //Faltan los sinonimos 
        Dictionary<string, List<string>> sino = new Dictionary<string, List<string>> { };

        for (int i = 0; i < docu_names.Count; i++)
        {
            string document = docu_names[i];

            List<string> root_words = doc_root[document];//Las palabras raices

            List<string> original_words = doc_orig_words[document]; //Las palabras originales 

            List<Tupples> derivates = doc_derivates[document];//Las derivadas

            Dictionary<string, float> tf = doc_tf[document];//tf  de cada palabra

            //Agregar aca los sinonimos

            Saves_Doc save = new Saves_Doc(document, tf, original_words, root_words, sino, derivates);





        }










    }

    #endregion


    private static bool I_need_add(string path)
    {
        bool x = Save.Existis_Path(path);
        if (!x)
        {
            return true;
        }
        if (It_The_Same(path))
        {
            Delete(path); //Elimino el archivo y sustituyo
            return true;
        }
        return false;
    }

    private static bool It_The_Same(string path)
    {

        // DateTime date = Directory.GetLastWriteTimeUtc(path); //Se guarda la ultima hora del cambio en UTC
        if (File.Exists(path))
        {
            DateTime date = File.GetCreationTimeUtc(path);
            if (Last_Changes.ContainsKey(path))
            {
                DateTime temp = Last_Changes[path];
                if (temp != date)
                {
                    return false;  //no es igual
                }
            }
            else
            {
                Add_Items(path, date); //Añadir a Last_Changes
                return false;  //Hubo que añadir
            }
            return true; //es igual
        }
        return false;
    }


    private static void Delete(string path)
    {
        File.Delete(path);
    }
    private static void Create_Directory(string path, string name, ref bool create_now)
    {
        if (name != null)
        {
            path = Path.Join(Environment.CurrentDirectory, path, name);
        }


        bool x = Save.Existis_Path(path);

        if (!x)
        {

            Directory.CreateDirectory(path);
            //A las carpetas de los dpc no mirar el path
            create_now = true;
        }
        else
        {
            create_now = false;
        }

    }



    private static void Save_in_doc_files(Saves_Doc save, string doc_name)
    {  //Save.Path_Texts_Folder(document);
        string root = @"../auxi/Documents_Info/documents_words";

        string path = Path.Join(Environment.CurrentDirectory, root, doc_name); //Cmprueb si esta la carpeta de ese archivo

        bool r = false; //Indica si estaba creada o no la carpeta
                        //Falso ya creada 

        Create_Directory(root, doc_name, ref r); //se guarda en root mas 

        if (r)
        {

            DateTime date = File.GetLastWriteTimeUtc(path); //hacer metodo para comprobar esto
            Add_Items(path, date);
            Add_Items(name, path);
        }
        else if (I_need_add(path))
        {

            DateTime date = File.GetLastWriteTimeUtc(path);
            Add_Items(path, date);
            Add_Items(name, path);
        }

        if (r)
        {
            string path_strem = root + doc_name + Ext;
            Stream stream = new FileStream(path_strem, FileMode.OpenOrCreate);
            Save.Serialize(stream, save);
        }

    }

    private static void Save_Words(string tipo, object save)
    {
        //Save.Path_Aux(dire,tipo);

        string[] root = { "..", "auxi", "Words" };
        string path = Path.Join(root);
        path = Path.Join(Environment.CurrentDirectory, path, "folder " + tipo);
        bool r = false;

        Create_Directory(path, null!, ref r);



        string name = tipo;//Nombre para encontra path
        bool x = false;

        if (r)
        {
            Save.Serialize(path, save, ref x);
            DateTime date = File.GetLastWriteTimeUtc(path);
            Add_Items(path, date);
            Add_Items(name, path);
        }
        else if (I_need_add(path))
        {
            Save.Serialize(path, save, ref x);
            DateTime date = File.GetLastWriteTimeUtc(path);
            Add_Items(path, date);
            Add_Items(name, path);
        }

    }



    public static void Save_public()
    {



    }


}



#region Saves
[DataContract]
public class Saves_Doc : IEquatable<Saves_Doc>
{
    public Saves_Doc(string name, Dictionary<string, float> tf, List<string> original_words, List<string> root_words, Dictionary<string, List<string>> sino, List<Tupples> derivadas)
    {
        this.Name = name;
        this.tf = tf;
        this.Original_Words = original_words;
        this.Derivadas = derivadas;
        this.Root_Words = root_words;
        this.Sino = sino;
    }
    [DataMember]
    public string Name { get; private set; }
    [DataMember]
    public Dictionary<string, float> tf { get; private set; }

    [DataMember]
    public List<string> Root_Words { get; private set; }

    [DataMember]

    public List<string> Original_Words { get; private set; }
    [DataMember]

    public Dictionary<string, List<string>> Sino { get; private set; }
    [DataMember]

    public List<Tupples> Derivadas { get; private set; }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Saves_Doc objAsPart = obj as Saves_Doc;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
    public bool Equals(Saves_Doc other)
    {
        if (other == null) return false;
        return (this.Name.Equals(other.Name));
    }
}

[DataContract]
public class Saves_Info : IEquatable<Saves_Info>
{
    public Saves_Info(HashSet<string> root_words, Dictionary<(string, string), double> tf_idf, HashSet<string> bug_words, Dictionary<string, (List<string>, int)> control_score, Dictionary<(string, string), double> tf_idf_last_of_bugs, SortedList<string, string> tildes, Dictionary<string, string> memory, Dictionary<int, List<string>> length_Words, Dictionary<(char, int), HashSet<WORDS>> char_length)
    {
        this.Root_words = root_words;
        this.Tf_Idf_instance = tf_idf;
        this.Bug_words = bug_words;
        this.Control_score = control_score;
        this.Tf_Idf_last_of_bugs = tf_idf_last_of_bugs;
        this.Tildes = tildes;
        this.Memory = memory;
        this.Length_Words = length_Words;
        this.Char_Length = char_length;
    }
    #region Campo
    const string Name = "Information";
    private static string path = Path.Join(Environment.CurrentDirectory, "..", "auxi", "Words");
    //Path de donde se aloja
    private DateTime date = File.GetLastWriteTimeUtc(path);
    //Ultima fecha de modificacion ella data la igualdad entre estos

    #endregion
    [DataMember]
    public HashSet<string> Root_words { get; private set; }
    [DataMember]
    public Dictionary<(string, string), double> Tf_Idf_instance { get; private set; }
    [DataMember]
    public HashSet<string> Bug_words { get; private set; }
    [DataMember]

    public Dictionary<string, (List<string>, int)> Control_score { get; private set; }
    [DataMember]

    public Dictionary<(string, string), double> Tf_Idf_last_of_bugs { get; private set; }
    [DataMember]

    public SortedList<string, string> Tildes { get; private set; }
    [DataMember]

    public Dictionary<string, string> Memory { get; private set; }
    [DataMember]

    public Dictionary<int, List<string>> Length_Words { get; private set; }
    [DataMember]

    public Dictionary<(char, int), HashSet<WORDS>> Char_Length { get; private set; }



    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Saves_Info objAsPart = obj as Saves_Info;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public override int GetHashCode()
    {
        return date.GetHashCode();
    }
    public bool Equals(Saves_Info other)
    {
        if (other == null) return false;
        return (this.date.Equals(other.date));
    }
}


[DataContract]

public class Saves_Le : IEquatable<Saves_Le>
{

   


    #region Campo
    const string Name = "Levenstein";
    private static string path = Path.Join(Environment.CurrentDirectory, "..", "auxi", "Words", "Levenstein");
    //Path de donde se aloja
    private DateTime date = File.GetLastWriteTimeUtc(path);
    //Ultima fecha de modificacion ella data la igualdad entre estos

    #endregion

    public Saves_Le(Dictionary<string, HashSet<L_Words>> levenstein_words)
    {

        this.Levenstein_words = levenstein_words;


    }

    [DataMember]
    public Dictionary<string, HashSet<L_Words>> Levenstein_words { get; private set; }


    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Saves_Le objAsPart = obj as Saves_Le;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public override int GetHashCode()
    {
        return date.GetHashCode();
    }
    public bool Equals(Saves_Le other)
    {
        if (other == null) return false;
        return (this.date.Equals(other.date));
    }





}


public class Saves_Doc_Name: IEquatable<Saves_Doc_Name>
{

    #region Campo
    const string Name = "Levenstein";
    private static string path = Path.Join(Environment.CurrentDirectory, "..", "auxi", "Words", "Levenstein");
    //Path de donde se aloja
    private DateTime date = File.GetLastWriteTimeUtc(path);
    //Ultima fecha de modificacion ella data la igualdad entre estos

    #endregion
       

    public Saves_Doc_Name(int documents_Count,HashSet<string> documents_Path,List<string> documents_names,Dictionary<string, List<string>> documents_coincidences,Dictionary<string, int>Doc_name_repe)
    {
          // Auxiliar_Class.Documents_Count ; 
          // Auxiliar_Class.documents_original_names;
          // Auxiliar_Class.Documents_names;
          //  Auxiliar_Class.doc_coincidances;
          //  Auxiliar_Class.doc_name_repe;
        this.Levenstein_words = levenstein_words;


    }

    [DataMember]
    public Dictionary<string, HashSet<L_Words>> Levenstein_words { get; private set; }


    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Saves_Le objAsPart = obj as Saves_Le;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public override int GetHashCode()
    {
        return date.GetHashCode();
    }
    public bool Equals(Saves_Le other)
    {
        if (other == null) return false;
        return (this.date.Equals(other.date));
    }


#endregion


}
*/








