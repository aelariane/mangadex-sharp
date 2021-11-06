using System;

using MangaDexSharp.Parameters.Enums;

namespace MangaDexSharp.Parameters.Order.Manga
{
    public class GetMangaListOrderParameters : OrderParameters
    {
        public OrderByType? Title { get; set; }
        public OrderByType? Year { get; set; }
        public OrderByType? CreatedAt { get; set; }
        public OrderByType? UpdatedAt { get; set; }
        public OrderByType? LatestUploadedChapter { get; set; }
        public OrderByType? FollowedCount { get; set; }
        public OrderByType? Relevance { get; set; }

        public static GetMangaListOrderParameters Default { get; } = new GetMangaListOrderParameters()
        {
            LatestUploadedChapter = OrderByType.Descending
        };
    }
}
