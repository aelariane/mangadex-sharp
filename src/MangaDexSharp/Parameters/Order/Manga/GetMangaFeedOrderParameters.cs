using System;

using MangaDexSharp.Parameters.Enums;

namespace MangaDexSharp.Parameters.Order.Manga
{
    public class GetMangaFeedOrderParameters : OrderParameters
    {
        public OrderByType? Volume { get; set; }
        public OrderByType? Chapter { get; set; }
    }
}
