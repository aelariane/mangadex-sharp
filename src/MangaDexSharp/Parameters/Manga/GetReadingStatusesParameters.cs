using System;

using MangaDexSharp.Enums;
using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Parameters.Manga
{
    public sealed class GetReadingStatusesParameters : QueryParameters
    {
        [QueryParameterName("status")]
        public MangaReadingStatus Status { get; }

        public GetReadingStatusesParameters(MangaReadingStatus status)
        {
            Status = status;
        }
    }
}
