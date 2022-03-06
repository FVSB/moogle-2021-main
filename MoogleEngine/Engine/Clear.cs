

 namespace MoogleEngine;

 public static class Clear_class{
 // Limpia los objetos temporales despues de cada consulta 

 #region Clear public
 public static void Clear()
 {
   Query_Class.Clear_query();

   Tf_Idf_class.Clear_Tf_Idf();

   LevD.Clear();

   Operators.Clear();
   
 }

 #endregion


  

  

 }