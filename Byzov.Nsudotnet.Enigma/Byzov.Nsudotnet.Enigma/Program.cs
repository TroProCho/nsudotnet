using System;
using System.Data;
using System.IO;

namespace Byzov.Nsudotnet.Enigma
{
    class Program
    {
        public const int NumberParametersForEncrypt = 4;
        private const int NumberParametersForDecrypt = 5;

        private static void Main(string[] args)
        {
            if (args.Length != NumberParametersForDecrypt && args.Length != NumberParametersForEncrypt)
            {
                Console.WriteLine(Resources.ComandLineArgumentsMessage);
                Console.Read();
                return;
            }
            SupportedAlgorithms supportedAlgorithm;
            if (!SupportedAlgorithms.TryParse(args[2], true, out supportedAlgorithm))
            {
                Console.Write(Resources.AlgorithmNotSupportedMessage, args[2]);
                return;
            }


            using (var algorithm = Enigma.GetSymmetricAlgorithm(supportedAlgorithm))
            {
                try
                {
                    switch (args[0].ToLower())
                    {
                        case "encrypt":
                            {
                                Enigma.Encrypt(args[1], algorithm, args[3]);
                                break;
                            }

                        case "decrypt":
                            {
                                Enigma.Decrypt(args[1], algorithm, args[3], args[4]);
                                break;
                            }

                        default:
                            {
                                Console.WriteLine(Resources.ComandLineArgumentsMessage);
                                return;
                            }
                    }
                }
                catch (KeyNotFoundException e)
                {
                    Console.WriteLine(Resources.FileNotKeyFileMessage, e.Filename);
                    return;
                }
                catch (FileNotFoundException e)
                {
                    Console.Out.WriteLine(Resources.FileNotFoundMessage, e.FileName);
                    return;
                }
            }
        }
    }
}