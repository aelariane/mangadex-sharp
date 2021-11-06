#nullable disable
using System.Collections.Generic;

using MangaDexSharp.Constants;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Internal.Dto.ResourceAttributes;

namespace MangaDexSharp.Internal.Dto.Resources
{
    [HasAttributesOtType(typeof(MangaRelationAttributes))]
    internal class MangaRelationDto : ResourceDto
    {
        public MangaRelationAttributes Attributes => GetAttributes<MangaRelationAttributes>();

        [Relationship(RelationshipNames.Manga)]
        public IEnumerable<MangaDto> MangaRelations { get; set; }
    }
}
