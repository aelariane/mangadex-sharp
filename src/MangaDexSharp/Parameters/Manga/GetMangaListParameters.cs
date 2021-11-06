using System;
using System.Collections.Generic;

using MangaDexSharp.Enums;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Parameters.Enums;
using MangaDexSharp.Parameters.Order.Manga;

namespace MangaDexSharp.Parameters.Manga
{
    public sealed class GetMangaListParameters : ListQueryParameters, ICanQueryByIdCollection
    {
        [QueryParameterName("title")]
        public string? Title { get; set; }

        [QueryParameterName("authors")]
        public ICollection<Guid>? Authors { get; set; }

        [QueryParameterName("artists")]
        public ICollection<Guid>? Artists { get; set; }

        [QueryParameterName("year")]
        public int? Year { get; set; }

        [QueryParameterName("includedTags")]
        public ICollection<Guid>? IncludeTags { get; set; }

        [QueryParameterName("includedTagsMode")]
        public IncludeTagsMode? IncludeTagsMode { get; set; }

        [QueryParameterName("excludedTags")]
        public ICollection<Guid>? ExcludeTags { get; set; }

        [QueryParameterName("exludedTagsMode")]
        public ExcludeTagsMode? ExcludeTagsMode { get; set; }

        [QueryParameterName("status")]
        public ICollection<MangaStatus>? Status { get; set; }

        [QueryParameterName("originalLanguage")]
        public ICollection<string>? OriginalLanguages { get; set; }

        [QueryParameterName("excludedOriginalLanguage")]
        public ICollection<string>? ExcludedOriginalLanguages { get; set; }

        [QueryParameterName("availableTranslatedLanguage")]
        public ICollection<string>? AvalibileTranslatedLanguages { get; set; }

        [QueryParameterName("publicationDemographic")]
        public ICollection<MangaPublicationDemographic>? PublicationDemographics { get; set; }

        [QueryParameterName("contentRating")]
        public ICollection<ContentRating>? ContentRatings { get; set; }

        [QueryParameterName("ids")]
        public ICollection<Guid>? MangaIds { get; set; }

        [QueryParameterName("createdAtSince")]
        public DateTime? CreatedAtSince { get; set; }

        [QueryParameterName("updatedAtSince")]
        public DateTime? UpdatedAtSince { get; set; }

        ICollection<Guid> ICanQueryByIdCollection.Ids
        {
            get => MangaIds ??= new HashSet<Guid>();
            set => MangaIds = value;
        }

        public GetMangaListParameters() : base()
        {
            Order =  GetMangaListOrderParameters.Default;
        }

        public GetMangaListParameters(GetMangaListOrderParameters orderOptions) : base()
        {
            Order = orderOptions;
        }
    }
}
