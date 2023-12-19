using System;
using System.Text;
using CryptoSoft;

namespace Cryptosoft
{

    class CryptoSoft
    {
        public static Paths paths;
        public static Configuration config;

        public static string source;
        public static string destination;

        static void Main(string[] args)
        {
            // Initialize
            Initialize();

            // Check args
            if (args.Length == 0) { Console.WriteLine("Please enter arguments as shown in the help menu. Type \"cryptosoft.exe help\" for more information."); return; }

            // Check if help
            if (args[0] == "help") { Help(); return; }

            // Check if args are correct
            if (args.Length != 2) { Console.WriteLine("Please enter arguments as shown in the help menu. Type \"cryptosoft.exe help\" for more information."); return; }

            // Get source and destination
            source = args[0];
            destination = args[1];

            // Check if source is correct
            if (!paths.CheckFile(source)) { Console.WriteLine("The source file does not exist."); return; }

            // Encrypt
            string encrypted = XOR.Encrypt(paths.GetFileContent(source), config.Key);

            // Save encrypted file
            paths.SaveFile(destination, encrypted);
        }

        public static void Initialize()
        {
            // Load paths
            paths = new Paths();

            // Check all folders and files
            CheckFolders();
            CheckFiles();

            // Load config
            config = new Configuration(paths.CryptoSoftConfigFilePath);

            // Check key
            while (!CheckKey())
            {
                Console.WriteLine("Please enter a key for encryption and decryption. The key must be at least 8 characters long.");
                config.Key = Console.ReadLine();
            }

            // Save config
            config.ModifyKey(paths.CryptoSoftConfigFilePath);
        }

        public static void CheckFolders()
        {
            if (!paths.CheckFolfer(paths.CryptoSoftFolderPath)) {paths.CreateFolder(paths.CryptoSoftFolderPath);}
            if (!paths.CheckFolfer(paths.CryptoSoftCryptedFilesFolderPath)) {paths.CreateFolder(paths.CryptoSoftCryptedFilesFolderPath);}
            if (!paths.CheckFolfer(paths.CryptoSoftConfigFolderPath)) {paths.CreateFolder(paths.CryptoSoftConfigFolderPath);}
        }

        public static void CheckFiles() {if (!paths.CheckFile(paths.CryptoSoftConfigFilePath)){paths.CreateFile(paths.CryptoSoftConfigFilePath);}}

        public static bool CheckKey()
        {
            if (config.Key == null) {return false;}
            if (config.Key.Length < 8) {return false;}

            return true;
        }

        public static void Help()
        {
            Console.WriteLine("");
            Console.WriteLine("------------------------------------");
            Console.WriteLine("CryptoSoft Help Menu");
            Console.WriteLine("");
            Console.WriteLine("------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Usage: cryptosoft [sourceFiles] [destinationFiles]");
        }
    }
}