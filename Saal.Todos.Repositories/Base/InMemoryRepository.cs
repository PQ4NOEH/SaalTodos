using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Saal.Todos.Repositories.Base
{
    /// <summary>
    /// In memory implementation of <see cref="ICrudAsyncRepository"/>
    /// </summary>
    public abstract class InMemoryRepository<TData> : ICrudAsyncRepository<TData>
        where TData : class
    {
        
        readonly protected IInMemoryStorage<TData> _memoryStorage;

        protected InMemoryRepository([NotNull]IInMemoryStorage<TData> memoryStorage)
        {
            _memoryStorage = memoryStorage??throw new ArgumentNullException(nameof(memoryStorage));
        }

        int _nextId
        {
            get
            {
                if (_memoryStorage.Data.Any()) 
                {
                    return _memoryStorage.Data.Select(GetId).Max() + 1;
                }
                else
                {
                    return 1;
                }
            }
        }

        
        /// <summary>
        /// Set Id value of the Id property
        /// </summary>
        /// <param name="data">The Aggregate</param>
        /// <param name="id">The Id value we want to set</param>
        /// <returns>An Aggregate with the id value set</returns>
        protected abstract TData SetId(TData data, int id);
        
        /// <summary>
        /// Get the Id value of an aggregae
        /// </summary>
        /// <param name="data">The aggregate</param>
        /// <returns>The Id</returns>
        protected abstract int GetId(TData data);

        

        /// <summary>
        /// See <see cref="ICrudAsyncRepository.Fetch"/>
        /// </summary>
        public Task<IPaginatedResult<TData>> Fetch(int pageSize, int pageNumber)
        {
            var data = _memoryStorage.Data
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize);
            var result = new PaginatedResult<TData>(_memoryStorage.Data.Count(), pageNumber, pageSize, data);
            return Task.FromResult<IPaginatedResult<TData>>(result);
        }

        /// <summary>
        /// See <see cref="ICrudAsyncRepository.FetchById"/>
        /// </summary>
        public Task<TData> FetchById(int id) => Task.FromResult(_memoryStorage.Data.FirstOrDefault(i => GetId(i) == id));

        /// <summary>
        /// See <see cref="ICrudAsyncRepository.Remove"/>
        /// </summary>
        public async Task<bool> Remove(int id)
        {
            var aggregate = await this.FetchById(id);
            return _memoryStorage.DoMutate(data => data.Remove(aggregate));
        }

        /// <summary>
        /// See <see cref="ICrudAsyncRepository.Insert"/>
        /// </summary>
        public Task<int> Insert([NotNull] TData aggregate)
        {
            if(aggregate == null) throw new ArgumentNullException(nameof(aggregate));
            return _memoryStorage.DoMutate(data =>
            {
                var id = _nextId;
                var dataWithId = SetId(aggregate, _nextId);
                data.Add(dataWithId);
                return Task.FromResult(id);
            });
        }

        /// <summary>
        /// See <see cref="ICrudAsyncRepository.Update"/>
        /// </summary>
        public Task Update([NotNull] TData aggregate)
        {
            if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));
            var aggregateId = GetId(aggregate);
            Action<IList<TData>> sideEffect = (IList<TData>  data) => {
                var recordToRemove = data.FirstOrDefault(i => GetId(i) == aggregateId);
                data.Remove(recordToRemove);
                data.Add(aggregate);
            };
            _memoryStorage.DoMutate(sideEffect);
            return Task.CompletedTask;
        }
    }
}
