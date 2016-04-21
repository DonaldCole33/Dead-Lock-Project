using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dead_Lock_Project
{
    /// <summary>
    /// Static Class that returns information from a text file
    /// </summary>
    static public class FileParser
    {
        
        /// <summary>
        /// Getting the initial lines from the file placed in a list of split lines 
        /// </summary>
        static public List<string[]> GetSplitLineListFromFile(string fileLocation)
        {
            List<string[]> _linesFromFile = new List<string[]>();
            StreamReader dataFile = File.OpenText(fileLocation);
            string line;
            _linesFromFile = new List<string[]>();

            while ((line = dataFile.ReadLine()) != null) 
            {
                var lineItems = line.Trim(Constants.fileSeperators).Split(Constants.fileSeperators);
                if (lineItems.First().Count() != 0)
                {
                    _linesFromFile.Add(lineItems);
                }
                
            }
            return _linesFromFile;
        }
    }
}
