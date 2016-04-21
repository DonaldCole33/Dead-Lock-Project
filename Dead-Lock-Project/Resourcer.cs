using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dead_Lock_Project
{
    /// <summary>
    /// Resourcer will configure safestates or deadlocks between resources
    /// and Processes.
    /// </summary>
    public class Resourcer
    {
        private Dictionary<int, int> _resources;

        private Dictionary<int, Process> _processes;

        private int _numberofResources;

        private int _numberofProcesses;

        private List<string[]> _splitLineList;

        public int NumberofResources
        {
            get
            {
                return _numberofResources;
            }

            set
            {
                _numberofResources = value;
            }
        }

        public int NumberofProcesses
        {
            get
            {
                return _numberofProcesses;
            }

            set
            {
                _numberofProcesses = value;
            }
        }

        /// <summary>
        /// Take in the filelocation of the processes and resources with a specific 
        /// format.
        /// </summary>
        /// <param name="FileLocation"></param>
        public Resourcer()
        {
            _splitLineList = FileParser.GetSplitLineListFromFile(Constants.dataFileLocation);

            _numberofProcesses = getProcessesAmountFromList();
            _numberofResources = getResourcesAmountFromList();

            _resources = new Dictionary<int, int>(_numberofResources);
            _processes = new Dictionary<int, Process>(_numberofProcesses);
        }

        /// <summary>
        /// Get the amount of processes from a parsed Line list
        /// </summary>
        /// <param name="splitLineList"></param>
        /// <returns></returns>
        private int getProcessesAmountFromList()
        {
            return int.Parse(_splitLineList.First().First());
        }

        private int getResourcesAmountFromList()
        {
            return _splitLineList.First().Count() - 1;
        }

        /// <summary>
        /// This will add the number of processes to the dictionary for processing
        /// </summary>
        private void addProcessesFromSplitLineList()
        {
            for (int i = 0; i < _numberofProcesses; i++)
            {
                var process = new Process(_numberofResources);
                process.AddAllocatedResources(_splitLineList.ElementAt(i));
                _splitLineList.RemoveAt(i);

                //_processes.Add(i, _splitLineList.ElementAt(i))
            }
        }

        private void addResourcesFromSplitLineList()
        {

        }

    }
}
