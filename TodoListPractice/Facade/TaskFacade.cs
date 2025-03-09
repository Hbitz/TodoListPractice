using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using TodoListPractice.Models;
using Newtonsoft.Json;
using TodoListPractice.Observer;

namespace TodoListPractice.Facade
{
    internal class TaskFacade
    {
        private const string FilePath = "tasks.json";
        private readonly TaskNotifier notifier;

        public TaskFacade(TaskNotifier notifier)
        {
            this.notifier = notifier;

            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "[]"); // Creates empty JSON array
            }
        }

        public List<TaskItem> GetTodos()
        {
            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new List<TaskItem>();
        }


        public void AddTodo(string description)
        {
            var tasks = GetTodos();
            // [-1] does not exist in C#. An older variant is [tasks.Count - 1], but tasks[^1] is a shorthand for that.
            int newId = tasks.Count > 0 ? tasks[^1].Id + 1 : 1; // get next available ID
            tasks.Add(new TaskItem(newId, description));
            SaveTasks(tasks);

            notifier.Notify();

        }

        public void UpdateTodo(int id, bool isCompleted)
        {
            var tasks = GetTodos();
            var task = tasks.Find(t => t.Id == id);
            if (task != null)
            {
                task.IsCompleted = isCompleted;
                SaveTasks(tasks);
                notifier.Notify();
            }
        }

        public void RemoveTodo(int id)
        {
            var tasks = GetTodos();
            tasks.RemoveAll(t => t.Id == id);
            SaveTasks(tasks);
            notifier.Notify();
        }

        private void SaveTasks(List<TaskItem> tasks)
        {
            string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
    }
}
