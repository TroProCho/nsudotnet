using System;

namespace Byzov.Nsudotnet.Enigma
{
    class KeyNotFoundException : Exception
    {
        public string Filename { get; private set; }

        public KeyNotFoundException()
            : base()
        {
            Filename = null;
        }

        public KeyNotFoundException(string filename)
            : base(string.Format("File {0} doesn't contain key.", filename))
        {
            Filename = filename;
        }
    }
}
