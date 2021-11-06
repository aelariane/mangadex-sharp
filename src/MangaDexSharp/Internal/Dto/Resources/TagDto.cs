#nullable disable
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Internal.Dto.Resources
{
    [HasAttributesOtType(typeof(TagAttributes))]
    [DtoFor(typeof(Tag))]
    internal class TagDto : ResourceDto
    {
        public TagAttributes Attributes => GetAttributes<TagAttributes>();
    }
}
