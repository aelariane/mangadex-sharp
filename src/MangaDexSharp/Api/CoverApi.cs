using System;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Exceptions;
using MangaDexSharp.Internal;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Parameters.Cover;
using MangaDexSharp.Resources;
using MangaDexSharp.Internal.Dto;
using MangaDexSharp.Parameters;
using MangaDexSharp.Collections;

namespace MangaDexSharp.Api
{
    public class CoverApi : MangaDexApi
    {
        internal CoverApi(MangaDexClient client) : base(client)
        {
            BaseApiPath = MangaDexApiPath + "/cover";
        }

        /// <summary>
        /// Gets information about <seealso cref="CoverArt"/>r with provided id
        /// </summary>
        /// <param name="coverId">Cover Id</param>
        /// <param name="includes"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="ForbiddenException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<CoverArt> GetCover(Guid coverId, IncludeParameters? includes, CancellationToken cancelToken = default)
        {
            CoverArtDto response = await GetObjectRequest<CoverArtDto>(
                BaseApiPath + "/" + coverId,
                includes,
                cancelToken);

            if (mangaDexClient.Resources.TryRetrieve(response, out CoverArt? result) && result != null)
            {
                return result;
            }
            throw new Exception($"Cannot retrieve cover with Id {response.Id}");
        }

        /// <summary>
        /// Gets list of <seealso cref="CoverArt"/>
        /// </summary>
        /// <param name="parameters">Additional filter parameters</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="ForbiddenException"></exception>
        public async Task<ResourceCollection<CoverArt>> GetList(
            GetCoverListParameters? parameters = null,
            CancellationToken cancelToken = default)
        {
            CollectionResponse<CoverArtDto> response = await GetCollectionRequest<CoverArtDto>(
                BaseApiPath,
                parameters,
                cancelToken);

            return mangaDexClient.Resources.MapResponseCollection<CoverArtDto, CoverArt>(response);
        }
    }
}
