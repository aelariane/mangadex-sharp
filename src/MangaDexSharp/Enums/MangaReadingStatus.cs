using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Enums
{
    /// <summary>
    /// Reading status of <seealso cref="Manga"/> tracked for <see cref="MangaDexClient.CurrentUser"/>
    /// </summary>
    [MappableEnum("readingStatus")]
    public enum MangaReadingStatus
    {
        /// <summary>
        /// Reading is in process
        /// </summary>
        [EnumFieldStringValue("reading")] Reading,
        /// <summary>
        /// Stopped reading for a while
        /// </summary>
        [EnumFieldStringValue("on_hold")] OnHold,
        /// <summary>
        /// Planned to read
        /// </summary>
        [EnumFieldStringValue("plan_to_read")] PlanToRead,
        /// <summary>
        /// Dropped reading the Manga
        /// </summary>
        [EnumFieldStringValue("dropped")] Dropped,
        /// <summary>
        /// In process of re-reading Manga
        /// </summary>
        [EnumFieldStringValue("re_reading")] ReReading,
        /// <summary>
        /// Completed reading Manga
        /// </summary>
        [EnumFieldStringValue("completed")] Completed
    }
}
