using System.Windows;
namespace MoogleEngine;
public class Vectorizer {

  //Vectorizar mis documentos
  
  
 
  #region Normalize
   // Normailar el vector
   //  Logitud en el plano 
  
   double Normalize( double[] Tf_idf)
   {     
         double sum=0;
        for (int i = 0; i < Tf_idf.Length; i++)
        {
            sum+=Math.Pow(2,Tf_idf[i]);
            
        }  
        return Math.Sqrt(sum);                                         
   }





  #endregion

  #region Dot
  

                //Tf_idf Text        Query
      double Dot(double[]Text,double[]query)
    {
        double sum=0;
        for (int i = 0; i <Text.Length; i++)
        {
            sum+=Text[i]*query[i];
        }
        return sum;
    }

  #endregion

#region Similitud coseno
  private double Sim_cos(double[]Text,double[]query)
  { 
      return Dot(Text,query)/(Normalize(Text)*Normalize(query));

  }

 #endregion

 public static double score(double[] query, double[] text)
 {
     Vectorizer v=new Vectorizer();
     return v.Sim_cos(text,query);


 }
  
 
    







}