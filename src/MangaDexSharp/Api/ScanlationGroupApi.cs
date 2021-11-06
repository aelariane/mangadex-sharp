using System;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Exceptions;
using MangaDexSharp.Internal;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Parameters.ScanlationGroup;
using MangaDexSharp.Resources;
using MangaDexSharp.Internal.Dto;
using MangaDexSharp.Parameters;

namespace MangaDexSharp.Api
{
    /// <summary>
    /// Provides access to api endpoints related to <seealso cref="ScanlationGroup"/>
    /// </summary>
    /// <remarks>Learn more here: https://api.mangadex.org/docs.html#tag/ScanlationGroup </remarks>
    public class ScanlationGroupApi : MangaDexApi
    {
        internal ScanlationGroupApi(MangaDexClient client) : base(client)
        {
            BaseApiPath = MangaDexApiPath + "/group";
        }

        /// <summary>
        /// Follows <see cref="MangaDexClient.CurrentUser"/> to specifc <seealso cref="ScanlationGroup"/>
        /// </summary>
        /// <param name="groupId">Id of <seealso cref="ScanlationGroup"/> to follow</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task FollowGroup(Guid groupId, CancellationToken cancelToken = default)
        {
            await PostRequest<MangaDexResponse>(
              BaseApiPath + "/" + groupId + "/follow",
              cancelToken);
        }

        /// <summary>
        /// Gets list of <seealso cref="ScanlationGroup"/>
        /// </summary>
        /// <param name="parameters">Additional filter parameters</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="ForbiddenException"></exception>
        public async Task<ResourceCollection<ScanlationGroup>> GetList(
            GetGroupListParameters? parameters = null,
            CancellationToken cancelToken = default)
        {
            CollectionResponse<ScanlationGroupDto> response = await GetCollectionRequest<ScanlationGroupDto>(
                BaseApiPath,
                parameters,
                cancelToken);

            return mangaDexClient.Resources.MapResponseCollection<ScanlationGroupDto, ScanlationGroup>(response);
        }

        /// <summary>
        /// Follows <see cref="MangaDexClient.CurrentUser"/> to specifc <seealso cref="ScanlationGroup"/>
        /// </summary>
        /// <param name="groupId">Id of <seealso cref="ScanlationGroup"/> to unfollow</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task UnfollowGroup(Guid groupId, CancellationToken cancelToken = default)
        {
            await PostRequest<MangaDexResponse>(
              BaseApiPath + "/" + groupId + "/follow",
              cancelToken);
        }


        /// <summary>
        /// Gets information about <seealso cref="ScanlationGroup"/> with provided id
        /// </summary>
        /// <param name="groupId">Group Id</param>
        /// <param name="includes"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="ForbiddenException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ScanlationGroup> ViewGroup(Guid groupId, IncludeParameters? includes, CancellationToken cancelToken = default)
        {
            ScanlationGroupDto response = await GetObjectRequest<ScanlationGroupDto>(
                BaseApiPath + "/" + groupId,
                includes,
                cancelToken);

            if (mangaDexClient.Resources.TryRetrieve(response, out ScanlationGroup? result) && result != null)
            {
                return result;
            }
            throw new Exception($"Cannot retrieve {nameof(ScanlationGroup)} with Id {response.Id}");
        }
    }
}
