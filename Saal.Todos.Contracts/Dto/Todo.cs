using System;

namespace Saal.Todos.Contracts.Dto
{
    public class Todo
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public DateTime? DeadLine { get; set; }
        public bool Done { get; set; } = false;
    }
}
