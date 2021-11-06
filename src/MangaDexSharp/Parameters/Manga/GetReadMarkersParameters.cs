using System;
using System.Collections.Generic;

using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Parameters.Manga
{
    public sealed class GetReadMarkersParameters : QueryParameters
    {
        [QueryParameterName("grouped")]
        public bool Grouped { get; set; } = true;

        [QueryParameterName("ids")]
        public ICollection<Guid> MangaIds { get; set; }

        public GetReadMarkersParameters(ICollection<Guid> ids)
        {
            MangaIds = ids;
        }
    }
}
