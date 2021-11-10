using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Objects;
using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Manga;

namespace MangaDexSharp.Resources
{
    /// <summary>
    /// Represents Author or Artist of <see cref="Manga"/>
    /// </summary>
    public class Author : MangaDexAuditableResource
    {
        private bool _noManga = false;
        internal HashSet<Guid> RelatedMangaIds { get; } = new HashSet<Guid>();

        /// <summary>
        /// Name of Author
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Biography of Author
        /// </summary>
        public IReadOnlyDictionary<string, string> Biography { get; }

        /// <summary>
        /// Url to Author's image
        /// </summary>
        /// <remarks>Optional</remarks>
        public Uri? ImageUrl { get; internal set; }

        /// <summary>
        /// Url to Author's twitter profile
        /// </summary>
        /// <remarks>Optional</remarks>
        public Uri? Twitter { get; internal set; }

        /// <summary>
        /// Url to Author's pixiv.net profile
        /// </summary>
        /// <remarks>Optional</remarks>
        public Uri? Pixiv { get; internal set; }

        /// <remarks>Optional</remarks>
        public Uri? MelonBook { get; internal set; }

        /// <summary>
        /// Url to Author's fanbox
        /// </summary>
        /// <remarks>Optional</remarks>
        public Uri? FanBox { get; internal set; }

        /// <summary>
        /// Url to Author's booth
        /// </summary>
        /// <remarks>Optional</remarks>
        public Uri? Booth { get; internal set; }

        /// <summary>
        /// Url to Author's nicivideo.jp channel
        /// </summary>
        /// <remarks>Optional</remarks>
        public Uri? NicoVideo { get; internal set; }

        /// <summary>
        /// Url to Author's skeb.jp profile
        /// </summary>
        /// <remarks>Optional</remarks>
        public Uri? Skeb { get; internal set; }

        /// <summary>
        /// Url to Author's fantia.jp profile
        /// </summary>
        /// <remarks>Optional</remarks>
        public Uri? Fantia { get; internal set; }

        /// <summary>
        /// Url to Author's tumblr
        /// </summary>
        /// <remarks>Optional</remarks>
        public Uri? Tumblr { get; internal set; }

        /// <summary>
        /// Url to Author's YouTube channel
        /// </summary>
        /// <remarks>Optional</remarks>
        public Uri? Youtube { get; internal set; }

        /// <summary>
        /// Url to Author's personal website
        /// </summary>
        /// <remarks>Optional</remarks>
        public Uri? Website { get; internal set; }

        //public bool IsMangaAuthor { get; internal set;  }
        /// <summary>
        /// If Author is also drawer
        /// </summary>
        public bool IsArtist { get; internal set; }

        /// <summary>
        /// Collection of Id to <see cref="Manga"/> this author is related to
        /// </summary>
        public IReadOnlyCollection<Guid> MangaIds => RelatedMangaIds;

        internal Author(
            MangaDexClient client,
            Guid id,
            string name,
            LocalizedString biography,
            DateTime createdAt,
            DateTime updatedAt)
            : base(client, id, createdAt, updatedAt)
        {
            Name = name;
            Biography = biography;
        }

        /// <summary>
        /// Gets collection of <seealso cref="Manga"/> with the author involved in creation process
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>Collection of <seealso cref="Manga"/> where the author is mentioned as creator</returns>
        public async Task<IReadOnlyCollection<Manga>> GetMangaList(CancellationToken cancelToken = default)
        {
            if(RelatedMangaIds.Count == 0)
            {
                if (_noManga)
                {
                    return new List<Manga>();
                }

                Author author = await Client.Author.GetAuthor(Id, new IncludeParameters() { IncludeManga = true }, cancelToken);
                if(author.RelatedMangaIds.Count == 0)
                {
                    _noManga = true;
                }
                return await GetMangaList(cancelToken);
            }
            if (TryGetRelationCollection(RelatedMangaIds, out List<Manga> list))
            {
                return list;
            }

            var parameters = new GetMangaListParameters();
            parameters.MangaIds = RelatedMangaIds;
            parameters.Amount = RelatedMangaIds.Count;
            parameters.Includes = new IncludeParameters()
            {
                IncludeCover = true
            };

            ResourceCollection<Manga> relatedManga = await Client.Manga.GetList(parameters, cancelToken);

            foreach(Manga manga in relatedManga)
            {
                RegisterRelation(manga);
            }

            return relatedManga;
        }
    }
}
