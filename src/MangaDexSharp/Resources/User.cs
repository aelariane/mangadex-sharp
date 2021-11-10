using MangaDexSharp.Collections;
using MangaDexSharp.Objects.Feed;
using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Chapter;
using MangaDexSharp.Parameters.Enums;
using MangaDexSharp.Parameters.Order.Chapter;
using MangaDexSharp.Parameters.ScanlationGroup;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MangaDexSharp.Resources
{
    /// <summary>
    /// Represents User of MangaDex
    /// </summary>
    public class User : MangaDexResource 
    {
        private bool _noGroups;

        internal HashSet<Guid> RelatedGroupIds { get; } = new HashSet<Guid>();

        /// <summary>
        /// Name of User
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Roles of User
        /// </summary>
        public IEnumerable<string> Roles { get; }

        /// <summary>
        /// Collection of Id of <see cref="ScanlationGroup"/> where User has membership or is leader
        /// </summary>
        public IReadOnlyCollection<Guid> GroupIds => RelatedGroupIds;

        /// <summary>
        /// If User is member of any <seealso cref="ScanlationGroup"/>
        /// </summary>
        public bool IsMemberOfAnyGroup => RelatedGroupIds.Count > 0;

        internal User(
            MangaDexClient client,
            Guid id, 
            string username, 
            IEnumerable<string> roles)
            : base(client, id)
        {
            Username = username;
            Roles = roles;
        }

        /// <summary>
        /// Gets collection of <seealso cref="Chapter"/> uploaded by the User, in form of Feed
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<MangaFeed> GetUploadedManga(CancellationToken cancelToken = default)
        {
            var chaptersOrder = new GetChapterListOrderParameters();
            chaptersOrder.CreatedAt = OrderByType.Descending;

            var parameters = new GetChapterListParameters(chaptersOrder);
            parameters.ApplySettings(Client.Settings);
            parameters.Amount = Client.Settings.ItemsPerPage;
            parameters.Uploader = Id;
            parameters.Includes = new IncludeParameters()
            {
                IncludeManga = true,
                IncludeScanlationGroup = true,
                IncludeUser = true
            };

            ResourceCollection<Chapter> initialFeed = await Client.Chapter.GetList(parameters, cancelToken);

            var feed = new MangaFeed(
                Client,
                initialFeed,
                Client.Chapter.BaseApiPath,
                parameters);
            await feed.InitializeFirstPage(cancelToken);

            return feed;
        }

        /// <summary>
        /// Gets collection of <seealso cref="ScanlationGroup"/> where the User is member or leader
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<ScanlationGroup>> GetUserGroups(CancellationToken cancelToken = default)
        {
            if(RelatedGroupIds.Count == 0)
            {
                if (_noGroups)
                {
                    return new List<ScanlationGroup>();
                }
                User user = await Client.User.GetUser(Id, null, cancelToken);
                if (user.RelatedGroupIds.Count == 0)
                {
                    _noGroups = true;
                }
                return await GetUserGroups(cancelToken);
            }

            if(TryGetRelationCollection(RelatedGroupIds, out List<ScanlationGroup> groups))
            {
                return groups;
            }

            var parameters = new GetGroupListParameters();
            parameters.Amount = RelatedGroupIds.Count;
            parameters.GroupIds = RelatedGroupIds;

            ResourceCollection<ScanlationGroup> groupResult = await Client.ScanlationGroup.GetList(parameters, cancelToken);

            foreach(ScanlationGroup group in groupResult)
            {
                RegisterRelation(group);
            }

            return groupResult;
        }
    }
}
