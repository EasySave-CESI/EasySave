using EasySaveWPF.MVVM.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace EasySaveWPF.MVVM.Models
{
    public class Configuration
    {
        public string ConfigFilePath { get; set; }
        public string Language { get; set; }
        public string LogFormat { get; set; }

        public Configuration(string path)
        {
            ConfigFilePath = path;

            Dictionary<string, string> parameters = LoadConfig(path);

            if (parameters != null && parameters.ContainsKey("language") && parameters.ContainsKey("logformat"))
            {
                Language = parameters["language"];
                LogFormat = parameters["logformat"];
            }
            else
            {
                Language = "en";
                LogFormat = "json";
            }
        }

        public Configuration()
        {
        }

        public static Dictionary<string, string> LoadConfig(string filePath)
        {
            try
            {
                if (filePath == null || !File.Exists(filePath)) { CreateConfigFile(filePath); }

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlNodeList appSettingsNodes = doc.SelectNodes("/configuration/appSettings/add");

                if (appSettingsNodes == null) { return null; } // Handle the case where appSettingsNodes is null

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                foreach (XmlNode node in appSettingsNodes)
                {
                    string key = node.Attributes["key"].Value;
                    string value = node.Attributes["value"].Value;

                    if (key == "language" || key == "logformat") { parameters[key] = value; }
                }

                doc.Save(filePath);

                return parameters;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public static void CreateConfigFile(string configFilePath)
        {
            try
            {
                if (configFilePath == null)
                {
                    Console.WriteLine("Error: configFilePath is null");
                    return;
                }

                XmlDocument doc = new XmlDocument();
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlNode configurationNode = doc.CreateElement("configuration");
                doc.AppendChild(configurationNode);

                AddAppSettingNode(doc, configurationNode, "language", "en");
                AddAppSettingNode(doc, configurationNode, "logformat", "json");

                doc.Save(configFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private static void AddAppSettingNode(XmlDocument doc, XmlNode parentNode, string key, string value)
        {
            XmlNode appSettingsNode = doc.CreateElement("appSettings");
            parentNode.AppendChild(appSettingsNode);

            XmlNode addNode = doc.CreateElement("add");
            XmlAttribute keyAttribute = doc.CreateAttribute("key");
            keyAttribute.Value = key;
            XmlAttribute valueAttribute = doc.CreateAttribute("value");
            valueAttribute.Value = value;
            addNode.Attributes.Append(keyAttribute);
            addNode.Attributes.Append(valueAttribute);
            appSettingsNode.AppendChild(addNode);
        }

        public static void WriteConfig(string filePath, string language, string logFormat)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlNodeList appSettingsNodes = doc.SelectNodes("/configuration/appSettings/add");

                if (appSettingsNodes != null)
                {
                    foreach (XmlNode node in appSettingsNodes)
                    {
                        string key = node.Attributes["key"].Value;
                        string value = node.Attributes["value"].Value;

                        if (key == "language" || key == "logformat")
                        {
                            XmlAttribute attribute = node.Attributes["value"];
                            if (attribute != null) { attribute.Value = (key == "language") ? language : logFormat; }
                        }
                    }
                }
                doc.Save(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static implicit operator Configuration(ConfigurationViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}
