#nullable disable
using System;

using MangaDexSharp.Enums;

namespace MangaDexSharp.Internal.Dto.Requests.Manga
{
    internal class UpdateMangaReadingStatusRequest
    {
        public MangaReadingStatus? Status { get; set; }

        public UpdateMangaReadingStatusRequest(MangaReadingStatus? status)
        {
            Status = status;
        }
    }
}
