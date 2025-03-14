using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListPractice.Models;
using TodoListPractice.Services;

namespace TodoListPractice.Observer
{
    internal class TaskLogger : ITaskObserver
    {
        // If observer only need to know something changed, an empy update is enough.
        // If observers care about what changed, Update could have parameter that takes a TaskEventType eventType 
        // If observers need full details, we could use TaskEvent
        public void Update(TaskEventType eventType, TaskItem task)
        {
            switch (eventType)
            {
                case TaskEventType.TaskCreated:
                    MessageStore.AddMesage($"[LOG]: Task created - {task.Description}");
                    break;

                case TaskEventType.TaskUpdated:
                    MessageStore.AddMesage($"[LOG]: Task updated - {task.Description}, Completed: {task.IsCompleted}");
                    break;

                case TaskEventType.TaskDeleted:
                    MessageStore.AddMesage($"[LOG]: Task removed - {task.Description}");
                    break;
            }
        }
    }
}
