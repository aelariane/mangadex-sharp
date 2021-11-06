using System.Collections;
using System.Collections.Generic;
using System.Linq;

using MangaDexSharp.Resources;

namespace MangaDexSharp.Objects.Feed
{
    /// <summary>
    ///     Element of <seealso cref="ChapterFeed"/>, which contains collections of     <seealso cref="ChapterInfo"/> grouped by <seealso cref="Chapter.Volume"/>
    ///     and each element of the group is grouped by <seealso cref="Chapter.ChapterName"/>
    /// </summary>
    public class ChapterFeedElement : IReadOnlyCollection<ChapterInfoGroup>
    {
        /// <summary>
        /// Availible <seealso cref="Chapter.ChapterName"/> in the Volume
        /// </summary>
        public IReadOnlyCollection<string> AvailibleChapters { get; }

        /// <summary>
        /// Collection of <seealso cref="ChapterInfo"/> grouped by <seealso cref="Chapter.ChapterName"/>
        /// </summary>
        public IReadOnlyCollection<ChapterInfoGroup> Chapters { get; }

        /// <inheritdoc/>
        public int Count => Chapters.Count;

        /// <summary>
        /// Volume of the group
        /// </summary>
        public string Volume { get; }


        internal ChapterFeedElement(
            string? volume,
            IEnumerable<KeyValuePair<string?, IEnumerable<ChapterInfo>>> groupedChapters)
        {
            Volume = volume ?? "No Volume";

            AvailibleChapters = groupedChapters
                .Select(x => x.Key ?? string.Empty)
                .ToList();

            Chapters = groupedChapters
                .Select(x => new ChapterInfoGroup(x.Key ?? string.Empty, x.Value.ToList()))
                .ToList();
        }

        /// <inheritdoc/>
        public IEnumerator<ChapterInfoGroup> GetEnumerator()
        {
            return Chapters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Chapters.GetEnumerator();
        }
    }
}
