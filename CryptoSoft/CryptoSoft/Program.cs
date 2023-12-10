using System;
using System.Text;

class CryptoSoft
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: CryptoSoft <text_to_encrypt> <key>");
            return;
        }

        string textToEncrypt = args[0];
        string key = args[1];

        string encryptedText = Encrypt(textToEncrypt, key);

        Console.WriteLine(encryptedText);
    }

    static string Encrypt(string text, string key)
    {
        StringBuilder encryptedText = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            encryptedText.Append((char)(text[i] ^ key[i % key.Length]));
        }

        return encryptedText.ToString();
    }
}
