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
    public class ResourcerService
    {
        /// <summary>
        /// Start a new Resourcer Serivice that parses a specific file with Processes
        /// </summary>
        /// <param name="FileLocation"></param>
        public ResourcerService(string fileLocation)
        {
            _splitLineList = FileParserService.GetSplitLineListFromFile(fileLocation);
            NumberofProcesses = getProcessesAmountFromList();
            NumberofResources = getResourcesAmountFromList();

            _initialResources = new List<int>(NumberofResources);
            _processes = new List<Process>(NumberofProcesses);
        }

        /// <summary>
        /// The initial resources that were parsed from the file
        /// </summary>
        private List<int> _initialResources;

        /// <summary>
        /// The list of Processes
        /// </summary>
        private List<Process> _processes;

        /// <summary>
        /// The number of Resources
        /// </summary>
        private int _numberofResources;

        /// <summary>
        /// The number of Processes
        /// </summary>
        private int _numberofProcesses;

        /// <summary>
        /// The list of raw data in the form of string arrays
        /// </summary>
        private List<string[]> _splitLineList;

        /// <summary>
        /// The number of resources
        /// </summary>
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

        /// <summary>
        /// The Number of Processes
        /// </summary>
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
        /// Get the amount of processes from a parsed Line list
        /// </summary>
        /// <returns>int</returns>
        private int getProcessesAmountFromList()
        {
            return int.Parse(_splitLineList.First().First());
        }

        /// <summary>
        /// Get the amount of resources from a parsed line list
        /// </summary>
        /// <returns>int</returns>
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

        /// <summary>
        /// Add the initial allocated Resources from a split line list
        /// into a new List of allocated Resourses
        /// </summary>
        private void addInitialAllocatedResourcesFromSplitLineList()
        {
            for (int i = 0; i < _numberofResources; i++)
            {
                int resourceAmount = int.Parse(_splitLineList.First().ElementAt(i + 1));
                _initialResources.Add(resourceAmount);
            }
        }

        /// <summary>
        /// This is the main method to run the Resourcer
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
                addNeededResourcesToProcesses();
                if (!checkDeadlockDetection())
                {
                    noSafeStateCount++;
                }
                else
                {
                    noSafeStateCount = 0;
                    
                }
                
                if (noSafeStateCount == Constants.deadlockLimit)
                {
                    //We need to relenquish the processes allocated resources each time until all 
                    //processes have been relenquished (End program) or a safe state occurs
                    Console.Write(Constants.deadlockLimit);
                    Console.WriteLine(" Deadlocks in a row!");
                    

                    foreach(var process in _processes)
                    {
                        var strVar = "Relenquishing Process " + process.ProcessID;
                        Console.Write(strVar);
                        
                        addNeededResourcesToProcesses();
                        process.RelenquishAllocatedResources();

                        if (checkDeadlockDetection())
                        {
                            //Can continue the program
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Did not work");
                        }
                    }

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

        /// <summary>
        /// Check whether there is deadlocks for the next needed resources
        /// vs the amount of available resources
        /// </summary>
        /// <returns>bool</returns>
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

            Console.WriteLine("Running Deadlock Detection");
            Console.Write("Initial Allocated Resources are: ");
            PrintResources(availableResourcesLeft);

            while (workPool.Any())
            {
                for (int i = 0; i < NumberofProcesses; i++)
                {
                    foreach (var process in workPool)    //go through each process until finished is filled or not
                    {
                        string strVar = "Checking Process " + process.ProcessID + ", it's resources are: ";
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

        /// <summary>
        /// Add the needed resources to each of the processes
        /// </summary>
        private void addNeededResourcesToProcesses()
        {
            for (int i = 0; i < NumberofProcesses; i++)
            {
                _processes.ElementAt(i).AddNeededResources(_splitLineList.First());
                _splitLineList.RemoveAt(0);
            }
        }

        /// <summary>
        /// Print the Available Resources
        /// </summary>
        private void PrintAvailableResources()
        {
            string output = "";
            foreach (int i in _initialResources)
            {
                output += i;
                output += ",";
            }
            var newOutput = output.TrimEnd(',');
            Console.WriteLine(newOutput);
        }

        /// <summary>
        /// Print any list of resources
        /// </summary>
        /// <param name="resources"></param>
        public void PrintResources(List<int> resources)
        {
            string output = "";
            foreach (var i in resources)
            {
                output += i;
                output += ",";
            }
            var newOutput = output.TrimEnd(',');
            Console.WriteLine(newOutput);
        }

        /// <summary>
        /// Print the Valid State format from a finished list of Processes
        /// </summary>
        /// <param name="processPool"></param>
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
