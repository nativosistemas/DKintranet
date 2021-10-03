using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace DKintranet.Codigo.clases.Generales
{
    /// <summary>
    /// Realiza una clonación de un objeto de estructura compleja
    /// </summary>
    public static class Clonar
    {
        public static T Copiar<T>(T fuente)
        {
            //Verificamos que sea serializable antes de hacer la copia
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("El tipo de dato debe ser serializable.", "fuente");
            }
            if (Object.ReferenceEquals(fuente, null))
            {
                return default(T);
            }
            //Creamos un stream en memoria
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, fuente);
                stream.Seek(0, SeekOrigin.Begin);
                //Deserializamos la porcón de memoria en el nuevo objeto
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}