using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListPractice.Models
{
    internal class TaskItem
    {
        public int Id {  get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public TaskItem(int id, string description) 
        {
            Id = id;
            Description = description;
            IsCompleted = false;
        }
    }
}
