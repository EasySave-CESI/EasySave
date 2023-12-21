using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EasySaveWPF.MVVM.ViewModels;
using Newtonsoft.Json;


namespace EasySaveWPF.MVVM.Models
{
    internal class ClientModel // This class will be used to communicate with the server via sockets
    {
        List<SaveProfile> saveProfiles = new List<SaveProfile>();

        public ClientModel()
        {
            Socket clientSocket = SeConnecter();
            EcouterReseau(clientSocket);
            Deconnecter(clientSocket);
        }

        private static Socket SeConnecter()
        {
            Console.OutputEncoding = Encoding.UTF8;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 46154);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(ipep);
                MessageBox.Show("Connecté au serveur");
                return socket;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de se connecter au serveur");
                return null;
            }
        }

        private static void EcouterReseau(Socket client)
        {
            try
            {
                byte[] data = new byte[1024];
                int size = client.Receive(data);
                string json = Encoding.UTF8.GetString(data, 0, size);

                SaveProfile saveProfile = JsonConvert.DeserializeObject<SaveProfile>(json);

                MessageBox.Show("Nom : " + saveProfile.Name + "\nSource : " + saveProfile.SourceFilePath + "\nTarget : " + saveProfile.TargetFilePath + "\nType : " + saveProfile.TypeOfSave + "\nState : " + saveProfile.State + "\nTotalFilesToCopy : " + saveProfile.TotalFilesToCopy + "\nTotalFilesSize : " + saveProfile.TotalFilesSize + "\nNbFilesLeftToDo : " + saveProfile.NbFilesLeftToDo + "\nProgression : " + saveProfile.Progression);

            }
            catch (Exception)
            {
                // 
            }
        }


        private static void Deconnecter(Socket socket)
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception)
            {
                // 
            }
        }

        private static void Program(string url)
        {
            MessageBox.Show("Programme lancé");
        }
    }
}
