using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Program
    {
        public static string xmlURL = "https://josht13.github.io/cse445_a4/Hotels.xml";
        public static string xmlErrorURL = "https://josht13.github.io/cse445_a4/HotelsErrors.xml";
        public static string xsdURL = "https://josht13.github.io/cse445_a4/Hotels.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            errors.Clear();

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(null, xsdUrl);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = schemas;
            settings.ValidationEventHandler += ValidationEvent;

            using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
            {
                while (reader.Read()) { }
            }

            if (errors.Count == 0)
                return "No errors are found";
            else
                return string.Join(Environment.NewLine, errors);
        }

        private static List<string> errors = new List<string>();

        private static void ValidationEvent(object sender, ValidationEventArgs e)
        {
            errors.Add(e.Message);
        }

        // Q2.2
        public static string Xml2Json(string xmlUrl)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlUrl);

            string jsonText = JsonConvert.SerializeXmlNode(doc);
            jsonText = jsonText.Replace("\"@Rating\":", "\"_Rating\":");
            return jsonText;
        }
    }
}



