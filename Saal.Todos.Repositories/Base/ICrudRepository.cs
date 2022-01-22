using System.Threading.Tasks;

namespace Saal.Todos.Repositories.Base
{
    /// <summary>
    /// Interface for crud repository with async execution
    /// </summary>
    /// <typeparam name="TData">The aggregate type the repository manages</typeparam>
    public interface ICrudAsyncRepository<TData>
        where TData : class
    {
        /// <summary>
        /// Fetches a specific records page
        /// </summary>
        /// <param name="pageSize">The number of elements per page</param>
        /// <param name="pageNumber">The page number</param>
        /// <returns>Paginated aggregates</returns>
        Task<IPaginatedResult<TData>> Fetch(int pageSize, int pageNumber);

        /// <summary>
        /// Fetches a concrete aggregate
        /// </summary>
        /// <param name="id">The id of the requested aggregate</param>
        /// <returns>Promise with The matched record or null if no match</returns>
        Task<TData> FetchById(int id);

        /// <summary>
        /// Adds the Aggregate
        /// </summary>
        /// <param name="data">The new record</param>
        /// <returns>Promise with The Id of the new aggregate</returns>
        Task<int> Insert(TData data);

        /// <summary>
        /// Changes an aggregate
        /// </summary>
        /// <param name="data">The aggregate we want to update</param>
        /// <returns>Promise</returns>
        Task Update(TData data);

        /// <summary>
        /// Removes an agregate
        /// </summary>
        /// <param name="id">The aggregate id we want to remove</param>
        /// <returns>Promise with The number of affected aggregates</returns>
        Task<bool> Remove(int id);
    }

    
}
