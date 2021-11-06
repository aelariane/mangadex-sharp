using MangaDexSharp.Parameters.Enums;

namespace MangaDexSharp.Parameters.Order.Author
{
    public class GetAuthorListOrderParameters : OrderParameters
    {
        public OrderByType? Name { get; }
    }
}
