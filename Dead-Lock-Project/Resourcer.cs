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

            _initialResources = new List<int>(_numberofResources);
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
                process.ProcessID = i + 1;
                process.AddAllocatedResources(_splitLineList.ElementAt(0));
                _processes.Add(process);
                _splitLineList.RemoveAt(0);
            }
        }

        private void addInitialAllocatedResourcesFromSplitLineList()
        {
            for (int i = 0; i < _numberofResources; i++)
            {
                int resourceAmount = int.Parse(_splitLineList.First().ElementAt(i + 1));
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
            addProcessesFromSplitLineList();        //Init the Processes

            while (_splitLineList.Count() > 0)
            {
                if (!checkDeadlockDetection())
                {
                    noSafeStateCount++;
                }
                else
                {
                    noSafeStateCount = 0;
                    
                }
                
                if (noSafeStateCount == 3)
                {
                    //We need to do some special stuff to the queue then restart the same process queue
                }
                else
                {
                    //Clear the needed resources from the processes
                    foreach(var process in _processes)
                    {
                        process.RemoveNeededResources();
                    }
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

            Console.WriteLine("Running Deadlock Detection");
            Console.WriteLine("Initial Allocated Resources are:");

            while (workPool.Any())
            {
                for (int i = 0; i < NumberofProcesses; i++)
                {
                    foreach (var process in workPool)    //go through each process until finished is filled or not
                    {
                        string strVar = "Starting Process " + process.ProcessID + ", it's resources are: ";
                        Console.WriteLine(strVar);
                        process.PrintAllocatedResources();
                        process.PrintNeededResources();

                        if (process.CheckForSafety(availableResourcesLeft))
                        {
                            //need to find at least one process within this j loop
                            strVar = "Valid Process Found: Process " + process.ProcessID;
                            Console.WriteLine(strVar);

                            finishedPool.Add(process);
                            workPool.Remove(process);

                            for (int k = 0; k < NumberofResources; k++)         //Create a function for this loop
                            {
                                //Add the freed resources to the available Resources
                                availableResourcesLeft[k] += process.getNeededResourceElementAt(k);
                                availableResourcesLeft[k] += process.getAllocatedResourceElementAt(k);
                            }
                            Console.WriteLine("New Allocated Resources Available are:");
                            PrintResources(availableResourcesLeft);
                            break;
                        }
                    }

                    if (finishedPool.Count < (i + 1))
                    {
                        //Error -> Deadlock State
                        Console.WriteLine("DEADLOCK STATE DETECTED\n");
                        return false;
                    }
                }
            }

            Console.WriteLine("VALID STATE DETECTED: ");
            PrintValidState(finishedPool);
            Console.WriteLine();
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

        private void PrintAvailableResources()
        {
            foreach (int i in _initialResources)
            {
                Console.Write(i);
            }
            Console.WriteLine();
        }

        private void PrintResources(List<int> resources)
        {
            foreach (var i in resources)
            {
                Console.Write( i);
            }
            Console.WriteLine();
        }

        public void PrintValidState(List<Process> processPool)
        {
            string output = "(";
            foreach(var process in processPool)
            {
                output += process.ProcessID + ",";
            }
            var newOutput = output.TrimEnd(',');
            newOutput += ")";
            Console.WriteLine(newOutput);
        }

    }
}
