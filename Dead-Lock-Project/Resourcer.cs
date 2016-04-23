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
        private List<int> _initialResources;

        private List<Process> _processes;

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

            NumberofProcesses = getProcessesAmountFromList();
            _numberofResources = getResourcesAmountFromList();

            _initialResources = new List< int>(_numberofResources);
            _processes = new List<Process>(NumberofProcesses);
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
        /// This will add the number of processes to the dictionary for processing and
        /// remove the lines that have been read
        /// </summary>
        private void addProcessesFromSplitLineList()
        {
            for (int i = 0; i < NumberofProcesses; i++)
            {
                var process = new Process(_numberofResources);
                process.AddAllocatedResources(_splitLineList.ElementAt(0));
                _processes.Add(process);
                _splitLineList.RemoveAt(0);
            }
        }

        private void addInitialAllocatedResourcesFromSplitLineList()
        {
            for (int i = 0; i < _numberofResources; i++)
            {
                int resourceAmount = int.Parse(_splitLineList.First().ElementAt(i+1));
                _initialResources.Add(resourceAmount);
            }
        }

        /// <summary>
        /// This is the main method to run the Program
        /// </summary>
        public void Start()
        {
            //add initial Resources
            addInitialAllocatedResourcesFromSplitLineList();

            //Remove this element
            _splitLineList.RemoveAt(0);

            //now we need to add the resources to the processes
            int noSafeStateCount = 0;

            while (_splitLineList.Count() > 0) {
                do
                {
                    addProcessesFromSplitLineList();
                    
                } while (checkDeadlockDetection());
                noSafeStateCount++;

                if(noSafeStateCount == 3)
                {
                    //We need to do some special stuff to the queue
                }
            }

            
            
        }

        private bool checkDeadlockDetection()
        {
            //Calculate the Available resources left from process allocation
            var availableResourcesLeft = new List<int>(NumberofProcesses);
            var resources = 0;

            for (int i = 0; i < _numberofResources; i++)
            {
                for (int j = 0; j < NumberofProcesses; j++)
                {
                    resources += _processes.ElementAt(j).getAllocatedResourceElementAt(i);
                }
                availableResourcesLeft.Add(_initialResources.ElementAt(i) - resources);
                resources = 0;
            }
            
            //We need to see if we can prevent deadlocks

            List<Process> workPool = new List<Process>(_processes);
            List<Process> finishedPool = new List<Process>(NumberofProcesses);
            List<Process> holdPool = new List<Process>(NumberofProcesses);
            var processChecks = new List<int>(NumberofProcesses);

            addNeededResourcesToProcesses();

            while (workPool.Any()) { 
                for (int i = 0; i < NumberofProcesses ; i++)
                {
                    foreach(int j = 0; j < NumberofProcesses - finishedPool.Count ; j++)
                    {
                        if (workPool[j].CheckForSafety(availableResourcesLeft))
                        {
                            //need to find at least one process within this j loop
                            finishedPool.Add(workPool[j]);
                            workPool.RemoveAt(j);
                            
                            for(int k = 0; k < NumberofResources; k++)
                            {
                                //Add the freed resources to the available Resources
                                availableResourcesLeft[k] += workPool[j].getNeededResourceElementAt(k);
                            }
                        }
                    }

                    if (finishedPool.Count < (i + 1))
                    {
                        //Error -> Deadlock State
                        return false;
                    }
                }
            }   
            
            return true;
        }

        private void addNeededResourcesToProcesses()
        {
            for (int i = 0; i < NumberofProcesses; i++)
            {
                _processes.ElementAt(i).AddNeededResources(_splitLineList.First());
                _splitLineList.RemoveAt(0);
            }
        }
        
    }
}
