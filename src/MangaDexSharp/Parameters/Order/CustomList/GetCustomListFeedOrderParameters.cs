using System;

using MangaDexSharp.Parameters.Enums;

namespace MangaDexSharp.Parameters.Order.CustomList
{
    public class GetCustomListFeedOrderParameters : OrderParameters
    {
        public OrderByType? Volume { get; set; }
        public OrderByType? Chapter { get; set; }
        public OrderByType? CreatedAt { get; set; }
    }
}
