#nullable disable
using System.Collections.Generic;

using MangaDexSharp.Enums;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Objects;

namespace MangaDexSharp.Internal.Dto.ResourceAttributes
{
    internal class MangaAttributes : AuditableResourceAttributes
    {
        public LocalizedString Description { get; set; }
        public ContentRating ContentRating { get; set; }
        public MangaPublicationDemographic? PublicationDemographic { get; set; }
        public LocalizedString Links { get; set; }
        public LocalizedString Title { get; set; }
        public IEnumerable<LocalizedString> AltTitles { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public bool IsLocked { get; set; }
        public string OriginalLanguage { get; set; }
        public string LastVolume { get; set; }
        public string LastChapter { get; set; }
        public MangaState State { get; set; }
        public MangaStatus? Status { get; set; }
        public int? Year { get; set; }
    }
}
