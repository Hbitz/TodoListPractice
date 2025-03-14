using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListPractice.Services
{
    internal static class MessageStore
    {
        private static readonly List<string> messages = new();

        public static void AddMesage(string msg)
        {
            messages.Add(msg);
        }

        public static List<string> GetMessages()
        {
            return messages;
        }

        public static void ClearMessages()
        {
            messages.Clear();
        }
    }
}
