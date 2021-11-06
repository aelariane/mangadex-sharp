#nullable disable
using System;

namespace MangaDexSharp.Internal.Dto.ResourceAttributes
{
    internal class CoverArtAttributes : AuditableResourceAttributes
    {
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Volume { get; set; }
    }
}
