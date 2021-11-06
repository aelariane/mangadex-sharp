using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Parameters.Enums
{
    [MappableEnum("order")]
    public enum OrderByType
    {
        [EnumFieldStringValue("asc")] Ascending,
        [EnumFieldStringValue("desc")] Descending
    }
}
