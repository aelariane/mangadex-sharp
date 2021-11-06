using System.Collections.Generic;

namespace MangaDexSharp.Api.Data
{
    /// <summary>
    /// Provides information about volume of <seealso cref="Resources.Manga"/>
    /// </summary>
    public class MangaVolumeInfo
    {
        /// <summary>
        /// Volume in string format
        /// </summary>
        public string Volume { get; }

        /// <summary>
        /// Amount of <seealso cref="Resources.Chapter"/> with on requested languages (All availible by default)
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Collection of <seealso cref="Resources.Chapter"/> metadata for the volume
        /// </summary>
        public IReadOnlyDictionary<string, MangaChapterInfo> Chapters { get; }

        internal MangaVolumeInfo(
            string volume,
            IEnumerable<KeyValuePair<string, MangaChapterInfo>> chapters,
            int count)
        {
            Volume = volume;
            Chapters = new Dictionary<string, MangaChapterInfo>(chapters);
            Count = count;
        }
    }
}
