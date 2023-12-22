using EasySaveWPF.MVVM.Models;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows;

internal class ClientModel
{
    public List<SaveProfile> saveProfiles = new List<SaveProfile>();
    private readonly Socket clientSocket;
    public string ServerIP { get; set; }

    public ClientModel(string serverIp)
    {
        ServerIP = serverIp;
        clientSocket = SeConnecter();
        ReceiveProfiles();
    }

    private Socket SeConnecter()
    {
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ServerIP), 46154);
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            socket.Connect(ipep);
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

    public void ReceiveProfiles()
    {
        Task.Run(() =>
        {
            while (true)
            {
                byte[] data = new byte[1024];
                int size = clientSocket.Receive(data);
                string json = Encoding.UTF8.GetString(data, 0, size);
                if (json == "\n")
                {
                    // Si une délimitation est reçue, ignorez-la et passez au profil suivant
                    continue;
                }
                // Désérialisez le JSON pour obtenir l'objet SaveProfile
                try
                {
                    SaveProfile receivedProfile = JsonConvert.DeserializeObject<SaveProfile>(json);
                    // Si un profil avec le même nom existe déjà, remplacez-le
                    if (saveProfiles.Exists(profile => profile.Name == receivedProfile.Name))
                    {
                        saveProfiles[saveProfiles.FindIndex(profile => profile.Name == receivedProfile.Name)] = receivedProfile;
                    }
                    else
                    {
                        saveProfiles.Add(receivedProfile);
                    }
                    Task.Delay(200).Wait();
                }
                catch (Exception)
                {
                    continue;
                }
            }
        });
    }
}