#nullable disable
using System;
using System.Collections.Generic;

using MangaDexSharp.Enums;

namespace MangaDexSharp.Internal.Dto.Responses.Manga
{
    internal class AllMangaReadingStatusesResponse : MangaDexResponse
    {
        public IDictionary<Guid, MangaReadingStatus> Statuses { get; set; }
    }
}
