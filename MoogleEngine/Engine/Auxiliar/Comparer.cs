using System;
using System.Runtime.Serialization;
namespace MoogleEngine;
//En este .cs estan todos los metodos de comparacion y otas clases auxiliares 
// Si alguna no se usa es porque en proximas actualizaciones se usaran
#region String Length Comparer
public class StringLengthComparer : IComparer<string>
{
    public int Compare(string x, string y)
    {
        if (x.Length > y.Length) 
            return 1;

        if (x.Length < y.Length) 
            return -1;

        return 0;
    }
}
#endregion
#region Word Index
public class Word_Index : IEquatable<Word_Index>
{  
     public Word_Index(string word,int index)
    {
       this.Word=word;
       this.Index=index;
    }

     public string Word{get;set;}
     public  int   Index{get;set;}


      public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Word_Index objAsPart = obj as Word_Index ;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public override int GetHashCode()
    {
        return Word.GetHashCode();
    }
    public bool Equals(Word_Index other)
    {
        if (other == null) return false;
        return (this.Word.Equals(other.Word));
    }
   

}

public class WORDS_INDEX_COMPARER:IComparer<Word_Index>
{

    public int Compare(Word_Index x,Word_Index y)
    {
        if (x.Index > y.Index) 
            return 1;

        if (x.Index < y.Index) 
            return -1;

        return 0;
    }




}





#endregion

#region Up_Score
public class Up_Score : IEquatable<Up_Score>
{
     public Up_Score(string word,float count)
    {
        this.Word=word;
        this.count=count;
    }

    public string Word { get; set; }
    public float count { get; set; }

    
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Up_Score objAsPart = obj as Up_Score;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public override int GetHashCode()
    {
        return Word.GetHashCode();
    }
    public bool Equals(Up_Score other)
    {
        if (other == null) return false;
        return (this.Word.Equals(other.Word));
    }
    
}

public class Up_ScoreComparerScores : IComparer<Up_Score>
{
    public int Compare(Up_Score x, Up_Score y)
    {
        
        if (x.count > y.count) 
            return 1;

        if (x.count< y.count) 
            return -1;

        return 0;
    }
}
#endregion
#region Words
public class WORDS : IEquatable<WORDS>
{
     public WORDS(string word)
    {
       this.Word=word;
    }

    public string Word { get; set; }
    
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        WORDS objAsPart = obj as WORDS ;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public override int GetHashCode()
    {
        return Word.GetHashCode();
    }
    public bool Equals(WORDS other)
    {
        if (other == null) return false;
        return (this.Word.Equals(other.Word));
    }
   
}
#endregion
#region Score Texts
public class Up_Scores_Texts : IEquatable<Up_Scores_Texts>
{

     public Up_Scores_Texts(string document,double score)
    {
       this.Document=document;
       this.Score=score;
    }

    public string Document { get; set; }
    public double Score { get; set; }

    
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Up_Scores_Texts objAsPart = obj as Up_Scores_Texts;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public override int GetHashCode()
    {
        return Document.GetHashCode();
    }
    public bool Equals(Up_Scores_Texts other)
    {
        if (other == null) return false;
        return (this.Document.Equals(other.Document));
    }
   
}


public class Up_Scores_Texts_Score_Comparer : IComparer<Up_Scores_Texts>
{
    public int Compare(Up_Scores_Texts x, Up_Scores_Texts y)
    {
        
        if (x.Score > y.Score) 
            return 1;

        if (x.Score< y.Score) 
            return -1;

        return 0;
    }
}


#endregion


#region  TUPPLEs
public class Tupples:IEquatable<Tupples>
{
    public Tupples(string root_word,string derivate,int length)
    {
        this.Root_Word=root_word;
        this.Derivate=derivate;
        this.Length=length;
    }


    public string Root_Word { get; set; }
    public string Derivate { get; set; }
    public int Length { get; set; }

     public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Tupples objAsPart = obj as Tupples;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public override int GetHashCode()
    {
        return Derivate.GetHashCode();
    }
    public bool Equals(Tupples other)
    {
        if (other == null) return false;
        return (this.Derivate.Equals(other.Derivate));
    }
}

public class TupplesLengthComparer : IComparer<Tupples>
{
    public int Compare(Tupples x, Tupples y)
    {
        
        if (x.Length > y.Length) 
            return 1;

        if (x.Length< y.Length) 
            return -1;

        return 0;
    }
}

#endregion

#region Levenshtein words  Para determinar el score 

public class L_Words:IEquatable<L_Words>
{
    public L_Words(string word,float score)
    {  
        this.Word=word;
        this.Score=score;
        
    }


    public string Word{ get;private set; }
   
    public float Score{get;private set;} 

     public override bool Equals(object obj)
    {
        if (obj == null) return false;
        L_Words objAsPart = obj as L_Words;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public override int GetHashCode()
    {
        return Word.GetHashCode();
    }
    public bool Equals(L_Words other)
    {
        if (other == null) return false;
        return (this.Word.Equals(other.Word));
    }
}

public class L_Words_Score_Comparer : IComparer<L_Words>
{
    public int Compare(L_Words x, L_Words y)
    {
        
        if (x.Score > y.Score) 
            return -1;

        if (x.Score< y.Score) 
            return 1;
        if (x.Score==y.Score)
        {
           Random random=new Random();
           int r =random.Next(1,2);   //Si tiene = L_Dis que lo determine la suerte
           if (r==1)
           {
               return 1;
           } 
        }
        return -1;
    }
}




#endregion
