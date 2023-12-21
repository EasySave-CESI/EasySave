using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveWPF.MVVM.Models
{
    internal class ServerModel
    {
        static IPEndPoint clientep;

        static void Main()
        {
            Socket serverSocket = SeConnecter();
            Socket clientSocket = AccepterConnexion(serverSocket);
            EcouterReseau(clientSocket);
            Deconnecter(serverSocket);
        }

        private static Socket SeConnecter()
        {
            Console.OutputEncoding = Encoding.UTF8;
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

        private static void EcouterReseau(Socket client)
        {
            try
            {
                // Create a SaveProfile object
                SaveProfile saveProfile = new SaveProfile("ExampleName", "ExampleSourcePath", "ExampleTargetPath", "ExampleState", 10, 1024, 5, 50, "ExampleType");

                // Serialize the SaveProfile object to JSON
                string json = JsonConvert.SerializeObject(saveProfile);
                byte[] data = Encoding.UTF8.GetBytes(json);

                // Send the serialized data
                client.Send(data);
            }
            catch (Exception)
            {
                //
            }
        }



        private static void Deconnecter(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
