using System;
using System.Collections.Generic;

namespace Saal.Todos.Repositories.Base
{
    /// <summary>
    /// Data structure with pagedata
    /// </summary>
    /// <typeparam name="TData">Data type contained in the data structure</typeparam>
    public interface IPaginatedResult<TData>
        where TData : class
    {
        /// <summary>
        /// Total number of data type records (readonly)
        /// </summary>
        int NumberOfRecords { get; }

        /// <summary>
        /// Page number contained (readonly)
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Number of elements per page (readonly)
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Page data (readonly)
        /// </summary>
        IEnumerable<TData> PageData { get; }
    }

    /// <summary>
    /// see <see cref="IPaginatedResult"/>
    /// </summary>
    public class PaginatedResult<TData> : IPaginatedResult<TData>
        where TData : class
    {
        public PaginatedResult(int numberOfRecords, int pageNumber, int pageSize, IEnumerable<TData> pageData)
        {
            if (numberOfRecords < 0) throw new ArgumentOutOfRangeException(nameof(numberOfRecords));
            NumberOfRecords = numberOfRecords;

            if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber));
            PageNumber = pageNumber;

            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));
            PageSize = pageSize;

            PageData = pageData ?? throw new ArgumentNullException(nameof(pageData));
        }

        /// <summary>
        /// see <see cref="IPaginatedResult.NumberOfRecords"/>
        /// </summary>
        public int NumberOfRecords { get; }

        /// <summary>
        /// see <see cref="IPaginatedResult.PageNumber"/>
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// see <see cref="IPaginatedResult.PageSize"/>
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// see <see cref="IPaginatedResult.PageData"/>
        /// </summary>
        public IEnumerable<TData> PageData { get; }


    }
}
