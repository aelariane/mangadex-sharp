#nullable disable
using System;
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.Responses.Manga
{
    internal class MangaReadMarkersResponse : MangaDexResponse
    {
        public IReadOnlyCollection<Guid> Data { get; set; }
    }
}
