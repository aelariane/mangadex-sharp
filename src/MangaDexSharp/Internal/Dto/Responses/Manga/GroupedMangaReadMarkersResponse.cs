#nullable disable
using System;
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.Responses.Manga
{
    internal class GroupedMangaReadMarkersResponse : MangaDexResponse
    {
        public IDictionary<Guid, IReadOnlyCollection<Guid>> Data { get; set; }
    }
}
