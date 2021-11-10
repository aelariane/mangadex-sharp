using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Api.Data;
using MangaDexSharp.Collections;
using MangaDexSharp.Collections.Internal;
using MangaDexSharp.Enums;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Objects;
using MangaDexSharp.Objects.Feed;
using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Author;
using MangaDexSharp.Parameters.Enums;
using MangaDexSharp.Parameters.Manga;
using MangaDexSharp.Parameters.Order.Manga;

namespace MangaDexSharp.Resources
{
    /// <summary>
    /// Represents Manga
    /// </summary>
    public class Manga : MangaDexAuditableResource
    {
        private bool _noArtists;
        private bool _noAuthors;
        private bool _noMainCover;
        private bool _statusFetched = false;
        private MangaReadingStatus? _status;


        internal Dictionary<string, string> LinksDictionary { get; set; }
        internal HashSet<Guid> RelatedAuthorIds { get; } = new HashSet<Guid>();
        internal HashSet<Guid> RelatedArtistIds { get; } = new HashSet<Guid>();
        internal Dictionary<Guid, MangaRelation> RelatedMangaIds { get; } = new Dictionary<Guid, MangaRelation>();
        internal Guid MainCoverId { get; set; }

        /// <summary>
        /// Alternative titles of Manga
        /// </summary>
        public IEnumerable<LocalizedString> AlternaiveTitles { get; }

        /// <summary>
        /// Collection of Id of <see cref="Author"/> that marked as Artists of the Manga
        /// </summary>
        public IEnumerable<Guid> ArtistIds => RelatedArtistIds;

        /// <summary>
        /// Collection of Id of <see cref="Author"/> that marked as Authors of the Manga
        /// </summary>
        public IEnumerable<Guid> AuthorIds => RelatedAuthorIds;

        /// <summary>
        /// Content rating of Manga
        /// </summary>
        public ContentRating ContentRating { get; }

        /// <summary>
        /// Description of Manga
        /// </summary>
        public LocalizedString Description { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLocked { get; }

        /// <summary>
        /// List of links to other resources where Manga can be found
        /// </summary>
        public MangaLinks Links { get; }

        /// <summary>
        /// Last chapter of manga
        /// </summary>
        /// <remarks>Optional</remarks>
        public string? LastChapter { get; internal set; }

        /// <summary>
        /// Last released volume of Manga
        /// </summary>
        /// <remarks>Optional</remarks>
        public string? LastVolume { get; internal set; }

        /// <summary>
        /// Original language of Manga
        /// </summary>
        public string OriginalLanguage { get; }

        /// <summary>
        /// Target audience of Manga
        /// </summary>
        /// <remarks>Optional</remarks>
        /// 
        public MangaPublicationDemographic? PublicationDemographic { get; internal set; }

        /// <summary>
        /// Collection of related <see cref="Manga"/>
        /// </summary>
        /// <remarks>Also specifies type of relation</remarks>
        public IReadOnlyCollection<KeyValuePair<Guid, MangaRelation>> RelatedMangas => RelatedMangaIds;

        /// <summary>
        /// Current status of Manga
        /// </summary>
        /// <remarks>Optional</remarks>
        public MangaStatus? Status { get; internal set; }

        /// <summary>
        /// Manga tags
        /// </summary>
        public IReadOnlyCollection<Tag>? Tags { get; }

        /// <summary>
        /// Main title of Manga
        /// </summary>
        public LocalizedString Title { get; }

        /// <summary>
        /// Release year of Manga
        /// </summary>
        /// <remarks>Optional</remarks>
        public int? Year { get; internal set; }

        /// <summary>
        /// Id of last volume <see cref="CoverArt"/>
        /// </summary>
        public Guid MainCoverArtId => MainCoverId;

        internal Manga(
            MangaDexClient client,
            Guid id,
            LocalizedString title,
            IEnumerable<LocalizedString> altTitles,
            LocalizedString description,
            bool isLocked,
            IReadOnlyDictionary<string, string>? links,
            string originalLanguage,
            ContentRating contentRating,
            IEnumerable<Tag>? tags,
            DateTime createdAt,
            DateTime updatedAt)
            : base(client, id, createdAt, updatedAt)
        {
            Title = title;
            AlternaiveTitles =  altTitles;
            Description = description;
            IsLocked = isLocked;
            if(links == null)
            {
                LinksDictionary = new Dictionary<string, string>();
            }
            else
            {
                LinksDictionary = new Dictionary<string, string>(links);
            }
            OriginalLanguage = originalLanguage;
            ContentRating = contentRating;
            Tags = tags == null ? new List<Tag>() : null;

            Links = new MangaLinks(this);
        }

        /// <summary>
        /// Follows <see cref="MangaDexClient.CurrentUser"/> to the Manga
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task Follow(CancellationToken cancelToken = default)
        {
            await Client.Manga.FollowManga(Id, cancelToken);
        }


        /// <summary>
        /// Gets list of <seealso cref="Author"/> who are mentioned as drawers for the Manga
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>Collection of <seealso cref="Author"/></returns>
        public async Task<IReadOnlyCollection<Author>> GetArtists(CancellationToken cancelToken = default)
        {
            if (RelatedArtistIds.Any() == false)
            {
                if (_noArtists)
                {
                    return new List<Author>();
                }
                Manga manga = await Client.Manga.ViewManga(Id, null, cancelToken);
                if(manga.RelatedArtistIds.Count == 0)
                {
                    _noArtists = true;
                }
                return await GetArtists(cancelToken);
            }

            if(TryGetRelationCollection(RelatedArtistIds, out List<Author> artists))
            {
                foreach(Author artist in artists)
                {
                    artist.IsArtist = true;
                }
                return artists;
            }

            var parameters = new GetAuthorListParameters()
            {
                AuthorIds = new List<Guid>(RelatedArtistIds)
            };

            ResourceCollection<Author> artistsResult = await Client.Author.GeList(parameters, cancelToken);

            foreach(Author artist in artistsResult)
            {
                RegisterRelation(artist);
                artist.IsArtist = true;
            }

            return artistsResult;
        }

        /// <summary>
        /// Gets list of <seealso cref="Author"/> involved in creation of the Manga
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>Collection of <seealso cref="Author"/></returns>
        public async Task<IReadOnlyCollection<Author>> GetAuthors(CancellationToken cancelToken = default)
        {
            if (RelatedAuthorIds.Any() == false)
            {
                if (_noAuthors)
                {
                    return new List<Author>();
                }
                Manga manga = await Client.Manga.ViewManga(Id, null, cancelToken);
                if (manga.RelatedAuthorIds.Count == 0)
                {
                    _noAuthors = true;
                }
                return await GetArtists(cancelToken);
            }

            if (TryGetRelationCollection(RelatedAuthorIds, out List<Author> artists))
            {
                return artists;
            }

            var parameters = new GetAuthorListParameters()
            {
                AuthorIds = new List<Guid>(RelatedAuthorIds)
            };

            ResourceCollection<Author> authorResult = await Client.Author.GeList(parameters, cancelToken);

            foreach (Author author in authorResult)
            {
                RegisterRelation(author);
            }

            return authorResult;
        }

        /// <summary>
        /// Gets feed of the Manga
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<ChapterFeed> GetFeed(OrderByType order = OrderByType.Descending, CancellationToken cancelToken = default)
        {
            var feedOrder = new GetMangaFeedOrderParameters();
            feedOrder.Chapter = order;
            feedOrder.Volume = order;

            GetMangaFeedParameters parameters = new GetMangaFeedParameters(feedOrder);
            parameters.ApplySettings(Client.Settings);
            parameters.Amount = Client.Settings.ItemsPerPage * Client.Settings.ChapterListMultiplier;
            parameters.Includes = new IncludeParameters()
            {
                IncludeManga = true,
                IncludeScanlationGroup = true,
                IncludeUser = true
            };

            ResourceCollection<Chapter> initialFeed = await Client.Manga.GetMangaFeed(Id, parameters, cancelToken);

            var feed = new ChapterFeed(
                Client,
                this,
                initialFeed,
                Client.Manga.BaseApiPath + "/" + Id + "/feed",
                parameters);
            await feed.InitializeFirstPage(cancelToken);

            return feed;
        }

        /// <summary>
        /// Gets <seealso cref="CoverArt"/> that represents art for last released volume of the Manga
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<CoverArt> GetMainCover(CancellationToken cancelToken = default)
        {
            if(MainCoverArtId == Guid.Empty)
            {
                await Client.Manga.ViewManga(Id, null, cancelToken);
            }
            if(TryGetRelation(MainCoverArtId, out CoverArt? cover) && cover != null)
            {
                return cover;
            }
            cover = await Client.Cover.GetCover(MainCoverArtId, null, cancelToken);
            RegisterRelation(cover);

            return cover;
        }

        public async Task<IReadOnlyCollection<Guid>> GetReadChaptersIds(CancellationToken cancelToken = default)
        {
            IReadOnlyCollection<Guid> result = await Client.Manga.GetReadMarkersOfManga(Id, cancelToken);
            return result;
        }

        /// <summary>
        /// Gets reading status of the Manga for <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="forceRenew">If cached status should be renewed forcefully</param>
        /// <param name="cancelToken"></param>
        /// <returns>Reading status; null if not in <seealso cref="MangaDexClient.CurrentUser"/>'s read list</returns>
        public async Task<MangaReadingStatus?> GetReadingStatus(bool forceRenew = false, CancellationToken cancelToken = default)
        {
            if (_statusFetched && !forceRenew)
            {
                return _status;
            }

            _status = await Client.Manga.GetMangaReadingStatus(Id, cancelToken);
            _statusFetched = true;
            return _status;
        }

        /// <summary>
        /// Gets list of <seealso cref="Manga"/> related to the Manga
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>Collection of related <seealso cref="Manga"/></returns>
        public async Task<IReadOnlyDictionary<Manga, MangaRelation>> GetRelatedMangas(CancellationToken cancelToken = default)
        {
            IReadOnlyDictionary<Guid, MangaRelation> relations =  await Client.Manga.GetRelatedMangas(Id, cancelToken);

            foreach(var pair in relations)
            {
                if(RelatedMangaIds.ContainsKey(pair.Key) == false)
                {
                    RelatedMangaIds.Add(pair.Key, pair.Value);
                }
            }

            var result = new Dictionary<Manga, MangaRelation>();
            if (relations.Count == 0)
            {
                return result;
            }

            var parameters = new GetMangaListParameters(new GetMangaListOrderParameters())
            {
                MangaIds = new List<Guid>(relations.Keys),
                ContentRatings = new ContentRating[]
                {
                    ContentRating.Safe,
                    ContentRating.Suggestive,
                    ContentRating.Erotica,
                    ContentRating.Pornographic
                },
                Amount = relations.Count
            };

            ResourceCollection<Manga> relatedMangas = await Client.Manga.GetList(parameters, cancelToken);

            foreach(Manga relatedManga in relatedMangas)
            {
                result.Add(relatedManga, relations[relatedManga.Id]);
            }

            return result;
        }

        /// <summary>
        /// Gets information about volumes of the Manga
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>Collection of Volumes</returns>
        public async Task<IReadOnlyCollection<Volume>> GetVolumes(CancellationToken cancelToken = default)
        {
            var aggregateOptions = Client.CreateParameters<AggregateMangaParameters>();

            IReadOnlyCollection<MangaVolumeInfo> volumeInfos = await Client.Manga.Aggregate(Id, aggregateOptions, cancelToken);

            return volumeInfos
                .Select(x =>
                    new Volume(Client, Id, x.Volume, x.Count))
                .ToList();
        }

        /// <summary>
        /// Unfollows <see cref="MangaDexClient.CurrentUser"/> from the Manga
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task Unfollow(CancellationToken cancelToken = default)
        {
            await Client.Manga.UnfollowManga(Id, cancelToken);
        }

        /// <summary>
        /// Updates reading status of the Manga for <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="status">New reading status of Manga (null to remove Manga from read list)</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task UpdateReadingStatus(MangaReadingStatus? status, CancellationToken cancelToken = default)
        {
            await Client.Manga.UpdateReadingStatus(Id, null, cancelToken);

            if(status == null)
            {
                _statusFetched = true;
                _status = null;
                return;
            }

            await Client.Manga.UpdateReadingStatus(Id, status, cancelToken);
            _statusFetched = true;
            _status = status;
        }
    }
}
