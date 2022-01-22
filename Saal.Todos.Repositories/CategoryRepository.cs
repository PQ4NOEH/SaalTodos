using Saal.Todos.Contracts.Dto;
using Saal.Todos.Repositories.Base;

namespace Saal.Todos.Repositories
{
    public interface ICategoryRepository: ICrudAsyncRepository<Category>
    {
        
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
    }


}
