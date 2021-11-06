using System;

namespace MangaDexSharp.Api.Data
{
    /// <summary>
    /// Provides information abour <seealso cref="Resources.Chapter"/> of <seealso cref="Resources.Manga"/> volume
    /// </summary>
    public class MangaChapterInfo
    {
        /// <summary>
        /// Chapter in string format 
        /// </summary>
        public string Chapter { get; }

        /// <summary>
        /// Id of last updated/uploaded <seealso cref="Resources.Chapter"/> with <seealso cref="Resources.Chapter.ChapterName"/>(Not sure about this)
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Amount of <seealso cref="Resources.Chapter"/> with <seealso cref="Resources.Chapter.ChapterName"/> in different translations
        /// </summary>
        public int Count { get; }

        internal MangaChapterInfo(Guid guid, string chapter, int count)
        {
            Id = guid;
            Chapter = chapter;
            Count = count;
        }
    }
}
