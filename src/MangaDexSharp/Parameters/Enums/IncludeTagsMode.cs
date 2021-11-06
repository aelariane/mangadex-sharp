using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Parameters.Enums
{
    /// <summary>
    /// Mode for included tags
    /// </summary>
    [MappableEnum("includeTagsMode")]
    public enum IncludeTagsMode
    {
        [EnumFieldStringValue("AND")] And,
        [EnumFieldStringValue("OR")] Or
    }
}
