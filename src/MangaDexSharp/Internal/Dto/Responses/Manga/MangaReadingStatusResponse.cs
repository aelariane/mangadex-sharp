#nullable disable
using MangaDexSharp.Enums;

namespace MangaDexSharp.Internal.Dto.Responses.Manga
{
    internal class MangaReadingStatusResponse : MangaDexResponse
    {
        public MangaReadingStatus? Status { get; set; }
    }
}
