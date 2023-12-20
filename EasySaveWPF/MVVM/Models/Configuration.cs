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
        public string Theme { get; set; }
        public string MaxFileSize { get; set; }

        public Configuration(string path)
        {
            ConfigFilePath = path;

            Dictionary<string, string> parameters = LoadConfig(path);

            if (parameters != null && parameters.ContainsKey("language") && parameters.ContainsKey("logformat") && parameters.ContainsKey("theme") && parameters.ContainsKey("maxfilesize"))
            {
                Language = parameters["language"];
                LogFormat = parameters["logformat"];
                Theme = parameters["theme"];
                MaxFileSize = parameters["maxfilesize"];
            }
            else
            {
                Language = "en";
                LogFormat = "json";
                Theme = "light";
                MaxFileSize = "1000000";
            }
        }

        public static Dictionary<string, string> LoadConfig(string filePath)
        {
            try
            {
                // Check if the filePath is null
                if (filePath == null)
                {
                    Console.WriteLine("Error: filePath is null");
                    return new Dictionary<string, string>();
                }

                if (!File.Exists(filePath)) { CreateConfigFile(filePath); }

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlNodeList appSettingsNodes = doc.SelectNodes("/configuration/appSettings/add");

                if (appSettingsNodes == null) { return null; } // Handle the case where appSettingsNodes is null

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                foreach (XmlNode node in appSettingsNodes)
                {
                    string key = node.Attributes["key"].Value;
                    string value = node.Attributes["value"].Value;

                    if (key == "language" || key == "logformat" || key == "theme" || key == "maxfilesize")
                    {
                        parameters.Add(key, value);
                    }
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
                AddAppSettingNode(doc, configurationNode, "theme", "light");
                AddAppSettingNode(doc, configurationNode, "maxfilesize", "0");

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

        public static void WriteConfig(string filePath, string newlanguage, string newlogFormat, string newtheme, string newmaxFileSize)
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

                        if (key == "language")
                        {
                            node.Attributes["value"].Value = newlanguage;
                        }
                        else if (key == "logformat")
                        {
                            node.Attributes["value"].Value = newlogFormat;
                        }
                        else if (key == "theme")
                        {
                            node.Attributes["value"].Value = newtheme;
                        }
                        else if (key == "maxfilesize")
                        {
                            node.Attributes["value"].Value = newmaxFileSize;
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
