using MangaDexSharp.Internal.Dto.Resources;

namespace MangaDexSharp.Internal
{
    internal interface IResourceFactory
    {
        MangaDexResource Create(ResourceDto dto);
        void Sync(MangaDexResource resource, ResourceDto dto);
    }
}
