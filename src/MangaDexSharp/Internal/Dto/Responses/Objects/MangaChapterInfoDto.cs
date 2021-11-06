#nullable disable
using System;

namespace MangaDexSharp.Internal.Dto.Responses.Objects
{
    internal class MangaChapterInfoDto
    {
        public string Chapter { get; set; }
        public Guid Id { get; set; }
        public int Count { get; set; }
    }
}
