﻿using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListPractice.Facade;
using TodoListPractice.Models;
using TodoListPractice.Observer;
using TodoListPractice.Services;

namespace TodoListPractice.UI
{
    internal class Menu
    {
        private readonly TaskFacade taskFacade;
        private readonly TaskNotifier notifier;
        private SortOption currentSortOption = SortOption.ByIdAscending;

        public Menu(TaskNotifier notifier)
        {
            this.notifier = notifier;
            taskFacade = new TaskFacade(notifier);
            var logger = new TaskLogger();
            notifier.Attach(logger);
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                AnsiConsole.MarkupLine("[bold underline blue]==== TODO LIST ====[/]");

                var tasks = taskFacade.GetTodos(currentSortOption);

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

                // Display any messages from the notifier.
                DisplayMessages();

                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select an option")
                        .PageSize(10)
                        .AddChoices(new[] { "Add Task", "Mark Task as Completed", "Remove Task", "Search Task", "Change Sorting", "Exit" })
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
                    case "Search Task":
                        SearchAndDisplayResults();
                        break;
                    case "Change Sorting":
                        ChangeSorting();
                        break;
                    case "Exit":
                        return;
                }
            }
        }

        // Searching task descriptions based on user input
        private void SearchAndDisplayResults()
        {
            var searchQuery = AnsiConsole.Ask<string>("Enter search term:");
            var results = taskFacade.SearchTodos(searchQuery);

            if (results.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No matching tasks found.[/]");
            }
            else
            {
                var table = new Table();
                table.Border = TableBorder.Rounded;
                table.AddColumn("[bold]ID[/]");
                table.AddColumn("[bold]Status[/]");
                table.AddColumn("[bold]Description[/]");

                foreach (var task in results)
                {
                    string status = task.IsCompleted ? "[green]Completed[/]" : "[red]Pending[/]";
                    table.AddRow(task.Id.ToString(), status, task.Description);
                }
                AnsiConsole.Write(table);
            }
            AnsiConsole.MarkupLine("[blue]Press any key to return to the menu...[/]");
            Console.ReadKey();
        }

        // Helper method to display messages from our notifier
        private void DisplayMessages()
        {
            var messages = MessageStore.GetMessages();
            foreach (var msg in messages)
            {
                Console.WriteLine(msg);
            }
            MessageStore.ClearMessages();
        }

        // Method to let user change the sorting order of the ToDo's.
        private void ChangeSorting()
        {
            var sortOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select sorting order")
                    .AddChoices(new[] { "ID Ascending", "ID Descending", "Description Ascending", "Description Descending", "Completed first", "Pending first" })
            );

            currentSortOption = sortOption switch
            {
                "ID Ascending" => SortOption.ByIdAscending,
                "ID Descending" => SortOption.ByIdDescending,
                "Description Ascending" => SortOption.ByDescriptionAscending,
                "Description Descending" => SortOption.ByDescriptionDescending,
                "Completed first" => SortOption.ByCompletedFirst,
                "Pending first" => SortOption.ByPendingFirst,
                _ => currentSortOption
            };
        }
    }
}
