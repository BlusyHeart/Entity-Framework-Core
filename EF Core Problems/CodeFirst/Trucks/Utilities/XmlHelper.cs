using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Trucks.Utilities
{
    public class XmlHelper
    {
        public string Serialize<T>(string root, T obj)
        {
            XmlRootAttribute rootAttribute = new XmlRootAttribute(root);

            StringBuilder output = new StringBuilder();

            XmlSerializer xmlSerializer =
               new XmlSerializer(typeof(T), rootAttribute);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(output);
            xmlSerializer.Serialize(writer, obj, namespaces);

            return output.ToString().TrimEnd();

        }


        public T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), xmlRoot);

            using StringReader sr = new StringReader(inputXml);

            T suppliersDTOs = (T)xmlSerializer.Deserialize(sr);

            return suppliersDTOs;
        }

        public IEnumerable<T> DeserializeCollection<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T[]), xmlRoot);

            using StringReader sr = new StringReader(inputXml);

            T[] suppliersDTOs = (T[])xmlSerializer.Deserialize(sr);

            return suppliersDTOs;
        }
    }
}
