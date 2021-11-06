#nullable disable
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using MangaDexSharp.Internal.Dto.ResourceAttributes;

namespace MangaDexSharp.Internal.Dto.Resources
{
    internal class ResourceDto : IResourceDtoWithAttributes
    {
        public IList<ResourceDto> AllRelations { get; } = new List<ResourceDto>();

        [JsonIgnore]
        BaseResourceAttributes IResourceDtoWithAttributes.Attributes { get; set; }

        public Guid Id { get; set; }
        public string Type { get; set; }

        protected TAttributes GetAttributes<TAttributes>() where TAttributes : BaseResourceAttributes
        {
            return (this as IResourceDtoWithAttributes).Attributes as TAttributes;
        }
    }
}
