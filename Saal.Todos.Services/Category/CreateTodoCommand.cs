using Saal.Todos.Services.Core;
using System;

namespace Saal.Todos.Services.Category
{
    public class CreateTodoCommand : ICommand
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public DateTime? DeadLine { get; set; }
        public bool Done { get; set; } = false;
    }
}
