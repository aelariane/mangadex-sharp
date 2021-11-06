using System;
using System.Collections.Generic;

using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Parameters.Order.User;

namespace MangaDexSharp.Parameters.User
{
    public class GetUserListParameters : ListQueryParameters, ICanQueryByIdCollection
    {
        [QueryParameterName("ids")]
        public ICollection<Guid>? UserIds { get; set; }

        [QueryParameterName("username")]
        public string? Username { get; set; }

        ICollection<Guid> ICanQueryByIdCollection.Ids
        {
            get => UserIds ?? new HashSet<Guid>();
            set => UserIds = value;
        }

        public GetUserListParameters()
        {
            Order = new GetUserListOrderParameters();
        }

        public GetUserListParameters(GetUserListOrderParameters orderParameters)
        {
            Order = orderParameters;
        }
    }
}
