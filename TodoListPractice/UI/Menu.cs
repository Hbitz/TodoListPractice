using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListPractice.Facade;
using TodoListPractice.Observer;

namespace TodoListPractice.UI
{
    internal class Menu
    {
        private readonly TaskFacade taskFacade;
        private readonly List<string> messages = new();

        public Menu(TaskNotifier notifier)
        {
            taskFacade = new TaskFacade(notifier, AddMessage); // Pass AddMessage method as a callback
            var logger = new TaskLogger();
            notifier.Attach(logger);
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                AnsiConsole.MarkupLine("[bold underline blue]==== TODO LIST ====[/]");

                // Display any messages for the user
                if (messages.Any())
                {
                    Console.WriteLine("----------------------------");
                    foreach (var msg in messages)
                    {
                        Console.WriteLine(msg);
                    }
                    Console.WriteLine("----------------------------");
                    messages.Clear();
                }


                var tasks = taskFacade.GetTodos();

                if (tasks.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No tasks found.[/]");
                }
                else
                {
                    var table = new Table();
                    table.Border = TableBorder.Rounded;
                    table.AddColumn("[bold]ID[/]");
                    table.AddColumn("[bold]Status[/]");
                    table.AddColumn("[bold]Description[/]");

                    foreach (var task in tasks)
                    {
                        string status = task.IsCompleted ? "[green]Completed[/]" : "[red]Pending[/]";
                        table.AddRow(task.Id.ToString(), status, task.Description);
                    }
                    AnsiConsole.Write(table);
                }

                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select an option")
                        .PageSize(10)
                        .AddChoices(new[] { "Add Task", "Mark Task as Completed", "Remove Task", "Exit" })
                );

                switch (option)
                {
                    case "Add Task":
                        var description = AnsiConsole.Ask<string>("Enter task description:");
                        taskFacade.AddTodo(description);
                        break;
                    case "Mark Task as Completed":
                        int completeId = AnsiConsole.Ask<int>("Enter task ID to mark as completed:");
                        taskFacade.UpdateTodo(completeId, true);
                        break;
                    case "Remove Task":
                        int removeId = AnsiConsole.Ask<int>("Enter task ID to remove:");
                        taskFacade.RemoveTodo(removeId);
                        break;
                    case "Exit":
                        return;
                }
            }
        }

        // Method to add any messages in our menu to display once.
        // Could be notifications, error messages etc from our last action.
        public void AddMessage(string msg)
        {
            messages.Add(msg);
        }
    }
}
