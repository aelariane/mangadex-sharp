using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Internal;
using MangaDexSharp.Internal.Dto;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Parameters.Follows;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Api
{
    /// <summary>
    /// Provides iteractions for <see cref="MangaDexClient.CurrentUser"/>, related to follows
    /// </summary>
    /// <remarks>This api should only be used if <see cref="MangaDexClient.CurrentUser"/> is not null</remarks>
    public class FollowsApi : MangaDexApi
    {
        internal FollowsApi(MangaDexClient client) : base(client)
        {
            BaseApiPath = MangaDexApiPath + "/user/follows";
        }


        /// <summary>
        /// Checks if <seealso cref="MangaDexClient.CurrentUser"/> is followed to specific <seealso cref="ScanlationGroup"/>
        /// </summary>
        /// <param name="groupId">Id of <seealso cref="ScanlationGroup"/></param>
        /// <param name="cancelToken"></param>
        /// <returns>true if follows, false otherwise</returns>
        public async Task<bool> CheckGroupFollow(Guid groupId, CancellationToken cancelToken = default)
        {
            try
            {
                await GetRequest<MangaDexResponse>(
                   BaseApiPath + "/group/" + groupId,
                   cancelToken,
                   true);
                return true;
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                throw;
            }
        }

        /// <summary>
        /// Checks if <seealso cref="MangaDexClient.CurrentUser"/> is followed to specific <seealso cref="Manga"/>
        /// </summary>
        /// <param name="mangaId">Id of <seealso cref="Manga"/></param>
        /// <param name="cancelToken"></param>
        /// <returns>true if follows, false otherwise</returns>
        public async Task<bool> CheckMangaFollow(Guid mangaId, CancellationToken cancelToken = default)
        {
            try
            {
                await GetRequest<MangaDexResponse>(
                   BaseApiPath + "/manga/" + mangaId,
                   cancelToken,
                   true);
                return true;
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                throw;
            }
        }

        /// <summary>
        /// Checks if <seealso cref="MangaDexClient.CurrentUser"/> is followed to specific <seealso cref="User"/>
        /// </summary>
        /// <param name="userId">Id of <seealso cref="User"/></param>
        /// <param name="cancelToken"></param>
        /// <returns>true if follows, false otherwise</returns>
        public async Task<bool> CheckUserFollow(Guid userId, CancellationToken cancelToken = default)
        {
            try
            {
                await GetRequest<MangaDexResponse>(
                   BaseApiPath + "/user/" + userId,
                   cancelToken,
                   true);
                return true;
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                throw;
            }
        }

        /// <summary>
        /// Gets list of <seealso cref="ScanlationGroup"/> which <seealso cref="MangaDexClient.CurrentUser"/> follows
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<ResourceCollection<ScanlationGroup>> GetFollowedGroups(GetFollowedGroupsParameters? parameters = null, CancellationToken cancelToken = default)
        {
            CollectionResponse<ScanlationGroupDto> response = await GetCollectionRequest<ScanlationGroupDto>(
                BaseApiPath + "/group",
                parameters,
                cancelToken,
                true);

            return mangaDexClient.Resources.MapResponseCollection<ScanlationGroupDto, ScanlationGroup>(response);
        }

        /// <summary>
        /// Gets list of <seealso cref="Manga"/> which <seealso cref="MangaDexClient.CurrentUser"/> follows
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<ResourceCollection<Manga>> GetFollowedManga(GetFollowedMangaParameters? parameters = null, CancellationToken cancelToken = default)
        {
            CollectionResponse<MangaDto> response = await GetCollectionRequest<MangaDto>(
                BaseApiPath + "/manga",
                parameters,
                cancelToken,
                true);

            return mangaDexClient.Resources.MapResponseCollection<MangaDto, Manga>(response);
        }

        /// <summary>
        /// Gets list of <seealso cref="Chapter"/> for all followed <seealso cref="Manga"/> of <seealso cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<ResourceCollection<Chapter>> GetFollowedMangaFeed(
            GetFollowedMangaFeedParameters? parameters,
            CancellationToken cancelToken = default)
        {
            CollectionResponse<ChapterDto> dto = await GetCollectionRequest<ChapterDto>(
               BaseApiPath + "/manga/feed",
               parameters,
               cancelToken,
               true);

            return mangaDexClient.Resources.MapResponseCollection<ChapterDto, Chapter>(dto);
        }

        /// <summary>
        /// Gets list of <seealso cref="User"/> which <seealso cref="MangaDexClient.CurrentUser"/> follows
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<ResourceCollection<User>> GetFollowedUsers(GetFollowedUsersParameters? parameters = null, CancellationToken cancelToken = default)
        {
            CollectionResponse<UserDto> response = await GetCollectionRequest<UserDto>(
                BaseApiPath + "/follows/user",
                parameters,
                cancelToken,
                true);

            return mangaDexClient.Resources.MapResponseCollection<UserDto, User>(response);
        }
    }
}
