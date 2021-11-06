using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Internal;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Internal.Dto;

namespace MangaDexSharp.Collections
{

    /// <summary>
    /// Represents response for requests returning collection of Resources
    /// </summary>
    /// <typeparam name="TResource">Resource type</typeparam>
    public sealed class ResourceCollection<TResource> : IReadOnlyCollection<TResource>, IEnumerable<TResource>
        where TResource : MangaDexResource
    {

        /// <summary>
        /// Collection of received resources
        /// </summary>
        private readonly IReadOnlyCollection<TResource> _data;

        /// <inheritdoc/>
        public int Count => _data.Count;
        /// <summary>
        /// Amount of resources
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Offset position
        /// </summary>
        public int Offset { get; }
        
        /// <summary>
        /// Total amount of resources
        /// </summary>
        public int Total { get; }

        internal ResourceCollection(
            IReadOnlyCollection<TResource> data,
            int limit,
            int offset,
            int total)
        {
            _data = data;
            Limit = limit;
            Offset = offset;
            Total = total;
        }
        /// <inheritdoc/>
        public IEnumerator<TResource> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_data).GetEnumerator();
        }
    }
}
