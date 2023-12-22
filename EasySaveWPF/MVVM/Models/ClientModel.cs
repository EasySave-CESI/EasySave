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
    }
}
