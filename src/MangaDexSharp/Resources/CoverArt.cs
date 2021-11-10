using System;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Constants;
using MangaDexSharp.Parameters;

namespace MangaDexSharp.Resources
{
    /// <summary>
    /// Represents Cover Art of Manga volume
    /// </summary>
    public class CoverArt : MangaDexAuditableResource
    {
        public string? Description { get; internal set; }

        /// <summary>
        /// File name of Cover
        /// </summary>
        /// <remarks>Read more here: https://api.mangadex.org/docs.html#section/Retrieving-Covers-from-the-API</remarks>
        public string FileName { get; }

        /// <summary>
        /// Id of <seealso cref="Manga"/> the Cover belongs to
        /// </summary>
        public Guid MangaId { get; internal set; }

        /// <summary>
        /// Url to source quality image
        /// </summary>
        public Uri SourceUrl => new Uri(string.Format(CoverUrls.Source, MangaId, FileName));

        /// <summary>
        /// Id of <seealso cref="User"/> who uploaded the Cover
        /// </summary>
        public Guid UploaderId { get; internal set; }

        /// <summary>
        /// Url to 256 pixel wide thumbnail
        /// </summary>
        public Uri Thumbnail256Px => new Uri(string.Format(CoverUrls.Thumbnail256Px, MangaId, FileName));

        /// <summary>
        /// Url to 512 pixel wide thumbnail
        /// </summary>
        public Uri Thumbnail512Px => new Uri(string.Format(CoverUrls.Thumbnail512Px, MangaId, FileName));

        /// <summary>
        /// Volume the Cover belongs to
        /// </summary>
        public string? Volume { get; internal set; }

        internal CoverArt(
            MangaDexClient client,
            Guid id,
            string fileName,
            DateTime createdAt,
            DateTime updatedAt)
            : base(client, id, createdAt, updatedAt)
        {
            FileName = fileName;
        }

        /// <summary>
        /// Gets <seealso cref="Manga"/> the Cover belongs to
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<Manga> GetManga(CancellationToken cancelToken = default)
        {
            if (MangaId == Guid.Empty)
            {
                await Client.Cover.GetCover(Id, null, cancelToken);
            }
            if (TryGetRelation(MangaId, out Manga? manga) && manga != null)
            {
                return manga;
            }
            
            Manga myManga = await Client.Manga.ViewManga(MangaId, null, cancelToken);
            RegisterRelation(myManga);

            return myManga;
        }

        /// <summary>
        /// Gets <seealso cref="User"/> who uploaded the Cover
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<User> GetUploader(CancellationToken cancelToken = default)
        {
            if(UploaderId == Guid.Empty)
            {
                await Client.Cover.GetCover(Id, new IncludeParameters() { IncludeUser = true, IncludeManga = true }, cancelToken);
            }
            if (TryGetRelation(UploaderId, out User? user) && user != null)
            {
                return user;
            }

            user =  await Client.User.GetUser(UploaderId, null, cancelToken);
            RegisterRelation(user);

            return user;
        }
    }
}
    