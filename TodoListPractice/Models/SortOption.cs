using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListPractice.Models
{
    internal enum SortOption
    {
        ByIdAscending,
        ByIdDescending,
        ByDescriptionAscending,
        ByDescriptionDescending,
        ByCompletedAscending,
        ByCompletedFirst,
        ByPendingFirst
    }
}
