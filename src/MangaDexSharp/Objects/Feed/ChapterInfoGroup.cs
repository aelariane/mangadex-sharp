using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Exceptions;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Objects.Feed
{
    /// <summary>
    /// Collection of  availible translated <seealso cref="Resources.Chapter"/> of <seealso cref="ChapterFeedElement"/> grouped by <seealso cref="Chapter.ChapterName"/>
    /// </summary>
    public class ChapterInfoGroup : IReadOnlyCollection<ChapterInfo>
    {
        /// <summary>
        /// Chapter name
        /// </summary>
        public string Chapter { get; }

        /// <summary>
        /// Translated chapters
        /// </summary>
        public IReadOnlyCollection<ChapterInfo> TranslatedChapters { get; }

        int IReadOnlyCollection<ChapterInfo>.Count => TranslatedChapters.Count;

        internal ChapterInfoGroup(
            string chapterName,
            IReadOnlyCollection<ChapterInfo> infoCollection)
        {
            Chapter = chapterName;
            TranslatedChapters = infoCollection;
        }

        /// <summary>
        /// Marks all <seealso cref="Resources.Chapter"/>s in the group as read for <seealso cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>Requires to be logged in</returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task MarkRead(CancellationToken cancelToken = default)
        {
            MangaDexClient client = TranslatedChapters.First().Chapter.Client;
            Guid mangaId = TranslatedChapters.First().Chapter.RelatedMangaId;

            List<Guid> idCollection = TranslatedChapters
                .Select(x => x.Chapter.Id)
                .ToList();

            await client.Manga.MarkChaptersOfManga(mangaId, idCollection, null, cancelToken);
        }

        /// <summary>
        /// Marks all <seealso cref="Resources.Chapter"/> in the group as unread for <seealso cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task MarkUnread(CancellationToken cancelToken = default)
        {
            MangaDexClient client = TranslatedChapters.First().Chapter.Client;
            Guid mangaId = TranslatedChapters.First().Chapter.RelatedMangaId;

            List<Guid> idCollection = TranslatedChapters
                .Select(x => x.Chapter.Id)
                .ToList();

            await client.Manga.MarkChaptersOfManga(mangaId, null, idCollection, cancelToken);
        }

        /// <inheritdoc/>
        public IEnumerator<ChapterInfo> GetEnumerator()
        {
            return TranslatedChapters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return TranslatedChapters.GetEnumerator();
        }
    }
}
