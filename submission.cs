using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Program
    {
        public static string xmlURL      = "https://josht13.github.io/cse445_a4/Hotels.xml";
        public static string xmlErrorURL = "https://josht13.github.io/cse445_a4/HotelsErrors.xml";
        public static string xsdURL      = "https://josht13.github.io/cse445_a4/Hotels.xsd";

        public static void Main(string[] args)
        {
            try
            {
                string result = Verification(xmlURL, xsdURL);
                Console.WriteLine(result);

                result = Verification(xmlErrorURL, xsdURL);
                Console.WriteLine(result);

                result = Xml2Json(xmlURL);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        // Q2.1  — validate XML against XSD
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            var errors = new List<string>();

            void OnValidation(object sender, ValidationEventArgs e)
            {
                string location = "";
                if (e.Exception is XmlSchemaException xse && xse.LineNumber > 0)
                    location = $" (Line {xse.LineNumber}, Pos {xse.LinePosition})";
                errors.Add($"{e.Severity}: {e.Message}{location}");
            }

            try
            {
                XmlSchemaSet schemas = new XmlSchemaSet();
                using (XmlReader schemaReader = XmlReader.Create(xsdUrl))
                {
                    schemas.Add(null, schemaReader);
                }

                XmlReaderSettings settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema,
                    Schemas = schemas,
                    DtdProcessing = DtdProcessing.Prohibit
                };
                settings.ValidationEventHandler += OnValidation;

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { /* validation triggers here */ }
                }

                if (errors.Count == 0)
                    return "No errors are found";
                else
                    return string.Join(Environment.NewLine, errors);
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }

        // Q2.2  — convert valid XML to JSON
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                using (XmlReader reader = XmlReader.Create(xmlUrl, new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Prohibit
                }))
                {
                    doc.Load(reader);
                }

                // serialize to JSON
                string jsonText = JsonConvert.SerializeXmlNode(doc, Formatting.None, true);

                // replace "@Rating" with "_Rating" to match assignment format
                jsonText = jsonText.Replace("\"@Rating\":", "\"_Rating\":");

                // confirm JSON is deserializable (autograder check)
                JsonConvert.DeserializeXmlNode(jsonText);

                return jsonText;
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
    }
}


}

