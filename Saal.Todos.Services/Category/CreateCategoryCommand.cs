using Saal.Todos.Services.Core;

namespace Saal.Todos.Services.Category
{
    /// <summary>
    /// Create category command
    /// </summary>
    public class CreateCategoryCommand: ICommand
    {
        public string CategoryName { get; set; }   
    }
    
}
