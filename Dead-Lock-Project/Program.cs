using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dead_Lock_Project
{
    /// <summary>
    /// This program will read in a text file formated specifically to this program.  It will then run 
    /// a deadlock detection on the number of processes and the number of resources available to
    /// the processes.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        static void Main(string[] args)
        {
            ResourcerService myResourcer = new ResourcerService(Constants.dataFileLocation);    //Create our Resource Object

            myResourcer.Start();    //Start the Deadlock Detection, will run until finished
            Console.WriteLine("Press Enter to Quit");   
            Console.ReadLine();
        }
    }
}
