using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Enums
{
    [MappableEnum("group")]
    public enum TagGroup
    {
        /// <summary>
        /// Tag represents Genre
        /// </summary>
        [EnumFieldStringValue("genre")] Genre,
        /// <summary>
        /// Tag represents some theme
        /// </summary>
        [EnumFieldStringValue("theme")] Theme,
        /// <summary>
        /// Tag represents some format
        /// </summary>
        [EnumFieldStringValue("format")] Format,
        /// <summary>
        /// TODO
        /// </summary>
        [EnumFieldStringValue("content")] Content
    }
}
