using Saal.Todos.Contracts.Dto;
using Saal.Todos.Repositories.Base;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Saal.Todos.Repositories
{
    public interface ICategoryRepository: ICrudAsyncRepository<Category>
    {
        Task<int> InsertTodo(int categoryId, [NotNull] Todo todo);
        Task UpdateTodo(int originalCategoryId, int newCategoryId, [NotNull] Todo todo);
        Task<bool> RemoveTodo(int categoryId, int todoId);
    }

    /// <summary>
    /// In memory category repository see <see cref="InMemoryRepository"/>
    /// </summary>
    public class InMemoryCategoryRepository : InMemoryRepository<Category>, ICategoryRepository
    {
        public InMemoryCategoryRepository(IInMemoryStorage<Category> memoryStorage)
            : base(memoryStorage)
        {

        }
        /// <summary>
        /// see <see cref="InMemoryRepository.GetId(TData)"/>
        /// </summary>
        protected override int GetId(Category data) => data.Id.Value;

        /// <summary>
        /// see <see cref="InMemoryRepository.SetId(TData, int)"/>
        /// </summary>
        protected override Category SetId(Category data, int id) 
        {
            return new Category
            {
                Id = id,
                Name = data.Name,
                Todos = data.Todos
            };
        }

        int NextTodoId(IList<Category> data) => data.Max(c => c.Todos.Max(t => t.Id.Value)) + 1;
        public async Task<int> InsertTodo(int categoryId, [NotNull] Todo todo)
        {
            var aggregate = await this.FetchById(categoryId);
            if (aggregate != null)
            {
                _memoryStorage.DoMutate(data =>
                {
                    todo.Id = NextTodoId(data);
                    data.First(c => c.Id == categoryId).Todos.Add(todo);
                });
            }
            return todo.Id.Value;
        }
        /// <summary>
        /// Updates the existing todo
        /// </summary>
        /// <param name="originalCategoryId">The current category of the todo</param>
        /// <param name="newCategoryId">The new todo category (might be the same)</param>
        /// <param name="todo">The todo</param>
        /// <returns></returns>
        public async Task UpdateTodo(int originalCategoryId, int newCategoryId, [NotNull] Todo todo)
        {
            if(originalCategoryId != newCategoryId)
            {
                await RemoveTodo(originalCategoryId, todo.Id.Value);
            }
            var aggregate = await this.FetchById(newCategoryId);
            if (aggregate != null)
            {
                _memoryStorage.DoMutate(data => data.First(c => c.Id == newCategoryId).Todos.Add(todo));
            }

        }
        /// <summary>
        /// Removes the required todo from the category
        /// </summary>
        /// <param name="categoryId">The todo categoryid</param>
        /// <param name="todoId">The todoId</param>
        public async Task<bool> RemoveTodo(int categoryId, int todoId)
        {
            var aggregate = await this.FetchById(categoryId);
            if (aggregate == null) return false;
            return _memoryStorage.DoMutate<bool>(
                data => data.First(c => c.Id == categoryId).Todos.RemoveAll(x => x.Id == todoId) > 0
            );
        }
    }


}
