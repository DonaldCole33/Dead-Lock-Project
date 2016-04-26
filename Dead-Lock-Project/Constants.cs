using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dead_Lock_Project
{
    /// <summary>
    /// Global Constants used throughout the program
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Used for testing the Program
        /// </summary>
        public const string testDataFileLocation = @"..\..\Data.testdata.txt";

        /// <summary>
        /// Used for the completed Assignment
        /// </summary>
        public const string dataFileLocation = @"..\..\Data\data.txt";

        /// <summary>
        /// File seperators found in the files
        /// </summary>
        public static readonly char[] fileSeperators = { ';', ',' };

        /// <summary>
        /// The amount of deadlock states in a row to find
        /// </summary>
        public const int deadlockLimit = 3;
    }
}
