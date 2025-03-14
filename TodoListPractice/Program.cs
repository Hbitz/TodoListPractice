using TodoListPractice.Facade;
using TodoListPractice.Observer;
using TodoListPractice.UI;

namespace TodoListPractice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var notifier = TaskNotifier.Instance;
            Menu menu = new Menu(notifier);
            menu.ShowMenu();
        }
    }
}
