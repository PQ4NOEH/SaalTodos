using Saal.Todos.Services.Core;

namespace Saal.Todos.Services.Category
{
    public class ChangeCategoryCommand: ICommand
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

}
