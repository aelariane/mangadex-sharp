using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Internal.Dto;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Parameters;

namespace MangaDexSharp.Collections.Internal
{
    internal class SimpleResourceRetriever<TDto, TResource> : IResourceRetriever<TResource>
        where TDto : ResourceDto
        where TResource : MangaDexResource
    {
        private MangaDexClient _client;

        internal SimpleResourceRetriever(MangaDexClient client)
        {
            _client = client;
        }

        public async Task<ResourceCollection<TResource>> GetAsync(string endpoint, IQueryParameters? parameters, bool requireAuth, CancellationToken cancelToken = default)
        {
            var result = await _client.AtHome.GetCollectionRequest<TDto>(
                endpoint,
                parameters,
                cancelToken,
                requireAuth);

            return _client.Resources.MapResponseCollection<TDto, TResource>(result);
        }
    }
}
