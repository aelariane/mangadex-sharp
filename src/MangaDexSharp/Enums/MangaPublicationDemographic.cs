using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Enums
{
    /// <summary>
    /// Target audience of <seealso cref="Manga"/>
    /// </summary>
    [MappableEnum("publicationDemographic")]
    public enum MangaPublicationDemographic
    {
        /// <summary>
        /// Manga is shounen
        /// </summary>
        /// <remarks>Male audience in age 12-18</remarks>
        [EnumFieldStringValue("shounen")] Shounen = 1,
        /// <summary>
        /// Manga is shoujo
        /// </summary>
        /// <remarks>Female audience in age 12-18</remarks>
        [EnumFieldStringValue("shoujo")] Shoujo,
        /// <summary>
        /// Manga is josei
        /// </summary>
        /// <remarks>Female audience in age 18+</remarks>
        [EnumFieldStringValue("josei")] Josei,
        /// <summary>
        /// Manga is seinen
        /// </summary>
        /// <remarks>Male audience in age 18+</remarks>
        [EnumFieldStringValue("seinen")] Seinen,
        None = 0
    }
}
