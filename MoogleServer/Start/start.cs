namespace MoogleServer;
using System.Diagnostics;
public static class Start
{

    public static void Start_Program()
    {
        Stopwatch crono = new Stopwatch();
        crono.Start();
        MoogleEngine.Read.Start();
        System.Console.WriteLine(crono.ElapsedMilliseconds / 1000 + " Finish read");
        MoogleEngine.Sort.Start();
        System.Console.WriteLine(crono.ElapsedMilliseconds / 1000 + " Finish Clasification");
        MoogleEngine.Tf_Idf_class.Start();
        crono.Stop();
        System.Console.WriteLine(crono.ElapsedMilliseconds / 1000 + " Finish Tf_Idf");
        System.Console.WriteLine("All OK :😀");
         MoogleEngine.Re_Start.Start();
         MoogleEngine.Search_Class.Search("((~?)robot hola)");
        
    }



}