using System.Security.Cryptography;
using System.Text;

namespace KeyGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string keyDirectorypath = Path.Combine(Environment.CurrentDirectory,"Keys");
            if (!Directory.Exists(keyDirectorypath))
            {
                Directory.CreateDirectory(keyDirectorypath);
            }

            var rsa = RSA.Create();
            string privateKeyXml = rsa.ToXmlString(true);
            string publicKeyXml = rsa.ToXmlString(false);

            using var privateFile = File.Create(Path.Combine(keyDirectorypath,"PrivateKey.xml"));
            using var publicFile = File.Create(Path.Combine(keyDirectorypath, "PubliceKey.xml"));

            privateFile.Write(Encoding.UTF8.GetBytes(privateKeyXml));
            publicFile.Write(Encoding.UTF8.GetBytes(publicKeyXml));
        }
    }
}
