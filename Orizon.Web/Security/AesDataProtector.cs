using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Orizon.Web.Security
{
    public static class AesDataProtector
    {

        private static byte[] Key { get; set; }

        public static void Init()
        {
            var salt = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Encryption:Salt"]);
            var password = new Rfc2898DeriveBytes(ConfigurationManager.AppSettings["Encryption:Key"], salt);
            Key = password.GetBytes(32);
        }

        public static string Protect(string data)
        {
            return EncodeBase64Url(Encrypt(Encoding.UTF8.GetBytes(data)));
        }
        public static string Unprotect(string data)
        {
            return Encoding.UTF8.GetString(Decrypt(DecodeBase64Url(data)));
        }
        public static string Hash(string data)
        {
            var hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(data));
            return EncodeBase64Url(hash);
        }

        private static byte[] Encrypt(byte[] text)
        {
            byte[] block;
            byte[] iv;

            using (var r = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 })
            {
                r.GenerateIV();
                iv = r.IV;

                var encryptor = r.CreateEncryptor(Key, iv);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(text, 0, text.Length);
                        cs.FlushFinalBlock();
                        block = ms.ToArray();
                    }
                }
            }

            IEnumerable<byte> cipher = block.Concat(iv);
            return cipher.ToArray();
        }

        private static byte[] Decrypt(byte[] text)
        {
            byte[] plain = null;
            byte[] data = text.Take(text.Length - 16).ToArray();
            byte[] iv = text.Skip(data.Length).ToArray();
            int count = 0;

            using (var r = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 })
            {
                var decryptor = r.CreateDecryptor(Key, iv);
                using (var ms = new MemoryStream(data))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        plain = new byte[text.Length];
                        count = cs.Read(plain, 0, plain.Length);
                    }
                }
            }

            return plain;
        }

        private static string EncodeBase64Url(byte[] input)
        {
            var text = Convert.ToBase64String(input).Replace("+", "-").Replace("/", "_").Replace("=", "");
            return text;
        }

        private static byte[] DecodeBase64Url(string input)
        {
            if (string.IsNullOrEmpty(input)) { return null; }

            try
            {
                var text = input.Replace("_", "/").Replace("-", "+");
                text = text.PadRight(text.Length + (4 - text.Length % 4) % 4, '=');
                return Convert.FromBase64String(text);
            }
            catch
            {
                return null;
            }
        }

    }
}
