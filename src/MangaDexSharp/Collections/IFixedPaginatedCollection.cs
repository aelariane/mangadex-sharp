using System;

namespace MangaDexSharp.Collections
{
    /// <summary>
    /// Defines paginated collection, where each page has fixed amount of items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFixedPaginatedCollection<T> : IPaginatedCollection<T>
    {
        /// <summary>
        /// Expected amount of items in every page
        /// </summary>
        /// <remarks>Last page may contain less then this amount, if not full</remarks>
        int ItemsPerPage { get; }
    }
}
