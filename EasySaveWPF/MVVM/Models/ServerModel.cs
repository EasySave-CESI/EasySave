using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveWPF.MVVM.Models
{
    internal class ServerModel
    {
        private static IPEndPoint clientep;
        private static List<SaveProfile> saveProfiles = new List<SaveProfile>();
        private static Socket clientSocket;

        public ServerModel()
        {
            Socket serverSocket = SeConnecter();
            clientSocket = AccepterConnexion(serverSocket);
        }

        private static Socket SeConnecter()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 46154);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipep);
            socket.Listen(10);
            return socket;
        }

        private static Socket AccepterConnexion(Socket socket)
        {
            Socket client = socket.Accept();
            clientep = (IPEndPoint)client.RemoteEndPoint;
            return client;
        }


        private static void Deconnecter(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public void StartSendingMessages()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    SendMessage("Hello from server!"); // Envoyer le message souhaité
                    await Task.Delay(2000); // Attendre 2 secondes
                }
            });
        }

        private static void SendMessage(string message)
        {
            try
            {
                // Convertir le message en tableau d'octets
                byte[] data = Encoding.UTF8.GetBytes(message);

                // Envoyer les données au client
                clientSocket.Send(data);
            }
            catch (Exception ex)
            {
                // Gérer les exceptions liées à l'envoi du message
            }
        }
    }
}
