using System;
using System.Collections;
using System.Collections.Generic;

namespace MangaDexSharp.Collections
{
    /// <summary>
    /// Represents page of <seealso cref="IPaginatedCollection{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of items inside page</typeparam>
    public sealed class CollectionPage<T> : IReadOnlyCollection<T>
    {
        private IReadOnlyCollection<T> _data;

        /// <inheritdoc/>
        public int Count => _data.Count;

        /// <summary>
        /// Current page number
        /// </summary>
        public int Page { get; }

        /// <summary>
        /// Initializes new page
        /// </summary>
        /// <param name="collection">Collection of items to be inside the page</param>
        /// <param name="currentPage">Page number</param>
        public CollectionPage(IEnumerable<T> collection, int currentPage)
        {
            _data = new List<T>(collection);
            Page = currentPage;
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
