using System;

using MangaDexSharp.Parameters.Enums;

namespace MangaDexSharp.Parameters.Order.Chapter
{
    public class GetChapterListOrderParameters : OrderParameters
    {
        public OrderByType? Chapter { get; set; }
        public OrderByType? CreatedAt { get; set; }
        public OrderByType? PublishAt { get; set; }
        public OrderByType? UpdatedAt { get; set; }
        public OrderByType? Volume { get; set; }
    }
}
