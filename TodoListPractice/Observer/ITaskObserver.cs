using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListPractice.Models;

namespace TodoListPractice.Observer
{
    internal interface ITaskObserver
    {
        void Update(TaskEventType eventType, TaskItem task);
    }
}
