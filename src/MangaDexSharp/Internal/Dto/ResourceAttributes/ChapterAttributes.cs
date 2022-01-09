#nullable disable
using System;
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.ResourceAttributes
{
    internal class ChapterAttributes : AuditableResourceAttributes
    {
        public string Chapter { get; set; }
        //public IEnumerable<Uri> Data { get; set; }
        //public IEnumerable<Uri> DataSaver { get; set; }
        public Uri ExternalUrl { get; set; }
        //public string Hash { get; set; }
        public DateTime PublishAt { get; set; }
        public string Title { get; set; }
        public string TranslatedLanguage { get; set; }
        public string Volume { get; set; }
    }
}
