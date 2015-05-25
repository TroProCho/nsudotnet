using System;
using System.IO;
using System.Security.Cryptography;

namespace Byzov.Nsudotnet.Enigma
{
    class Enigma
    {
        private const string KeyFileExtenshion = ".key";

        public static void Encrypt(string inputFile, SymmetricAlgorithm algorithm, string outputFile)
        {
            string keyFilename = Path.ChangeExtension(inputFile, string.Concat(KeyFileExtenshion, Path.GetExtension(inputFile)));
            using (var keyFileStream = new FileStream(keyFilename, FileMode.Create, FileAccess.Write))
            {
                using (var keyStreamWriter = new StreamWriter(keyFileStream))
                {
                    keyStreamWriter.WriteLine(Convert.ToBase64String(algorithm.Key));
                    keyStreamWriter.WriteLine(Convert.ToBase64String(algorithm.IV));
                }
            }

            using (var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            {
                using (var cryptoStream = new CryptoStream(inputFileStream, algorithm.CreateEncryptor(), CryptoStreamMode.Read))
                {
                    using (var outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                    {
                        cryptoStream.CopyTo(outputStream);
                    }
                }
            }
        }

        public static void Decrypt(string inputFile, SymmetricAlgorithm algorithm, string keyFile, string outputFile)
        {
            using (var keyFileStream = new FileStream(keyFile, FileMode.Open, FileAccess.Read))
            {
                using (var keyStreamReader = new StreamReader(keyFileStream))
                {
                    string key = keyStreamReader.ReadLine();
                    string iv = keyStreamReader.ReadLine();
                    if (key == null || iv == null)
                    {
                        throw new KeyNotFoundException(keyFileStream.Name);
                    }
                    algorithm.Key = Convert.FromBase64String(key);
                    algorithm.IV = Convert.FromBase64String(iv);
                }
            }

            using (var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            {
                using (var cryptoStream = new CryptoStream(inputFileStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (var outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                    {
                        cryptoStream.CopyTo(outputStream);
                    }
                }
            }
        }

        public static SymmetricAlgorithm GetSymmetricAlgorithm(SupportedAlgorithms supportedAlgorithm)
        {
            switch (supportedAlgorithm)
            {
                case SupportedAlgorithms.AES:
                    {
                        return Aes.Create();
                    }
                case SupportedAlgorithms.DES:
                    {
                        return DES.Create();
                    }
                case SupportedAlgorithms.RC2:
                    {
                        return RC2.Create();
                    }
                case SupportedAlgorithms.Rijndael:
                    {
                        return Rijndael.Create();
                    }
            }
            return null;
        }
    }
}