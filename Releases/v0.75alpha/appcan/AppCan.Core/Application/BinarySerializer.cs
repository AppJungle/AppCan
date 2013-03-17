/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;



namespace AppCan.Core.Application
{
    /// <summary>
    /// This class is used for serializing objects to binary
    /// </summary>
    public class BinarySerializer :ISerializerAdapter
    {
        BinaryFormatter _serializer = new BinaryFormatter();

        /// <summary>
        /// Serliaze an object to binary to a file
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="objectToSerialize">Object to serialize</param>
        /// <param name="typeOfObjectToSerialize">Type of object to serialize</param>
        /// <returns>true</returns>
        public bool SerializeObject(string fileName, object objectToSerialize, Type typeOfObjectToSerialize)
        {
            Stream stream = File.Open(fileName, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
            
 
            return true;
        }

        /// <summary>
        /// Deserialize an object from a binary file
        /// </summary>
        /// <param name="fileName">The file name to open</param>
        /// <param name="typeOfObjectToDeserialize">The type of object being deserialized</param>
        /// <returns>The deserialized object</returns>
        public object DeserializeObject(string fileName, Type typeOfObjectToDeserialize)
        {

            Stream stream = File.Open(fileName, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            object objectToReturn = bFormatter.Deserialize(stream);
            stream.Close();
            

            return objectToReturn;
        }
    }
}
