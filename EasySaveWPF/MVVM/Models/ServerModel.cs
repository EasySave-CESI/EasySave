﻿using Newtonsoft.Json;
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
        public List<SaveProfile> saveProfiles = new List<SaveProfile>();
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

        public void StartSendingProfiles()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    foreach (SaveProfile profile in saveProfiles)
                    {
                        SendProfile(profile);
                        await Task.Delay(200);
                    }
                    // Ajoutez une délimitation entre les objets
                    SendProfileDelimiter();
                    await Task.Delay(200);
                }
            });
        }

        private static void SendProfileDelimiter()
        {
            try
            {
                // Envoyez une délimitation au client
                byte[] delimiter = Encoding.UTF8.GetBytes("\n");
                clientSocket.Send(delimiter);
            }
            catch (Exception ex)
            {
                // Gérez les exceptions liées à l'envoi de la délimitation
            }
        }

        private static void SendProfile(SaveProfile profile)
        {
            try
            {
                // Sérialisez l'objet SaveProfile en JSON
                string json = JsonConvert.SerializeObject(profile);
                byte[] data = Encoding.UTF8.GetBytes(json);

                // Envoyez les données au client
                clientSocket.Send(data);
            }
            catch (Exception ex)
            {
                // Gérez les exceptions liées à l'envoi du profil
            }
        }
    }
}
