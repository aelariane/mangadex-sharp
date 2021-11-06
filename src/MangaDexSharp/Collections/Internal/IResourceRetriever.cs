using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Parameters;

namespace MangaDexSharp.Collections.Internal
{
    internal interface IResourceRetriever<TResource>
        where TResource : MangaDexResource
    {
        Task<ResourceCollection<TResource>> GetAsync(
            string endpoint,
            IQueryParameters? parameters,
            bool requireAuth,
            CancellationToken cancelToken = default);
    }
}
