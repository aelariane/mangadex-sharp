using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Enums
{
    /// <summary>
    /// Current status of <seealso cref="Manga"/>
    /// </summary>
    [MappableEnum("status")]
    public enum MangaStatus
    {
        None,
        /// <summary>
        /// Manga is still goin omn
        /// </summary>
        [EnumFieldStringValue("ongoing")] Ongoing,
        /// <summary>
        /// Manga is completed
        /// </summary>
        [EnumFieldStringValue("completed")] Completed,
        /// <summary>
        /// Manga is paused
        /// </summary>
        [EnumFieldStringValue("hiatus")] Hiatus,
        /// <summary>
        /// Manga has been cancelled
        /// </summary>
        [EnumFieldStringValue("cancelled")] Cancelled
    }
}
