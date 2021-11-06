using System;
using System.Collections.Generic;
using System.Linq;

namespace MangaDexSharp.Resources
{
    public static class ResourcesExtensions
    {
        /// <summary>
        /// Converts collection of Resource to collection of their Ids 
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="resources">Collection of resources</param>
        /// <returns></returns>
        public static IReadOnlyCollection<Guid> ToIdCollection<TResource>(IEnumerable<TResource> resources)
            where TResource : MangaDexResource
        {
            return new List<Guid>(resources.Select(r => r.Id));
        }
    }
}
