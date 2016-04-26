using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dead_Lock_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            ResourcerService myResourcer = new ResourcerService();

            myResourcer.Start();
            Console.WriteLine("Press Enter to Quit");
            Console.ReadLine();
        }
    }
}
