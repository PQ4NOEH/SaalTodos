using Saal.Todos.Repositories.Base;
using System;
using System.Collections.Generic;

namespace Saal.Todos.Repositories.Tests.Fakes
{
    public class InMemoryStorageFake<TData> : IInMemoryStorage<TData> where TData : class
    {
        public IList<TData> Data { get; } =  new List<TData>();

        public void DoMutate(Action<IList<TData>> sideEffect) => sideEffect(Data);

        public TResult DoMutate<TResult>(Func<IList<TData>, TResult> sideEffect) => sideEffect(Data);
        
    }
}
