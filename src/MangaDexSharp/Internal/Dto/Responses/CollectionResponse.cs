#nullable disable
using System.Collections;
using System.Collections.Generic;

using MangaDexSharp.Internal.Dto.Resources;

namespace MangaDexSharp.Internal
{
    internal class CollectionResponse<TObject> : MangaDexResponse
        where TObject : ResourceDto
    {
        public IReadOnlyCollection<TObject> Data { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int Total { get; set; }
    }
}
