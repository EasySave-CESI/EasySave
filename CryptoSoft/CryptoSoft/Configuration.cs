using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CryptoSoft
{
    internal class Configuration
    {
        public string Key;

        public Configuration(string path) 
        {
            if (new System.IO.FileInfo(path).Length == 0) { CreateConfiguration(path); }
            Key = LoadConfiguration(path);
        }

        public string LoadConfiguration(string path)
        {
            // Load config file
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNodeList keyNode = doc.GetElementsByTagName("key");
            string Key = keyNode[0].InnerText;
            doc.Save(path);
            return Key;
        }

        public void CreateConfiguration(string path)
        {
            // Create config file
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode configNode = doc.CreateElement("config");
            doc.AppendChild(configNode);

            XmlNode keyNode = doc.CreateElement("key");
            keyNode.InnerText = Key;
            configNode.AppendChild(keyNode);
            doc.Save(path);
        }

        public void ModifyKey(string path)
        {
            // Load config file
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNodeList keyNode = doc.GetElementsByTagName("key");
            keyNode[0].InnerText = Key;

            doc.Save(path);
        }
    }
}
