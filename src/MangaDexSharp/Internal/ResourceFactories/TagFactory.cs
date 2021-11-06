using System;

using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Internal.ResourceFactories
{
    internal class TagFactory : IResourceFactory
    {
        private MangaDexClient _client;

        public TagFactory(MangaDexClient client)
        {
            _client = client;
        }

        public MangaDexResource Create(ResourceDto dto)
        {
            var tagDto = dto as TagDto;
            if (tagDto == null)
            {
                throw new ArgumentException($"Invald dto type {dto.GetType()}. {nameof(TagDto)} expected");
            }

            TagAttributes attributes = tagDto.Attributes;
            var tag = new Tag(
                _client,
                tagDto.Id,
                tagDto.Attributes.Group,
                tagDto.Attributes.Name,
                tagDto.Attributes.Description);

            tag.Version = tagDto.Attributes.Version;

            return tag;
        }

        public void Sync(MangaDexResource resource, ResourceDto dto)
        {
        }
    }
}
