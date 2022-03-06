#Moogle

# Moogle 
 Lista de instrucciones como Usar Moogle 0.02 
 
 Si quiere realizar una consulta cualquiera usted debe introducir en el recuadro  
 la palabra / oracion a buscar. 
 
 Siguiendo esta jerarquía será devuelta su consulta.
 
 Primero que la/s palabra/s se encuentren en el texto.
 
 °En este caso dado la relevancia de ellas en la consulta (en caso de ser 1 palabra sera el maximo) sera determinado por la similitud que tenga este vector consulta con su imagen 
 en cada documento. 
 
 En caso de no encontrarse la palabra o su raíz (casos de mas de 7 caracteres) se procede a buscar la de mayor coincidencia en el corpus (en el apartado de sugerencia se le informara que esta fue utilizada) y se le rebajara el score de esta en relación  
 con lo la simulitud de la palabra de la consulta 
 
 *Próximamente se añadirá la opción  para sinónimos y no aparecer en ese texto *  
 
 Para establecer el score por el cual se rige el orden de aparición de los resultados 
 se utiliza el Tf_Idf herramienta esencial, por ejemplo el Tf es el porcentaje de relevancia de la palabra en términos de apariciones y el Idf controla el nivel de rareza de esta en el corpus como ¿como? Pues muy fácil dado la cantidad de documentos y en la la de  apariciones que esta tiene, en cada uno se establece una proporcion para ello utilizamos el poder de la función logaritmo ya que mientas menor sea el cociente de la proporcion anterior mayor sera la "rareza" de la palabra en el corpus. Despues estos dos numeros se multiplican ya que su producto es el balance casi ideal. 
 
 Despues de tener estos datos ¿Como establecemos la coincidencia? 
 
 Pues de las clases de algebra lineal se conoce el contenido de espacios lineales  y las aplicaciones lineales tomando las palabras como nuestro espacio vectorial 
 y la consulta como el vector consulta establecemos una aplicacion lineal que nos da la coincidencia del score de la consulta con el de su imagen en los textos. Acá entra la función coseno ¿por qué? Muy fácil ya que necesitamos una funcion que mientras mas cercano estén dos vectores (la cercanía será componente a componente) mayor sea su valor. 
 Ademas para ello se normaliza el vector que no es más que su longitud en base a su score y el de su reflejo.  
 Y por último se realiza la organización de mayor a menor (por su score) y se procede a la entrega de los resultados. 
 
 ¿Qué pasa si quiero utilizar un operador? 
  
 En principio la búsqueda se realizará con normalidad  
 
 (para la utilización de este la palabra/s a cuantificar deben estar entre paréntesis y dentro de ellas también dentro de parentisis el operador de esta forma: 
 
  
      "(*operador*)*palabra/s*)"   
       
      ojo no poner operadores contradictorios, la busqueda solo ofrecera el ultimo operador
       Ejemplo ((!^)robot) apareceran solo los textos que la contengan  
 
 Por ejemplo si tenemos en nuestro corpus el famoso libro "Yo Robot" estamos seguro que la palabra "robot" existirá en él como no queremos que aparezca en los resultados escribimos: 
 ((!)robot)... en principio la búsqueda se ejecuta sin ninguna variación, aunque en la clase Operator ya se ha reconocido el uso de este operador (el cuál es: no quiero que exista en mis respuestas un documento con esa palabra) antes de entregar nuestra respuesta el metodo :  
 
 cs ```
  static void Dont_Exits(string query) 
    { 
          Textos_no_resultados.AddRange(Where(query,false).Item1); 
    } 
    ```
   asistido por el método Where añadirá a una lista negra de textos el texto "Yo Robot" 
   el cual sera removido de la respuesta final. 
 
   En caso de que sea estricto la cuantificación de las palabras dentro del parentesis en el lado del cuantificador se debe añadir *?*  si la búsqueda es ((^?)robot hola) solo aparecerán documentos que los contengan a los dos. 
 
   Para el operador (*) el efecto de este se hará por la función Control_Score que en el momento que estemos estableciendo la matriz de la aplicación lineal en la componente donde este dicha palabra se le multiplicara por "1.25" el score original (es acumulativo el uso de este n veces será "1.25xn").

   Es probable que exista la pregunta ¿ Por qué procesamos toda la búsqueda y después eliminamos resultados? 
 
   Pues.... ya que siempre se quiere mejorar el código, insertar y eliminar cosas pues es mas cómodo de esta forma poder insertar más operadores para poder utilizar más de uno en una cuantificación y la forma en que se pueda implementar es mas fácil y rápida  además que es  preferible dejar un "núcleo" del programa lo más "pura" posible y por último en la forma en la que fue (y esta siendo) construido este programa tiene la intensión de ser lo mas modular posible (onion_form (espero que no salte el copyright con Tor)). 
 
   ¿Que se espera para las próximas actualizaciones? 
 
   °La introducción de los sinónimos, la implementación de estos será clave en dos puntos 
     
    en caso de no encontrarse la palabra raíz o su derivada en el texto se utilizará el sinónimo con un score menor en función de su similitud. 
    
    en caso de existir sinónimos en el vector texto pues se normalizara el score de la palabra y todos sus sinónimos para asi dar mas relevancia a dicho texto. 
 
   introducción de nuevos operadores como: 
   -búsqueda de distancia mínima donde devuelve ordenado descendentemente los documentos que menor distantica tienen entre las palabras de la consulta (difiere en el anterior ya que en este caso no es la norma sino el menor absoluto) para ello se implementará  métodos recursivos semejantes al problema de la mochila. 
 
   -uso de historial y clasificadores  cuando se inicie cada búsqueda se introducirá inicialmente el nombre del usuario en base a su historial se ajustara el criterio de semejanza para la consulta y los clasificadores existirá un .txt donde estara clasificado cada documento por su contenido ejemplo: novela, naturaleza, educativo; 
   introduciendo el nombre de este se aumentara el score del texto que se encuentre en dicha sección además (Si da tiempo) en base a las palabras con mayor score calificar los textos. 
 
   °La joya de la corona :Serializaciòn del contenido. 
  _ (Espero entregar este lo mas rápido posible) 
   -La idea es disminuir el tiempo de carga ya que una vez clasificado los archivos es inútil volver a tener que  hacer el mismo proceso cada vez que se carga el problema y el otro es también disminuir el consumo de memoria RAM. 
   
   (Hasta poder cambiar archivos en medio de la  ejecución) 
 
   Para la serialización se utilizará la biblioteca: 
 
   <using System.Runtime.Serialization.Json;> 
 
   Donde los datos primeramente se guardaran en unas clases publicas como esta: 
 
```cs
 
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
 ```
  
 Y cada objeto se serializará con un método como este 
 
      
 ```cs
 public static void Serialize(Stream stream,Saves_Doc save)  
    { 
 
            DataContractJsonSerializer d = new DataContractJsonSerializer(typeof(Saves_Doc)); 
 
            d.WriteObject(stream,save); 
 
    } 
```
  
    
 
  Estos se guardaran en archivos.dll  
 
  Para la recuperar la informacion se utilizara el mismo objeto pero el metodo: 
 
  cs``
     Stream x=new FileStream("../auxi/saves.dll",FileMode.OpenOrCreate); 
   Saves_Doc saves=(Saves_Doc) d.ReadObject(x); 
  ``
 
 Luego donde en cada inicio de del programa se comprueba que sean los mismos documentos y no se hayan modificado para ello se guardará la fecha de la ultima modificación en horario UTC. 
 
 En caso de haber una modificación: 
 
 Eliminación del archivo: Se elimina toda la info de dicho archivo y se procede a re-calcular el Tf_Idf 
 
 Adición se concatena la informacion ya obtenida con la anterior (previo re-cálculo  del nuevo Tf_Idf). 
 
 Modificación se considera una eliminación y posterior adicción. 
 
 ¿Qué pasa si se modifica mientas está en ejecución el programa? 
 
 Pues cada 5 segundos comprueba si ha habido modificación en caso afirmativo la página entrará en modo recarga y reproducirá una cancion instrumental 
 
 ¿Como ahorra memoria ram?  
  
 Esta es la parte compleja como si de un reloj de alta gama se tratara, pues hay que programar los momentos exactos para la deserialización del contenido en función de la necesidad del programa en ese momento. 
 
(En desarrollo....)
 
 
 
 Hasta aquí es la version actual de Moogle. 
 
 Muchas gracias buen día.   
