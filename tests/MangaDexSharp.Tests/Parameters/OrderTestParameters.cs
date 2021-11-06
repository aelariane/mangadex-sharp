using MangaDexSharp.Parameters.Enums;
using MangaDexSharp.Parameters.Order;
using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Tests.Parameters
{
    public class OrderTestParameters : OrderParameters
    {
        public OrderByType? FirstParameter { get; set; }

        public OrderByType? SecondParameter { get; set; }
    }
}