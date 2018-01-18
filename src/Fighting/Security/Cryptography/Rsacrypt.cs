using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Fighting.Security.Cryptography
{
    public class Rsacrypt : IEncryptable
    {
        public string Encrypt(string input, string key)
        {
            string data = "I'm a programmer!";

            X509Certificate2 prvcrt = new X509Certificate2(@"D:\aaaa.pfx", "cqcca", X509KeyStorageFlags.Exportable);
            RSACryptoServiceProvider prvkey = (RSACryptoServiceProvider)prvcrt.PrivateKey;
            RSACryptoServiceProvider pubkey = (RSACryptoServiceProvider)prvcrt.PublicKey.Key;
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(pubkey.ExportParameters(false));
                    rsa.ImportParameters(prvkey.ExportParameters(true));
                    /* 加密 */
                    byte[] encryptBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(data), false);
                    string encryptString = Convert.ToBase64String(encryptBytes);
                    Console.WriteLine("======================加密=============================");
                    Console.WriteLine(encryptString);
                    Console.WriteLine("======================加密=============================");
                    /* 解密 */
                    byte[] decryptBytes = rsa.Decrypt(encryptBytes, false);
                    string decryptString = Encoding.UTF8.GetString(decryptBytes);
                    Console.WriteLine("======================解密=============================");
                    Console.WriteLine(decryptString);
                    Console.WriteLine("======================解密=============================");
                    /* 签名 */
                    byte[] signBytes = rsa.SignData(Encoding.UTF8.GetBytes(data), new SHA1CryptoServiceProvider());
                    string signString = Convert.ToBase64String(signBytes);
                    Console.WriteLine("======================签名=============================");
                    Console.WriteLine(signString);
                    Console.WriteLine("======================签名=============================");
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
            return string.Empty;
        }

        public string Encrypt(string input, string key, Encoding encoding)
        {
            throw new NotImplementedException();
        }
    }
}
