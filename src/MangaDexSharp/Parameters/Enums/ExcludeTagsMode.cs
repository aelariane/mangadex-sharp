using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Parameters.Enums
{
    /// <summary>
    /// Mode for exlcluded tags
    /// </summary>
    [MappableEnum("excludeTagsMode")]
    public enum ExcludeTagsMode
    {
        [EnumFieldStringValue("AND")] And,
        [EnumFieldStringValue("OR")] Or
    }
}
