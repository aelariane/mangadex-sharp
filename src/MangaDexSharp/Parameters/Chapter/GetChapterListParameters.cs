using System;
using System.Collections.Generic;

using MangaDexSharp.Enums;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Parameters.Order.Chapter;

namespace MangaDexSharp.Parameters.Chapter
{
    public sealed class GetChapterListParameters : ListQueryParameters, ICanQueryByIdCollection
    {
        [QueryParameterName("ids")]
        public ICollection<Guid>? ChapterIds { get; set; }

        [QueryParameterName("chapter")]
        public ICollection<string>? Chapters { get; set; }

        [QueryParameterName("contentRating")]
        public ICollection<ContentRating>? ContentRatings { get; set; }

        [QueryParameterName("createdAtSince")]
        public DateTime? CreatedAtSince { get; set; }

        [QueryParameterName("excludedOriginalLanguage")]
        public ICollection<string>? ExcludedOriginalLanguages { get; set; }

        [QueryParameterName("groups")]
        public ICollection<Guid>? Groups { get; set; }

        [QueryParameterName("includeFutureUpdates")]
        public bool IncludeFutureUpdates { get; set; } = true;

        [QueryParameterName("manga")]
        public Guid? Manga { get; set; }

        [QueryParameterName("originalLanguage")]
        public ICollection<string>? OriginalLanguages { get; set; }

        [QueryParameterName("publishAtSince")]
        public DateTime? PublishAtSince { get; set; }

        [QueryParameterName("title")]
        public string? Title { get; set; }

        [QueryParameterName("translatedLanguage")]
        public ICollection<string>? TranslatedLanguages { get; set; }

        [QueryParameterName("uploader")]
        public Guid? Uploader { get; set; }

        [QueryParameterName("updatedAtSince")]
        public DateTime? UpdatedAtSince { get; set; }

        [QueryParameterName("volume")]
        public ICollection<string>? Volumes { get; set; }

        ICollection<Guid> ICanQueryByIdCollection.Ids
        {
            get => ChapterIds ??= new HashSet<Guid>();
            set => ChapterIds = value;
        }

        public GetChapterListParameters() : base()
        {
            Order = new GetChapterListOrderParameters();
        }

        public GetChapterListParameters(GetChapterListOrderParameters parameters) : base()
        {
            Order = parameters;
        }
    }
}
