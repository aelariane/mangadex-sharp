using System;
using MangaDexSharp.Parameters.Enums;

namespace MangaDexSharp.Parameters.Order.Cover
{
    public class GetCoverListOrderParamateres : OrderParameters
    {
        public OrderByType? CreatedAt { get; set; }
        public OrderByType? UpdatedAt { get; set; }
        public OrderByType? Volume { get; set; }
    }
}
