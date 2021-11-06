using System;
using System.Linq;

using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Internal.ResourceFactories
{
    internal class CustomListFactory : IResourceFactory
    {
        private MangaDexClient _client;

        public CustomListFactory(MangaDexClient client)
        {
            _client = client;
        }

        public MangaDexResource Create(ResourceDto dto)
        {
            CustomListDto? listDto = dto as CustomListDto;
            if (listDto == null)
            {
                throw new ArgumentException($"Invald dto type {dto.GetType()}. {nameof(CustomListDto)} expected");
            }
            CustomListAttributes attributes = listDto.Attributes;

            var list = new CustomList(
                _client,
                listDto.Id,
                attributes.Name,
                attributes.Visibility,
                attributes.Version);

            list.RelatedUserId = listDto.UserRelations.First().Id;

            if(listDto.MangaRelations != null)
            {
                foreach(MangaDto manga in listDto.MangaRelations)
                {
                    list.RelatedMangaIds.Add(manga.Id);
                }
            }

            return list;
        }

        public void Sync(MangaDexResource resource, ResourceDto dto)
        {
        }
    }
}
