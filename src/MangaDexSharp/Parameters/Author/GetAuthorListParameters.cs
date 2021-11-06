using System;
using System.Collections.Generic;

using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Parameters.Order.Author;

namespace MangaDexSharp.Parameters.Author
{
    public sealed class GetAuthorListParameters : ListQueryParameters, ICanQueryByIdCollection
    {

        [QueryParameterName("name")]
        public string? AuthorName { get; set; }

        [QueryParameterName("ids")]
        public ICollection<Guid>? AuthorIds { get; set; }

        ICollection<Guid> ICanQueryByIdCollection.Ids
        {
            get => AuthorIds ??= new HashSet<Guid>();
            set => AuthorIds = value;
        }

        public GetAuthorListParameters() : base()
        {
            Order = new GetAuthorListOrderParameters();
        }

        public GetAuthorListParameters(GetAuthorListOrderParameters order) : base()
        {
            Order = order;
        }
    }
}
