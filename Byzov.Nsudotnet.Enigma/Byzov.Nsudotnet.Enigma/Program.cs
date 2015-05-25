using System;
using System.IO;

namespace Byzov.Nsudotnet.Enigma
{
    class Program
    {
        public const string ComandLineArguments = "Command line argemunts for encrypt: encrypt inputFile algorithmName outputFile\n" +
                                                  "For decrypt: decrypt inputFile algorithmName keyFile outputfile\n" +
                                                  "Supported algorytms: AES, DES, RC2, Rijndael.";

        public const string AlgorithmNotSupported = "Algorithm \"{0}\" not supported.\n" +
                                                  "Supported algorytms: AES, DES, RC2, Rijndael.";

        private const int NumberParametersForEncrypt = 4;
        private const int NumberParametersForDecrypt = 5;
        public const string Encrypt = "encrypt";
        public const string Decrypt = "decrypt";

        private static void Main(string[] args)
        {
            if (args.Length != NumberParametersForDecrypt && args.Length != NumberParametersForEncrypt)
            {
                Console.WriteLine(ComandLineArguments);
                return;
            }
            SupportedAlgorithms supportedAlgorithm;
            if (!SupportedAlgorithms.TryParse(args[2], true, out supportedAlgorithm))
            {
                Console.Write(AlgorithmNotSupported, args[2]);
                return;
            }

            
            using (var algorithm = Enigma.GetSymmetricAlgorithm(supportedAlgorithm))
            {
                try
                {
                    switch (args[0].ToLower())
                    {
                        case Encrypt:
                        {
                            Enigma.Encrypt(args[1], algorithm, args[3]);
                            break;
                        }

                        case Decrypt:
                        {
                            Enigma.Decrypt(args[1], algorithm, args[3], args[4]);
                            break;
                        }

                        default:
                        {
                            Console.WriteLine(ComandLineArguments);
                            return;
                        }
                    }
                }
                catch (KeyNotFoundException e)
                {
                    Console.WriteLine("File {0} isn't Key File.", e.Filename);
                    return;
                }
                catch (FileNotFoundException e)
                {
                    Console.Out.WriteLine("File {0} not found.", e.FileName);
                    return;
                }
            }
        }
    }
}