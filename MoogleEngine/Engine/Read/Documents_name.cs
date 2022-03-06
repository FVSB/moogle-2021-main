namespace MoogleEngine;
public class Documents_Names
{

 
    #region Devolver nombre del documento
    public static string Documents_Name(string path)
    {
        string temp=Path.GetFileNameWithoutExtension(path);

        return temp;
    }













    #endregion


}
