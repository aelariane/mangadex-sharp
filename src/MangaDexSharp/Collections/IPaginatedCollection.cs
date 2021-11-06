using System;
using System.Threading;
using System.Threading.Tasks;

namespace MangaDexSharp.Collections
{
    /// <summary>
    /// Defines collection that is paginated
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public interface IPaginatedCollection<T>
    {
        /// <summary>
        /// Gets current page
        /// </summary>
        CollectionPage<T> CurrentPage { get; }

        /// <summary>
        /// Navigates collection to specific the page
        /// </summary>
        /// <param name="page">Page to navigate</param>
        /// <returns>Requesed page</returns>
        Task<CollectionPage<T>> NavigateTo(int page, CancellationToken cancelToken = default);

        /// <summary>
        /// Gets next page of collection
        /// </summary>
        /// <returns>Next page</returns>
        Task<CollectionPage<T>> NextPage(CancellationToken cancelToken = default);

        /// <summary>
        /// Current page number
        /// </summary>
        /// <remarks>Start from 1</remarks>
        int Page { get; }

        /// <summary>
        /// Gets prvious page of collection
        /// </summary>
        /// <returns>Previous page</returns>
        Task<CollectionPage<T>> PreviousPage(CancellationToken cancelToken = default);

        /// <summary>
        /// Total amount of pages in collection
        /// </summary>
        int TotalPages { get; }
    }
}
