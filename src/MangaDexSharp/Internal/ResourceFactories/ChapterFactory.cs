using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDexSharp.Internal.ResourceFactories
{
    internal class ChapterFactory : IResourceFactory
    {
        private MangaDexClient _client;

        public ChapterFactory(MangaDexClient client)
        {
            _client = client;
        }

        public MangaDexResource Create(ResourceDto dto)
        {
            var chapterDto = dto as ChapterDto;
            if (chapterDto == null)
            {
                throw new ArgumentException($"Invald dto type {dto.GetType()}. {nameof(ChapterDto)} expected");
            }

            ChapterAttributes attributes = chapterDto.Attributes;
            var chapter = new Chapter(
                _client,
                chapterDto.Id,
                attributes.Title,
                attributes.TranslatedLanguage,
                attributes.Pages,
                attributes.CreatedAt,
                attributes.UpdatedAt,
                attributes.PublishAt);
            chapter.Version = attributes.Version;

            if(attributes.Volume != null)
            {
                chapter.Volume = attributes.Volume;
            }
            if(attributes.Chapter != null)
            {
                chapter.ChapterName = attributes.Chapter;
            }
            if(attributes.ExternalUrl != null)
            {
                chapter.ExternalUrl = attributes.ExternalUrl;
            }

            if(chapterDto.MangaRelations != null)
            {
                chapter.RelatedMangaId = chapterDto.MangaRelations.First().Id;
            }
            if(chapterDto.ScanlationGroupRelations != null)
            {
                foreach(ScanlationGroupDto group in chapterDto.ScanlationGroupRelations)
                {
                    chapter.RelatedGroupIds.Add(group.Id);
                }
            }
            if (chapterDto.UserRelations != null)
            {
                chapter.RelatedUserId = chapterDto.UserRelations.First().Id;
            }

            return chapter;
        }

        public void Sync(MangaDexResource resource, ResourceDto dto)
        {
        }
    }
}
