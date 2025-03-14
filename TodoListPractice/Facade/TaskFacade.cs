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
using TodoListPractice.Services;

namespace TodoListPractice.Facade
{
    internal class TaskFacade
    {
        private const string FilePath = "tasks.json";
        private readonly TaskNotifier notifier;
        // This is a callback.
        // When menu is instantiated and creates a TaskFacade, we pass the menu's AddMessage as a callback.
        // This ensures our TaskFacade can add messages to the menu, such as error messages etc.
        private readonly Action<string> addMessageCallback;


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
            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new List<TaskItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading tasks: {ex.Message}");
                return new List<TaskItem>(); // Return empty list if file read fails
            }
        }



        public void AddTodo(string description)
        {
            var tasks = GetTodos();
            // [-1] does not exist in C#. An older variant is [tasks.Count - 1], but tasks[^1] is a shorthand for that.
            // "Last()" does the same, and  is used for readability.
            int newId = tasks.Count > 0 ? tasks.Last().Id + 1 : 1; // Get next available ID. 
            var task = new TaskItem(newId, description);
            tasks.Add(task);
            SaveTasks(tasks);

            notifier.Notify(TaskEventType.TaskCreated,task);

        }

        public void UpdateTodo(int id, bool isCompleted)
        {
            var tasks = GetTodos();
            var task = tasks.Find(t => t.Id == id);
            if (task != null)
            {
                task.IsCompleted = isCompleted;
                SaveTasks(tasks);
                notifier.Notify(TaskEventType.TaskUpdated, task);
            }
        }

        public void RemoveTodo(int id)
        {
            var tasks = GetTodos();
            var taskToRemove = tasks.FirstOrDefault(t => t.Id == id);

            if (taskToRemove != null )
            {
                tasks.Remove(taskToRemove);
                SaveTasks(tasks);
                notifier.Notify(TaskEventType.TaskDeleted, taskToRemove);
            }
            else
            {
                // Store message so we can display it in the menu
                MessageStore.AddMesage($"Task with ID {id} was not found.");
            }
        }

        private void SaveTasks(List<TaskItem> tasks)
        {
            // Using StreamWriter to avoid locking the file for too long, as well as preventing multiple file access.
            using (var fileStream = new StreamWriter(FilePath))
            {
                string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                fileStream.Write(json);
            }

        }
    }
}
