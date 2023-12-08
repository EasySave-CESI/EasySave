using System;
using System.Collections.Generic;
using System.Xml;

namespace EasySaveConsoleApp
{
    public class Configuration
    {
        public string configFilePath { get; set; }
        public string language { get; set; }
        public string logFormat { get; set; }

        public Configuration(string path)
        {
            this.configFilePath = path;

            List<string> param = LoadConfiguration(path);
            this.language = param[0];
            this.logFormat = param[1];
        }

        public static List<string> LoadConfiguration(string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    Console.WriteLine("Configuration file not found, creating a new one...");
                    // Create a new file
                    XmlDocument document = new XmlDocument();

                    // Create the root element
                    XmlElement root = document.CreateElement("configuration");
                    document.AppendChild(root);

                    // Create the appSettings element
                    XmlElement appSettings = document.CreateElement("appSettings");
                    root.AppendChild(appSettings);

                    // Create the language element
                    XmlElement language = document.CreateElement("add");
                    language.SetAttribute("key", "language");
                    language.SetAttribute("value", "en");
                    appSettings.AppendChild(language);

                    // Create the logFormat element
                    XmlElement logFormat = document.CreateElement("add");
                    logFormat.SetAttribute("key", "logformat");
                    logFormat.SetAttribute("value", "json");
                    appSettings.AppendChild(logFormat);

                    // Save the document to the specified path
                    document.Save(filePath);
                    return new List<string> { "en", "json" };
                }
                else
                {
                    List<string> param = new List<string>();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);

                    XmlNodeList appSettingsNodes = doc.SelectNodes("/configuration/appSettings/add");

                    foreach (XmlNode node in appSettingsNodes)
                    {
                        string key = node.Attributes["key"].Value;
                        string value = node.Attributes["value"].Value;

                        param.Add($"{value}");
                    }

                    return param;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public static void SetConfiguration(string filePath, string language, string logFormat)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlNodeList appSettingsNodes = doc.SelectNodes("/configuration/appSettings/add");

                foreach (XmlNode node in appSettingsNodes)
                {
                    string key = node.Attributes["key"].Value;
                    string value = node.Attributes["value"].Value;

                    if (key == "language")
                    {
                        node.Attributes["value"].Value = language;
                    }
                    else if (key == "logformat")
                    {
                        node.Attributes["value"].Value = logFormat;
                    }
                }

                doc.Save(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static Dictionary<string, string> GetDictPrintStrings(string filePath)
        {
            try
            {
                Dictionary<string, string> printStrings = new Dictionary<string, string>();
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlNodeList stringsNodes = doc.SelectNodes("/printStrings/add");

                foreach (XmlNode node in stringsNodes)
                {
                    string key = node.Attributes["key"].Value;
                    string value = node.Attributes["value"].Value;

                    printStrings.Add($"{key}", $"{value}");
                }
                return printStrings;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

    }
}
