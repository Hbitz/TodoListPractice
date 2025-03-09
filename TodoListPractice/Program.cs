using TodoListPractice.Facade;
using TodoListPractice.Observer;

namespace TodoListPractice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var notifier = new TaskNotifier();

            //TaskFacade taskFacade = new TaskFacade();
            Menu menu = new Menu(notifier);
            menu.ShowMenu();
        }
    }
}
