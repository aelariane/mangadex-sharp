#nullable disable
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.Responses.Objects
{
    internal class MangaVolumeInfoDto
    {
        public string Volume { get; set; }
        public int Count { get; set; }
        public IReadOnlyDictionary<string, MangaChapterInfoDto> Chapters { get; set; }
    }
}
