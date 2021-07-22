using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace API.Helpers
{
    public class ObjectSerializer
    {
        public static string Serialize<T>(T typeObj, XmlWriterSettings settings = null)  
        {  
            if (typeObj == null)  
            {  
                return string.Empty;  
            }  
            try  
            {  
                using (var stringWriter = new StringWriter())  
                {  
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))  
                    {  
                        var serializer = new XmlSerializer(typeof(T));  
                        serializer.Serialize(xmlWriter, typeObj);  
                        return stringWriter.ToString();  
                    }  
                }  
            }  
            catch  
            {  
                return string.Empty;  
            }  
        } 
        
    }
}