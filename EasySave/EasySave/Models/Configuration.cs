using System.Xml;
using System;

namespace EasySave.Models
{
    public class Configuration
    {
        public string configFilePath { get; set; }
        public string language { get; set; }
        public string logFormat { get; set; }

        public Configuration(string path)
        {
            this.configFilePath = path;

            List<string> param = LoadConfig(path);
            this.language = param[0];
            this.logFormat = param[1];
        }

        public static List<string> LoadConfig(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    CreateConfigFile(filePath);
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlNodeList appSettingsNodes = doc.SelectNodes("/configuration/appSettings/add");

                List<string> param = new List<string>();

                foreach (XmlNode node in appSettingsNodes)
                {
                    string key = node.Attributes["key"].Value;
                    string value = node.Attributes["value"].Value;

                    if (key == "language")
                    {
                        param.Add(value);
                    }
                    else if (key == "logformat")
                    {
                        param.Add(value);
                    }
                }

                return param;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void CreateConfigFile(string configFilePath)
        {
            try
            {
                // Create a new file in the configFilePath
                XmlDocument doc = new XmlDocument();
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);

                doc.AppendChild(docNode);

                XmlNode configurationNode = doc.CreateElement("configuration");
                doc.AppendChild(configurationNode);

                XmlNode appSettingsNode = doc.CreateElement("appSettings");
                configurationNode.AppendChild(appSettingsNode);

                XmlNode addNode = doc.CreateElement("add");
                XmlAttribute keyAttribute = doc.CreateAttribute("key");
                keyAttribute.Value = "language";
                XmlAttribute valueAttribute = doc.CreateAttribute("value");
                valueAttribute.Value = "en";
                addNode.Attributes.Append(keyAttribute);
                addNode.Attributes.Append(valueAttribute);
                appSettingsNode.AppendChild(addNode);

                addNode = doc.CreateElement("add");
                keyAttribute = doc.CreateAttribute("key");
                keyAttribute.Value = "logformat";
                valueAttribute = doc.CreateAttribute("value");
                valueAttribute.Value = "json";
                addNode.Attributes.Append(keyAttribute);
                addNode.Attributes.Append(valueAttribute);
                appSettingsNode.AppendChild(addNode);

                doc.Save(configFilePath);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static void WriteConfig(string filePath, string language, string logFormat)
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
    }
}
