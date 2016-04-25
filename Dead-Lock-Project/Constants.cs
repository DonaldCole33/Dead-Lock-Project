using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dead_Lock_Project
{
    public class Constants
    {
        //public const string dataFileLocation = @"..\..\Data\data.txt";
        public const string dataFileLocation = @"..\..\Data\data.txt";

        public static readonly char[] fileSeperators = { ';', ',' };

        public const int deadlockLimit = 3;
    }
}
