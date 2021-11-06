#nullable disable
using MangaDexSharp.Enums;
using MangaDexSharp.Objects;

namespace MangaDexSharp.Internal.Dto.ResourceAttributes
{
    internal class TagAttributes : BaseResourceAttributes
    {
        public LocalizedString Description { get; set; }
        public LocalizedString Name { get; set; }
        public TagGroup Group { get; set; }
    }
}
