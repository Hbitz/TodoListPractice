using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListPractice.Models;

namespace TodoListPractice.Observer
{
    // This is both a singleton and an observer, but since it's the only singleton in the application I've decided to let in Observer folder
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

        public void Notify(TaskEventType eventType, TaskItem task)
        {
            foreach (var o in observers)
            {
                o.Update(eventType, task);
            }
        }
    }
}
