/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppCan.Core.Application
{
    /// <summary>
    /// Interface to for serializers to derive from, so that we can swap specific serializer implementations if needed.
    /// </summary>
    public interface ISerializerAdapter
    {
        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="objectToSerialize"></param>
        /// <param name="typeOfObjectToSerialize"></param>
        /// <returns></returns>
        bool SerializeObject(string fileName, object objectToSerialize, Type typeOfObjectToSerialize);

        /// <summary>
        /// Deserialize an object
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="typeOfObjectToDeserialize"></param>
        /// <returns></returns>
        object DeserializeObject(string fileName, Type typeOfObjectToDeserialize);
    }
}
