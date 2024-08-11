using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace AMS.Application.Services;

public class EncryptionHelper
{
    private readonly string? _encryptionKey;

    public EncryptionHelper(IConfiguration configuration)
    {
        _encryptionKey = configuration["EncryptionKey"];

        if (string.IsNullOrEmpty(_encryptionKey))
        {
            throw new Exception("Encryption key not found in configuration.");
        }
    }
    
    public string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            var key = Encoding.UTF8.GetBytes(_encryptionKey!);
            aes.Key = key;
            aes.IV = new byte[16];

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        using (Aes aes = Aes.Create())
        {
            var key = Encoding.UTF8.GetBytes(_encryptionKey!);
            aes.Key = key;
            aes.IV = new byte[16];

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}