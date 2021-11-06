using System;

using MangaDexSharp.Parameters.Enums;

namespace MangaDexSharp.Parameters.Order.User
{
    public sealed class GetUserListOrderParameters : OrderParameters
    {
        public OrderByType? Username { get; set; }
    }
}
