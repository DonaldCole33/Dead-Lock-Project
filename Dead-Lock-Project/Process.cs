using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dead_Lock_Project
{

    public class Process
    {
        private Dictionary<int, int> _allocatedResources;

        private Dictionary<int, int> _neededResources;

        private int _numberOfResources;

        public Process(int numberOfResources)
        {
            _numberOfResources = numberOfResources;
            _allocatedResources = new Dictionary<int, int>(_numberOfResources);
            _neededResources = new Dictionary<int, int>(_numberOfResources);
        }

        public void AddAllocatedResources(string[] resourceList)
        {
            for (int i = 0; i < _numberOfResources; i++)
            {
                _allocatedResources.Add(i, int.Parse(resourceList.ElementAt(i)));
            }
        }

        public void AddNeededResources(string[] resourceList)
        {
            for (int i = 0; i < _numberOfResources; i++)
            {
                _allocatedResources.Add(i, int.Parse(resourceList.ElementAt(i)));
            }
        }

        public void RemoveNeededResources()
        {

        }

        public void RemoveAllocatedResources()
        {

        }
    }
}
