using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MangaDexSharp.Internal.ResourceFactories
{
    internal class AuthorFactory : IResourceFactory
    {
        private MangaDexClient _client;

        public AuthorFactory(MangaDexClient client)
        {
            _client = client;
        }

        private void SetAuthorUris(Author author, AuthorAttributes attributes)
        {
            IEnumerable<PropertyInfo> authorProps = typeof(Author)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(Uri));

            IEnumerable<PropertyInfo> attributeProps = typeof(AuthorAttributes)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(Uri));

            foreach(PropertyInfo info in attributeProps)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Uri? value = info.GetMethod.Invoke(attributes, null) as Uri;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                if (value != null)
                {
                    PropertyInfo? authorProp = authorProps
                        .SingleOrDefault(x => x.Name.Equals(info.Name, StringComparison.OrdinalIgnoreCase));
                    if(authorProp != null)
                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        authorProp.SetMethod.Invoke(author, new object[] { value });
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    }
                }
            }
        }

        public MangaDexResource Create(ResourceDto dto)
        {
            var authorDto = dto as AuthorDto;
            if (authorDto == null)
            {
                throw new ArgumentException($"Invald dto type {dto.GetType()}. {nameof(AuthorDto)} expected");
            }

            AuthorAttributes attributes = authorDto.Attributes;

            var author = new Author(
                _client,
                dto.Id,
                attributes.Name,
                attributes.Biography,
                attributes.CreatedAt,
                attributes.UpdatedAt);
            author.Version = attributes.Version;

            //if (dto.Type == "author")
            //{
            //    author.IsMangaAuthor = true;
            //}
            if (dto.Type == "artist")
            {
                author.IsArtist = true;
            }

            SetAuthorUris(author, attributes);

            if(authorDto.MangaRelations != null)
            {
                foreach(var manga in authorDto.MangaRelations)
                {
                    author.RelatedMangaIds.Add(manga.Id);
                }
            }

            return author;
        }

        public void Sync(MangaDexResource resource, ResourceDto dto)
        {
            Author? author = resource as Author;
            if(author == null)
            {
                throw new ArgumentException($"Invald resource passed {resource.GetType()}. {nameof(Author)} expected");
            }
            author.IsArtist |= dto.Type == "artist";

            AuthorDto? authorDto = dto as AuthorDto;
            if (authorDto != null)
            {
                if (authorDto.MangaRelations != null)
                {
                    foreach (var manga in authorDto.MangaRelations)
                    {
                        if (author.RelatedMangaIds.Contains(manga.Id) == false)
                        {
                            author.RelatedMangaIds.Add(manga.Id);
                        }
                    }
                }
            }
        }
    }
}
