using System;
using System.Collections.Generic;
using System.Linq;

using MangaDexSharp.Enums;
using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Internal.ResourceFactories
{
    internal class MangaFactory : IResourceFactory
    {
        private MangaDexClient _client;
        private ResourcePool _pool;

        public MangaFactory(MangaDexClient client, ResourcePool pool)
        {
            _client = client;
            _pool = pool;
        }

        public MangaDexResource Create(ResourceDto dto)
        {
            var mangaDto = dto as MangaDto;
            if (mangaDto == null)
            {
                throw new ArgumentException($"Invald dto type {dto.GetType()}. {nameof(MangaDto)} expected");
            }

            MangaAttributes attributes = mangaDto.Attributes;
            IEnumerable<Tag> tags = attributes.Tags
                .Where(x => _pool.TryRetrieve<Tag>(x, out _))
                .Select(x =>
                {
                    _pool.TryRetrieve(x, out Tag? resultTag);
                    if (resultTag == null)
                    {
                        throw new Exception("Cannot Retreive tag from dto: " + x.Attributes.Name.First);
                    }
                    return resultTag;
                });

            var manga = new Manga(
                _client,
                mangaDto.Id,
                attributes.Title,
                attributes.AltTitles,
                attributes.Description,
                attributes.IsLocked,
                attributes.Links,
                attributes.OriginalLanguage,
                attributes.ContentRating,
                tags,
                attributes.CreatedAt,
                attributes.UpdatedAt);
            manga.Version = attributes.Version;

            if (attributes.Status != null)
            {
                manga.Status = attributes.Status;
            }
            if(attributes.LastChapter != null)
            {
                manga.LastChapter = attributes.LastChapter;
            }
            if(attributes.LastVolume != null)
            {
                manga.LastVolume = attributes.LastVolume;
            }
            if(attributes.PublicationDemographic != null)
            {
                manga.PublicationDemographic = attributes.PublicationDemographic;
            }
            if(attributes.Year != null)
            {
                manga.Year = attributes.Year;
            }

            if(mangaDto.AuthorRelations != null)
            {
                foreach (AuthorDto author in mangaDto.AuthorRelations)
                {
                    if (manga.RelatedAuthorIds.Contains(author.Id) == false)
                    {
                        manga.RelatedAuthorIds.Add(author.Id);
                    }
                }
            }

            if (mangaDto.ArtistRelations != null)
            {
                foreach (AuthorDto artist in mangaDto.ArtistRelations)
                {
                    if (manga.RelatedArtistIds.Contains(artist.Id) == false)
                    {
                        manga.RelatedArtistIds.Add(artist.Id);
                    }
                }
            }

            if (mangaDto.CoverRelations != null && mangaDto.CoverRelations.Any())
            {
                manga.MainCoverId = mangaDto.CoverRelations.First().Id;
            }

            return manga;
        }

        public void Sync(MangaDexResource resource, ResourceDto dto)
        {
            Manga manga = (Manga)resource;
            if(dto is MangaDto mangaDto)
            {
                if (mangaDto.AuthorRelations != null)
                {
                    foreach (AuthorDto author in mangaDto.AuthorRelations)
                    {
                        if (manga.RelatedAuthorIds.Contains(author.Id) == false)
                        {
                            manga.RelatedAuthorIds.Add(author.Id);
                        }
                    }
                }

                if (mangaDto.ArtistRelations != null)
                {
                    foreach (AuthorDto artist in mangaDto.ArtistRelations)
                    {
                        if (manga.RelatedArtistIds.Contains(artist.Id) == false)
                        {
                            manga.RelatedArtistIds.Add(artist.Id);
                        }
                    }
                }
                if (manga.MainCoverId == Guid.Empty)
                {
                    if (mangaDto.CoverRelations != null && mangaDto.CoverRelations.Any())
                    {
                        manga.MainCoverId = mangaDto.CoverRelations.First().Id;
                    }
                }
                }
        }
    }
}
