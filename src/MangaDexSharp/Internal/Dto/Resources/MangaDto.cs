#nullable disable
using System.Collections.Generic;

using MangaDexSharp.Constants;
using MangaDexSharp.Enums;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Internal.Dto.Resources
{
    [HasAttributesOtType(typeof(MangaAttributes))]
    [DtoFor(typeof(Manga))]
    internal class MangaDto : ResourceDto
    {
        [Relationship(RelationshipNames.Artist)]
        public IEnumerable<AuthorDto> ArtistRelations { get; set; }

        [Relationship(RelationshipNames.Author)]
        public IEnumerable<AuthorDto> AuthorRelations { get; set; }

        public MangaAttributes Attributes => GetAttributes<MangaAttributes>();

        [Relationship(RelationshipNames.CovertArt)]
        public IEnumerable<CoverArtDto> CoverRelations { get; set; }

        [Relationship(RelationshipNames.Manga)]
        public IEnumerable<MangaDto> MangaRelations { get; set; }

        [Relationship(RelationshipNames.User)]
        public IEnumerable<UserDto> UserRelations { get; set; }

        [Relationship(RelationshipNames.Creator)]
        public IEnumerable<CreatorDto> Creator { get; set; }
        public MangaRelation Related { get; set; }

    }
}