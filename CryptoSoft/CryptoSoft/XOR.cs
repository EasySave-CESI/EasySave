using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSoft
{
    internal class XOR
    {
        public static string Encrypt(string text, string key)
        {
            string result = string.Empty;
            for (int i = 0; i < text.Length; i++)
                result += (char)(text[i] ^ key[(i % key.Length)]);
            return result;
        }

        public static string Decrypt(string text, string key)
        {
            return Encrypt(text, key);
        }
    }
}
