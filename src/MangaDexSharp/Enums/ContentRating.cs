using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Enums
{
    [MappableEnum("contentRating")]
    public enum ContentRating
    {
        [EnumFieldStringValue("safe")] Safe,
        [EnumFieldStringValue("suggestive")] Suggestive,
        [EnumFieldStringValue("erotica")] Erotica,
        [EnumFieldStringValue("pornographic")] Pornographic
    }
}
