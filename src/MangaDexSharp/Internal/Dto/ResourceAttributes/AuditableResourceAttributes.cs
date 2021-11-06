#nullable disable
using System;

namespace MangaDexSharp.Internal.Dto.ResourceAttributes
{
    internal abstract class AuditableResourceAttributes : BaseResourceAttributes
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
