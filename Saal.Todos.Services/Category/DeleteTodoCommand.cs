using Saal.Todos.Services.Core;

namespace Saal.Todos.Services.Category
{
    public class DeleteTodoCommand : ICommand
    {
        public int CategoryId { get; set; }
        public int TodoId { get; set; }
    }
}
