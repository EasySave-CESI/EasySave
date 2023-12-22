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
        private static List<SaveProfile> saveProfiles = new List<SaveProfile>();
        private static Socket clientSocket;

        public ServerModel()
        {
            Socket serverSocket = SeConnecter();
            clientSocket = AccepterConnexion(serverSocket);
            StartSendingSaveProfiles();
            Deconnecter(serverSocket);
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

        private static void StartSendingSaveProfiles()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        SendSaveProfiles(saveProfiles);
                    }

                    System.Threading.Thread.Sleep(500);
                }
            });
        }



        public static void SendSaveProfiles(List<SaveProfile> newsaveProfiles)
        {
            try
            {
                saveProfiles = newsaveProfiles;
                while (true)
                {
                    foreach (SaveProfile saveProfile in saveProfiles)
                    {
                        // Serialize the SaveProfile object to JSON
                        string json = JsonConvert.SerializeObject(saveProfile);
                        byte[] data = Encoding.UTF8.GetBytes(json);

                        // Send the serialized data
                        clientSocket.Send(data);
                    }

                    // Add a delay between sending SaveProfiles
                    System.Threading.Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
                //
            }
        }

        private static void Deconnecter(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public static void AddSaveProfile(SaveProfile saveProfile)
        {
            // Add a SaveProfile to the list
            saveProfiles.Add(saveProfile);
        }
    }
}