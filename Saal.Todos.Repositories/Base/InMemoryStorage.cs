using System;
using System.Collections.Generic;

namespace Saal.Todos.Repositories.Base
{
    public interface IInMemoryStorage<TData> where TData : class
    {
        IList<TData> Data { get;  }
        TResult DoMutate<TResult>(Func<IList<TData>, TResult> action);
        void DoMutate(Action<IList<TData>> sideEffect);
    }

    public class InMemoryStorage<TData> :IInMemoryStorage<TData> where TData : class
    {
        static object _dataLock = new object();
        static List<TData> _data = new List<TData>();
        public IList<TData> Data => _data;

        public void DoMutate(Action<IList<TData>> sideEffect)
        {
            lock (_dataLock)
            {
                sideEffect(_data);
            }
        }
        public TResult DoMutate<TResult>(Func<IList<TData>, TResult> sideEffect)
        {
            lock (_dataLock)
            {
                return sideEffect(_data);
            }
        }
    }
}
