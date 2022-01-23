using Saal.Todos.Services.Core;

namespace Saal.Todos.Services.Category
{
    public class DeleteCategoryCommand : ICommand
    {
        public int CategoryId { get; set; }
    }

}
