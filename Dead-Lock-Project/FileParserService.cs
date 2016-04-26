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
    static public class FileParserService
    {
        
        /// <summary>
        /// Getting the initial lines from the file placed in a list of split lines 
        /// </summary>
        static public List<string[]> GetSplitLineListFromFile(string fileLocation)
        {
            List<string[]> _linesFromFile = new List<string[]>();
            StreamReader dataFile = File.OpenText(fileLocation);
            string line;

            while ((line = dataFile.ReadLine()) != null) 
            {
                //Trim off the trailing commas if there are any and then split the line to a string array
                var lineItems = line.Trim(Constants.fileSeperators).Split(Constants.fileSeperators);
                if (lineItems.First().Count() != 0)     //When the Line is not empty
                {
                    _linesFromFile.Add(lineItems);
                }
            }
            return _linesFromFile;
        }
    }
}
