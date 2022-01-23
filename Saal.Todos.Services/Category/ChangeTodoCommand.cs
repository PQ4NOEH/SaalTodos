using Saal.Todos.Contracts.Dto;
using Saal.Todos.Services.Core;

namespace Saal.Todos.Services.Category
{
    public class ChangeTodoCommand : ICommand
    {
        public int CurrentCategoryId { get; set; }
        public int NewCategoryId { get; set; }
        public Todo Todo { get; set; }
    }
}
