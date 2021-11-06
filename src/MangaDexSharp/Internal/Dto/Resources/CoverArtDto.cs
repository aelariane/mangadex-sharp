#nullable disable
using System.Collections.Generic;

using MangaDexSharp.Constants;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Internal.Dto.Resources
{
    [HasAttributesOtType(typeof(CoverArtAttributes))]
    [DtoFor(typeof(CoverArt))]
    internal class CoverArtDto : ResourceDto
    {
        public CoverArtAttributes Attributes => GetAttributes<CoverArtAttributes>();

        [Relationship(RelationshipNames.Manga)]
        public IEnumerable<MangaDto> MangaRelations { get; set; }

        [Relationship(RelationshipNames.User)]
        public IEnumerable<UserDto> UserRelations { get; set; }
    }
}
