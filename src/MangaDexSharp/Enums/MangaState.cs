using System;

using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Enums
{
    [MappableEnum("state")]
    public enum MangaState
    {
        [EnumFieldStringValue("draft")] Draft,
        [EnumFieldStringValue("submitted")] Submitted,
        [EnumFieldStringValue("published")] Published,
        [EnumFieldStringValue("rejected")] Rejected
    }
}
