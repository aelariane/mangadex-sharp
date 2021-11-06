#nullable disable
using System.Collections.Generic;

using MangaDexSharp.Constants;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Internal.Dto.Resources
{
    [HasAttributesOtType(typeof(ChapterAttributes))]
    [DtoFor(typeof(Chapter))]
    internal class ChapterDto : ResourceDto
    {
        public ChapterAttributes Attributes => GetAttributes<ChapterAttributes>();

        [Relationship(RelationshipNames.Manga)]
        public IEnumerable<MangaDto> MangaRelations { get; set; }

        [Relationship(RelationshipNames.ScanlationGroup)]
        public IEnumerable<ScanlationGroupDto> ScanlationGroupRelations { get; set; }

        [Relationship(RelationshipNames.User)]
        public IEnumerable<UserDto> UserRelations { get; set; }

    }
}
