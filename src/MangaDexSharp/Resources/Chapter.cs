using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MangaDexSharp.Api.Data;
using MangaDexSharp.Collections;
using MangaDexSharp.Exceptions;
using MangaDexSharp.Objects;
using MangaDexSharp.Parameters.ScanlationGroup;

namespace MangaDexSharp.Resources
{
    /// <summary>
    /// Chapter of <see cref="Manga"/>
    /// </summary>
    public class Chapter : MangaDexAuditableResource
    {
        internal Guid RelatedMangaId { get; set; }
        internal HashSet<Guid> RelatedGroupIds { get; } = new HashSet<Guid>();
        internal Guid? RelatedUserId { get; set; }

        /// <summary>
        /// Chapter in string format
        /// </summary>
        /// <remarks>Optional</remarks>
        public string? ChapterName { get; internal set; }

        /// <summary>
        /// Count of readable images for the chapter
        /// </summary>
        public int Pages { get; }

        /// <summary>
        /// If is not present on MangaDex, refers to alternative source
        /// </summary>
        /// <remarks>Only valid if <seealso cref="Hash"/>, <seealso cref="Data"/>, <seealso cref="DataSaver"/> are empty</remarks>
        public Uri? ExternalUrl { get; internal set; }

        /// <summary>
        /// If Chapter only availible on external resource (<seealso cref="ExternalUrl"/>)
        /// </summary>
        public bool IsExternal => ExternalUrl != null;

        /// <summary>
        /// Id of <see cref="Manga"/> which Chapter belongs to
        /// </summary>
        public Guid MangaId => RelatedMangaId;

        /// <summary>
        /// Time of publishing Chapter
        /// </summary>
        public DateTime PublishAt { get; }

        /// <summary>
        /// Chapter's title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Language of Chapter
        /// </summary>
        public string TranslatedLanguage { get; }

        /// <summary>
        /// Id of <see cref="User"/> who uploaded the chapter
        /// </summary>
        public Guid? UploadedUserId => RelatedUserId;

        /// <summary>
        /// Volume this Chapter belongs to
        /// </summary>
        /// <remarks>Optional</remarks>
        public string? Volume { get; internal set; }

        internal Chapter(
            MangaDexClient client,
            Guid id,
            string title, 
            string translatedLanguage,
            DateTime createdAt,
            DateTime updatedAt,
            DateTime publishAt)
            : base(client, id, createdAt, updatedAt)
        {
            Title = title;
            TranslatedLanguage = translatedLanguage;
            PublishAt = publishAt;
        }

        /// <summary>
        /// Gets <see cref="User"/> who uploaded the chapter
        /// </summary>
        /// <returns>User who uploaded Chapter</returns>
        public async Task<User?> GetUploader(CancellationToken cancelToken = default)
        {
            if(RelatedUserId == null)
            {
                return null;
            }
            else if(TryGetRelation(RelatedUserId.Value, out User? user))
            {
                return user;
            }

            User uploader = await Client.User.GetUser(RelatedUserId.Value, null, cancelToken);
            RegisterRelation(uploader);

            return uploader;
        }

        /// <summary>
        /// Gets collection of <seealso cref="ScanlationGroup"/> translated the Chapter
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<ScanlationGroup>> GetGroups(CancellationToken cancelToken = default)
        {
            if(RelatedGroupIds.Count == 0)
            {
                return new List<ScanlationGroup>();
            }
            else if(TryGetRelationCollection(RelatedGroupIds, out List<ScanlationGroup> list))
            {
                return list;
            }

            var parameters = new GetGroupListParameters();
            parameters.GroupIds = RelatedGroupIds;
            parameters.Amount = RelatedGroupIds.Count;

            ResourceCollection<ScanlationGroup> groups = await Client.ScanlationGroup.GetList(parameters, cancelToken);

            foreach(ScanlationGroup group in groups)
            {
                RegisterRelation(group);
            }

            return groups;
        }

        /// <summary>
        /// Gets <see cref="Manga"/> which Chapter belongs to
        /// </summary>
        /// <returns>Manga</returns>
        public async Task<Manga> GetManga(CancellationToken cancelToken = default)
        {
            if(TryGetRelation(RelatedMangaId, out Manga? manga) && manga != null)
            {
                return manga;
            }

            manga = await Client.Manga.ViewManga(RelatedMangaId, null, cancelToken);
            RegisterRelation(manga);

            return manga;

        }

        /// <summary>
        /// Marks chapter as read for <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task MarkRead(CancellationToken cancelToken = default)
        {
            await Client.Chapter.MarkChapterRead(Id, cancelToken);
        }

        /// <summary>
        /// Marks chapter as unread for <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task MarkUnread(CancellationToken cancelToken = default)
        {
            await Client.Chapter.MarkChapterUnread(Id, cancelToken);
        }

        /// <summary>
        /// Opens reading session for the Chapter
        /// </summary>
        /// <param name="dataSaver">If compressed images should be used by default</param>
        /// <param name="forcePort443">Force selecting from MangaDex@Home servers that use the standard HTTPS port 443</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>See https://api.mangadex.org/docs.html#section/Reading-a-chapter-using-the-API/Retrieving-pages-from-the-MangaDex@Home-network for more information </remarks>
        public async Task<ChapterReadSession> StartReadingSession(bool dataSaver, bool forcePort443 = false, CancellationToken cancelToken = default)
        {
            ReadingSessionInformation readingSession = await Client.AtHome.GetServerUrlForChapter(Id, forcePort443, cancelToken);
            return new ChapterReadSession(
                readingSession.BaseUrl,
                this,
                readingSession.Chapter.Data,
                readingSession.Chapter.DataSaver,
                readingSession.Chapter.Hash,
                dataSaver,
                forcePort443);
        }
    }
}
