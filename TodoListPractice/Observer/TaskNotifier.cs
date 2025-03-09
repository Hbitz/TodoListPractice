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
