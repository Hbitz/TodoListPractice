using TodoListPractice.Facade;

namespace TodoListPractice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TaskFacade taskFacade = new TaskFacade();
            Menu menu = new Menu(taskFacade);
            menu.ShowMenu();
        }
    }
}
