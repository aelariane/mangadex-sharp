using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Collections.Internal;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Objects;
using MangaDexSharp.Objects.Feed;
using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Chapter;
using MangaDexSharp.Parameters.Enums;
using MangaDexSharp.Parameters.Manga;
using MangaDexSharp.Parameters.Order.Chapter;

namespace MangaDexSharp.Resources
{
    public class ScanlationGroup : MangaDexAuditableResource
    {
        private bool _noLeader;
        private bool _noMembers;
        internal string? DiscordCode { get; set; }
        internal HashSet<Guid> RelatedMemberIds { get; } = new HashSet<Guid>();
        internal Guid? RelatedLeaderId { get; set; }

        /// <summary>
        /// Alternative names of the Group
        /// </summary>
        public IEnumerable<LocalizedString> AlternativeNames { get; }

        /// <summary>
        /// Contact email of the Group
        /// </summary>
        public string? ContactEmail { get; internal set; }

        /// <summary>
        /// Description of the Group
        /// </summary>
        public string? Description { get; internal set; }

        /// <summary>
        /// Discord invite to the discord server of the Group
        /// </summary>
        public Uri? Discord => Discord == null ? null : new Uri($"https://discord.gg/{DiscordCode}");

        /// <summary>
        /// Languages the Group specifies on
        /// </summary>
        public IEnumerable<string>? FocusedLanguages { get; internal set; }

        public string? IrcChannel { get; internal set; }

        public string? IrcServer { get; internal set; }

        /// <summary>
        /// Id of <seealso cref="User"/> who is leader of the Group
        /// </summary>
        public Guid? LeaderId => RelatedLeaderId;

        public bool Locked { get;  }

        /// <summary>
        /// Ids of <seealso cref="User"/> who are members of the Group
        /// </summary>
        public IEnumerable<Guid> MemberIds => RelatedMemberIds;

        /// <summary>
        /// Main name of the Group
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// If the Group is official
        /// </summary>
        public bool Official { get; }

        /// <summary>
        /// Url to the Group's website
        /// </summary>
        public string? Website { get; internal set; }

        internal ScanlationGroup(
            MangaDexClient client,
            Guid id,
            string name,
            IEnumerable<LocalizedString> altNames,
            bool official,
            bool locked,
            DateTime createdAt,
            DateTime updatedAt) 
            : base(client, id, createdAt, updatedAt)
        {
            Name = name;
            AlternativeNames = altNames;
            Official = official;
            Locked = locked;
        }


        /// <summary>
        /// Gets feed of the Group
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<MangaFeed> GetFeed(CancellationToken cancelToken = default)
        {
            var chaptersOrder = new GetChapterListOrderParameters();
            chaptersOrder.CreatedAt = OrderByType.Descending;

            var parameters = new GetChapterListParameters(chaptersOrder);
            parameters.ApplySettings(Client.Settings);
            parameters.Amount = Client.Settings.ItemsPerPage;
            parameters.Groups = new List<Guid>() { Id };
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
        /// Gets <seealso cref="User"/> who is marked as leader of the Group
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>null if leader is not presented</returns>
        public async Task<User?> GetLeader(CancellationToken cancelToken = default)
        {
            if(RelatedLeaderId == null)
            {
                if (_noLeader)
                {
                    return null;
                }
                var includes = new IncludeParameters() 
                { 
                    IncludeLeader = true,
                    IncludeMembers = true
                };
                ScanlationGroup group = await Client.ScanlationGroup.ViewGroup(Id, includes, cancelToken);
                if(group.RelatedLeaderId == null)
                {
                    _noLeader = true;
                }
                return await GetLeader(cancelToken);
            }
            if(TryGetRelation(RelatedLeaderId.Value, out User? leader) && leader != null)
            {
                return leader;
            }

            leader = await Client.User.GetUser(RelatedLeaderId.Value, null, cancelToken);
            RegisterRelation(leader);

            return leader;
        }

        /// <summary>
        /// Gets list of <seealso cref="User"/> who are members of the Group
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>Collection of <seealso cref="User"/> who are members of the Group</returns>
        public async Task<IReadOnlyCollection<User>> GetMembers(CancellationToken cancelToken = default)
        {
            var result = new List<User>();
            if (RelatedMemberIds.Count == 0)
            {
                if (_noMembers)
                {
                    return result;
                }
                var includes = new IncludeParameters()
                {
                    IncludeLeader = true,
                    IncludeMembers = true
                };
                ScanlationGroup group = await Client.ScanlationGroup.ViewGroup(Id, includes, cancelToken);
                if (group.RelatedMemberIds == null)
                {
                    _noMembers = true;
                }
                return await GetMembers(cancelToken);
            }

            if(TryGetRelationCollection(RelatedMemberIds, out result))
            {
                return result;
            }

            var parameters = new IncludeParameters()
            {
                IncludeLeader = true,
                IncludeMembers = true
            };
            ScanlationGroupDto myDto = await Client.ScanlationGroup.GetObjectRequest<ScanlationGroupDto>(
                Client.ScanlationGroup.BaseApiPath + "/" + Id,
                parameters,
                cancelToken,
                false);

            if(myDto.MemberRelations == null || myDto.MemberRelations.Any() == false)
            {
                return result;
            }

            foreach (ResourceDto dto in myDto.MemberRelations)
            {
                if (Client.Resources.TryRetrieve(dto.Id, out User? member) && member != null)
                {
                    result.Add(member);
                    RegisterRelation(member);
                }
            }

            return result;
        }
    }
}
