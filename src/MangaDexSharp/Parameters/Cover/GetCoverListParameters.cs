using System;
using System.Collections.Generic;

using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Parameters.Order.Cover;

namespace MangaDexSharp.Parameters.Cover
{
    public sealed class GetCoverListParameters : ListQueryParameters, ICanQueryByIdCollection
    {
        [QueryParameterName("ids")]
        public ICollection<Guid>? CoverIds { get; set; }

        [QueryParameterName("manga")]
        public ICollection<Guid>? Mangas { get; set; }

        [QueryParameterName("uploaders")]
        public ICollection<Guid>? Uploaders { get; set; }

        ICollection<Guid> ICanQueryByIdCollection.Ids 
        {
            get => CoverIds ??= new HashSet<Guid>();
            set => CoverIds = value;
        }

        public GetCoverListParameters() : base()
        {
            Order = new GetCoverListOrderParamateres();
        }

        public GetCoverListParameters(GetCoverListOrderParamateres order) : base()
        {
            Order = order;
        }
    }
}
