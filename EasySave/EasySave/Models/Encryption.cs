namespace EasySaveConsoleApp
{
    public class EncryptionMethod
    {
        public EncryptionMethod() { }

        public string execute_EncryptionMethod(string text, string name, string key)
        {
            if (name == "XOR")
            {
                string result = "";
                for (int i = 0; i < text.Length; i++)
                {
                    result += (char)(text[i] ^ key[i % key.Length]);
                }
                return result;
            }
            else
            {
                return "Error: No encryption method found";
            }
        }

        public string execute_decryption_method(string text, string name, string key)
        {
            if (name == "XOR")
            {
                string result = "";
                for (int i = 0; i < text.Length; i++)
                {
                    result += (char)(text[i] ^ key[i % key.Length]);
                }
                return result;
            }
            else
            {
                return "Error: No encryption method found";
            }
        }
    }
}