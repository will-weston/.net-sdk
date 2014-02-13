using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization;

namespace CTCT.Components
{
    /// <summary>
    /// Base class for components.
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class Component
    {
        /// <summary>
        /// Get the object from JSON.
        /// </summary>
        /// <typeparam name="T">The class type to be deserialized.</typeparam>
        /// <param name="json">The serialization string.</param>
        /// <returns>Returns the object deserialized from the JSON string.</returns>
        public static T FromJSON<T>(string json)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                return (T)ser.ReadObject(ms);
            }
        }

        public static object FromJSON(Stream json, Type type)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
            return serializer.ReadObject(json);
        }

        /// <summary>
        /// Serialize an object to JSON.
        /// </summary>
        /// <returns>Returns a string representing the serialized object.</returns>
        public virtual string ToJSON()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(this.GetType());
                ser.WriteObject(ms, this);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
