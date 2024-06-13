using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_API
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Choose a method convenient for you (XML / JSON)");
                string input = Console.ReadLine();

                Console.WriteLine(new LinkServer().ConnectServer(input));
            } while (true);
        }
    }
}
