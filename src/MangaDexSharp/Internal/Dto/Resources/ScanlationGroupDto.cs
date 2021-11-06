#nullable disable
using System.Collections.Generic;

using MangaDexSharp.Constants;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Internal.Dto.Resources
{
    [HasAttributesOtType(typeof(ScanlationGroupAttributes))]
    [DtoFor(typeof(ScanlationGroup))]
    internal class ScanlationGroupDto : ResourceDto
    {
        public ScanlationGroupAttributes Attributes => GetAttributes<ScanlationGroupAttributes>();

        [Relationship(RelationshipNames.GroupLeader)]
        public IEnumerable<UserDto> LeaderRelations { get; set; }

        [Relationship(RelationshipNames.GroupMember)]
        public IEnumerable<UserDto> MemberRelations { get; set; }
    }
}
