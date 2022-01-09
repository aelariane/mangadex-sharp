#nullable disable
using System;

namespace MangaDexSharp.Internal.Dto.Responses.AtHome
{
    internal class AtHomeServerResponse : MangaDexResponse
    {
        public string BaseUrl { get; set; }

        public ChapterAtHomeData Chapter { get; set; }
    }
}
