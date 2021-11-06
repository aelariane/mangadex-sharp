using System;
using System.Collections;
using System.Collections.Generic;

using MangaDexSharp.Resources;

namespace MangaDexSharp.Objects.Feed
{
    /// <summary>
    /// Element of <see cref="MangaFeed"/>
    /// </summary>
    public class MangaFeedElement : IEnumerable<ChapterInfo>
    {
        /// <summary>
        /// Chapters in the element
        /// </summary>
        public IReadOnlyCollection<ChapterInfo> Chapters { get; }

        /// <summary>
        /// Cover for <seealso cref="Manga"/>
        /// </summary>
        public CoverArt? Cover { get; }

        /// <summary>
        /// Manga reference
        /// </summary>
        public Manga Manga { get; }

        internal MangaFeedElement(Manga manga, CoverArt? cover, IReadOnlyCollection<ChapterInfo> chapters)
        {
            Chapters = chapters;
            Cover = cover;
            Manga = manga;
        }

        /// <inheritdoc/>
        public IEnumerator<ChapterInfo> GetEnumerator()
        {
            return Chapters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Chapters.GetEnumerator();
        }
    }
}
