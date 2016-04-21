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
            for (int i = 0; i < _numberOfResources; i++)
            {
                _allocatedResources.Add(int.Parse(resourceList.ElementAt(i)));
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

        }

        public void RemoveAllocatedResources()
        {

        }
    }
}
