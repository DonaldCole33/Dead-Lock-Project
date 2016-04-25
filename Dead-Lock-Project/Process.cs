using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dead_Lock_Project
{

    public class Process
    {
        private List<int> _allocatedResources;

        private List<int> _neededResources;

        private int _numberOfResources;

        public int ProcessID;

        public bool? ResourcesSatisfied { get; set; }

        public Process(int numberOfResources)
        {
            _numberOfResources = numberOfResources;
            _allocatedResources = new List<int>(_numberOfResources);
            _neededResources = new List<int>(_numberOfResources);
        }

        public void AddAllocatedResources(string[] resourceList)
        {
            for (int i = 0; i < _numberOfResources; i++)
            {
                _allocatedResources.Add(int.Parse(resourceList.ElementAt(i)));
            }
        }

        public void AddNeededResources(string[] resourceList)
        {
            if (_neededResources.Any())
            {
                _neededResources.Clear();
            }

            for (int i = 0; i < _numberOfResources; i++)
            {
                _neededResources.Add(int.Parse(resourceList.ElementAt(i)));
            }
        }

        public int getAllocatedResourceElementAt(int element)
        {
            return _allocatedResources.ElementAt(element);
        }

        public int getNeededResourceElementAt(int element)
        {
            return _neededResources.ElementAt(element);
        }

        public void RemoveNeededResources()
        {
            _neededResources.Clear();
        }

        public void RemoveAllocatedResources()
        {
            _allocatedResources.Clear();
        }

        /// <summary>
        /// This function will take in the resources that are available, 
        /// if there are enough to satisfy this process it will return true, else false
        /// </summary>
        /// <param name="availableResources"></param>
        /// <returns>bool</returns>
        public bool CheckForSafety(List<int> availableResources)
        {
            var count = 0; 

            for (int i = 0; i < _numberOfResources; i++)
            {
                if (_neededResources[i] <= availableResources[i])
                {
                    count++;
                }
            }
            
            if(count == _numberOfResources)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void PrintAllocatedResources()
        {
            string output = "Allocated: ";
            foreach(int i in _allocatedResources)
            {
                output += i;
                output += ",";
            }
            var newOutput = output.TrimEnd(',');
            Console.WriteLine(newOutput);
        }

        public void PrintNeededResources()
        {
            string output = "Needed:    ";
            foreach (int i in _neededResources)
            {
                output += i;
                output += ",";
            }
            var newOutput = output.TrimEnd(',');
            Console.WriteLine(newOutput);
        }

        public void RelenquishAllocatedResources()
        {
            for(int i = 0; i < _numberOfResources; i++)
            {
                _neededResources[i] += _allocatedResources[i];
                _allocatedResources[i] = 0;
            }
        }
    }
}
