using System;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Collections.Internal;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Follows;
using MangaDexSharp.Parameters.ScanlationGroup;

namespace MangaDexSharp.Resources
{
    /// <summary>
    /// Represents <seealso cref="User"/> who currently is logged in at <seealso cref="MangaDexClient"/>
    /// </summary>
    /// <remarks>Provides set of operations which should be executed only on local user</remarks>
    public class LocalUser : User
    {
        internal LocalUser(User baseOn)
            : base(
                  baseOn.Client,
                  baseOn.Id,
                  baseOn.Username,
                  baseOn.Roles)
        {
            Version = baseOn.Version;
            foreach (Guid id in baseOn.RelatedGroupIds)
            {
                RelatedGroupIds.Add(id);
            }
        }

        /// <summary>
        /// Follows User to provided <seealso cref="ScanlationGroup"/>
        /// </summary>
        /// <param name="followTo">Group to follow</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task FollowGroup(ScanlationGroup followTo, CancellationToken cancelToken = default)
        {
            await Client.ScanlationGroup.FollowGroup(followTo.Id, cancelToken);
        }

        /// <summary>
        /// Follows User to provided <seealso cref="Manga"/>
        /// </summary>
        /// <param name="followTo">Manga to follow</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task FollowManga(Manga followTo, CancellationToken cancelToken = default)
        {
            await Client.Manga.FollowManga(followTo.Id, cancelToken);
        }

        /// <summary>
        /// Gets list of <seealso cref="ScanlationGroup"/> that User follows
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>Paginated collection of <seealso cref="ScanlationGroup"/></returns>
        public async Task<IFixedPaginatedCollection<ScanlationGroup>> GetFollowedGroups(CancellationToken cancelToken = default)
        {
            var parameters = Client.CreateParameters<GetFollowedGroupsParameters>();
            parameters.Amount = Client.Settings.ItemsPerPage; 
            parameters.Includes = new IncludeParameters()
            {
                IncludeCover = true
            };

            var collection = await Client.Follows.GetFollowedGroups(parameters, cancelToken);
            return new PaginatedCollection<ScanlationGroupDto, ScanlationGroup>(
                Client,
                collection,
                Client.Follows.BaseApiPath + "/group",
                parameters,
                collection.Total,
                true);
        }

        /// <summary>
        /// Gets list of <seealso cref="Manga"/> that User follows
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>Paginated collection of <seealso cref="Manga"/></returns>
        public async Task<IFixedPaginatedCollection<Manga>> GetFollowedManga(CancellationToken cancelToken = default)
        {
            var parameters = Client.CreateParameters<GetFollowedMangaParameters>();
            parameters.Amount = Client.Settings.ItemsPerPage;
            parameters.Includes = new IncludeParameters()
            {
                IncludeCover = true
            };

            var collection = await Client.Follows.GetFollowedManga(parameters, cancelToken);
            return new PaginatedCollection<MangaDto, Manga>(
                Client,
                collection,
                Client.Follows.BaseApiPath + "/manga",
                parameters,
                collection.Total,
                true);
        }

        /// <summary>
        /// Checks if <seealso cref="ScanlationGroup"/> is followed by the User
        /// </summary>
        /// <param name="group">Group to check</param>
        /// <param name="cancelToken"></param>
        /// <returns>true if followed; false otherwise</returns>
        public async Task<bool> IsGroupFollowed(ScanlationGroup group, CancellationToken cancelToken = default)
        {
            return await Client.Follows.CheckGroupFollow(group.Id, cancelToken);
        }

        /// <summary>
        /// Checks if <seealso cref="Manga"/> is followed by the User
        /// </summary>
        /// <param name="manga">Manga to check</param>
        /// <param name="cancelToken"></param>
        /// <returns>true if followed; false otherwise</returns>
        public async Task<bool> IsMangaFollowed(Manga manga, CancellationToken cancelToken = default)
        {
            return await Client.Follows.CheckMangaFollow(manga.Id, cancelToken);
        }

        /// <summary>
        /// Unfollows User from provided <seealso cref="ScanlationGroup"/>
        /// </summary>
        /// <param name="unfollowFrom">Group to unfollow from</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task UnfollowGroup(ScanlationGroup unfollowFrom, CancellationToken cancelToken = default)
        {
            await Client.ScanlationGroup.UnfollowGroup(unfollowFrom.Id, cancelToken);
        }

        /// <summary>
        /// Unfollows User from provided <seealso cref="Manga"/>
        /// </summary>
        /// <param name="unfollowFrom">Manga to unfollow from</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task UnfollowManga(Manga unfollowFrom, CancellationToken cancelToken = default)
        {
            await Client.Manga.UnfollowManga(unfollowFrom.Id, cancelToken);
        }
    }
}
