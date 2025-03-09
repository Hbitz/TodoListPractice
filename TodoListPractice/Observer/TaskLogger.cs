using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListPractice.Observer
{
    internal class TaskLogger : ITaskObserver
    {
        // If observer only need to know something changed, an empy update is enough.
        // If observers care about what changed, Update could have parameter that takes a TaskEventType eventType 
        // If observers need full details, we could use TaskEvent
        public void Update()
        {
            Console.WriteLine("[LOG]: Tasks have been modified.");
        }
    }
}
