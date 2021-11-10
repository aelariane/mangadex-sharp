using System;
using System.Linq;

using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Internal.ResourceFactories
{
    internal class CoverArtFactory : IResourceFactory
    {
        private MangaDexClient _client;

        public CoverArtFactory(MangaDexClient client)
        {
            _client = client;
        }

        public MangaDexResource Create(ResourceDto dto)
        {
            var coverDto = dto as CoverArtDto;
            if (coverDto == null)
            {
                throw new ArgumentException($"Invald dto type {dto.GetType()}. {nameof(CoverArtDto)} expected");
            }

            CoverArtAttributes attributes = coverDto.Attributes;

            var cover = new CoverArt(
                _client,
                coverDto.Id,
                attributes.FileName,
                attributes.CreatedAt,
                attributes.UpdatedAt);
            cover.Version = attributes.Version;

            if(attributes.Description != null)
            {
                cover.Description = attributes.Description;
            }
            if(attributes.Volume != null)
            {
                cover.Volume = attributes.Volume;
            }

            if(coverDto.MangaRelations != null)
            {
                cover.MangaId = coverDto.MangaRelations.First().Id;
            }
            if (coverDto.UserRelations != null)
            {
                cover.UploaderId = coverDto.UserRelations.First().Id;
            }


            return cover;
        }

        public void Sync(MangaDexResource resource, ResourceDto dto)
        {
            CoverArt cover = (CoverArt)resource;
            if(dto is CoverArtDto coverDto)
            {
                if (cover.MangaId == Guid.Empty)
                {
                    if (coverDto.MangaRelations != null)
                    {
                        cover.MangaId = coverDto.MangaRelations.First().Id;
                    }
                }
                if (cover.UploaderId == Guid.Empty)
                {
                    if (coverDto.UserRelations != null)
                    {
                        cover.UploaderId = coverDto.UserRelations.First().Id;
                    }
                }
            }
        }
    }
}
