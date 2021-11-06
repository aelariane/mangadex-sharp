using MangaDexSharp.Internal.Dto.ResourceAttributes;

namespace MangaDexSharp.Internal.Dto.Resources
{
    internal interface IResourceDtoWithAttributes
    {
        BaseResourceAttributes Attributes { get; set; }
    }
}
