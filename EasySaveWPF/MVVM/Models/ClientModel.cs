using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace EasySaveWPF.MVVM.Models
{
    internal class ClientModel
    {
        public List<SaveProfile> saveProfiles = new List<SaveProfile>();
        public List<SaveProfile> newsaveProfiles = new List<SaveProfile>();
        private readonly Socket clientSocket;

        public ClientModel()
        {
            clientSocket = SeConnecter();

            // Start a background task to listen for SaveProfiles continuously
            Task.Run(() => ReceiveSaveProfiles());
        }


        private Socket SeConnecter()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.1.67"), 46154);
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

        private async void ReceiveSaveProfiles()
        {
            try
            {
                while (true) // Keep listening continuously
                {
                    byte[] data = new byte[1024];
                    int size = clientSocket.Receive(data);
                    string json = Encoding.UTF8.GetString(data, 0, size);

                    SaveProfile saveProfile = JsonConvert.DeserializeObject<SaveProfile>(json);

                    // if the SaveProfile name is "RESETLISTTRANSFER", we clear the list
                    if (saveProfile.Name == "RESETLISTTRANSFER")
                    {
                        saveProfiles = newsaveProfiles;
                    }
                    else
                    {
                        // Add the received SaveProfile to the list
                        saveProfiles.Add(saveProfile);
                    }

                    await Task.Delay(5000);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or connection closed
                MessageBox.Show("Erreur lors de la réception des profils : " + ex.Message);
            }
        }

        private void Deconnecter()
        {
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la déconnexion : " + ex.Message);
            }
        }


        // You can expose the saveProfiles list if needed
        public List<SaveProfile> SaveProfiles => saveProfiles;

        private static void Program(string url)
        {
            MessageBox.Show("Programme lancé");
        }
    }
}
