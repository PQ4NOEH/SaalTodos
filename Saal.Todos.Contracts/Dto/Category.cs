using System.Collections.Generic;

namespace Saal.Todos.Contracts.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class Category
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public List<Todo> Todos { get; set; } = new List<Todo>();
    }
}
