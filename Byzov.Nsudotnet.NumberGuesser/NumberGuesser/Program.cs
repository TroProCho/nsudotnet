using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGuesser
{
    class Program
    {
        public static void Main(string[] args)
        {
            (new NumberGuesser(Console.In, Console.Out)).StartGame();
        }
    }
}
