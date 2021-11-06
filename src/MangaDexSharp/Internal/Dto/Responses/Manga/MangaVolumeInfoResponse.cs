#nullable disable
using MangaDexSharp.Internal.Dto.Responses.Objects;
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.Responses.Manga
{
    internal class MangaVolumeInfoResponse : MangaDexResponse
    {
        public IDictionary<string, MangaVolumeInfoDto> Volumes { get; set; }
    }
}
