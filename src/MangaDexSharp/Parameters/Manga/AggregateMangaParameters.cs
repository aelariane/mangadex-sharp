using System;
using System.Collections.Generic;

using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Parameters.Manga
{
    public sealed class AggregateMangaParameters : QueryParameters
    {
        [QueryParameterName("groups")]
        public ICollection<Guid>? Groups { get; set; }

        [QueryParameterName("translatedLanguage")]
        public ICollection<string>? TranslatedLanguages { get; set; }

        public AggregateMangaParameters() : base()
        {
        }
    }
}
