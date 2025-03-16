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

        public TaskFacade(TaskNotifier notifier)
        {
            this.notifier = notifier;

            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "[]"); // Creates empty JSON array
            }
        }

        public List<TaskItem> GetTodos(SortOption sortOption = SortOption.ByIdAscending)
        {
            try
            {
                // Return empty list if file doesn't exist.
                if (!File.Exists(FilePath))
                {
                    return new List<TaskItem>();
                }
                string json = File.ReadAllText(FilePath);
                var tasks = JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new List<TaskItem>();

                // Sort based on choice
                return sortOption switch
                {
                    SortOption.ByIdAscending => tasks.OrderBy(t => t.Id).ToList(),
                    SortOption.ByIdDescending => tasks.OrderByDescending(t => t.Id).ToList(),
                    SortOption.ByDescriptionAscending => tasks.OrderBy(t => t.Description).ToList(),
                    SortOption.ByDescriptionDescending => tasks.OrderByDescending(t => t.Description).ToList(),
                    SortOption.ByCompletedFirst => tasks.OrderByDescending(t => t.IsCompleted).ToList(),
                    SortOption.ByPendingFirst => tasks.OrderBy(t => t.IsCompleted).ToList(),
                    _ => tasks
                };
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

            if (taskToRemove != null)
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
