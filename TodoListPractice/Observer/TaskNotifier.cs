using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListPractice.Observer
{
    internal class TaskNotifier
    {
        private readonly List<ITaskObserver> observers = new();

        // Converted to singleton:
        // Made it static so we can access it anywhere
        private static TaskNotifier? _instance;
        private TaskNotifier() { } // Private constructor to prevent instantiation
        public static TaskNotifier Instance
        {
            get
            {
                // Instantiation it once, then simply return it from then on.
                if (_instance == null) 
                {
                    _instance = new TaskNotifier();
                }
                return _instance;
            }
        }

        public void Attach(ITaskObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(ITaskObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var o in observers)
            {
                o.Update();
            }
        }
    }
}
