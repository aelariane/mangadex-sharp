using System;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Exceptions;
using MangaDexSharp.Internal;
using MangaDexSharp.Internal.Dto;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Chapter;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Api
{
    /// <summary>
    /// Provides access to api endpoints related to <seealso cref="Chapter"/> and some chapter read markers endpoints
    /// </summary>
    /// <remarks>Learn more here: https://api.mangadex.org/docs.html#tag/Chapter and https://api.mangadex.org/docs.html#tag/ChapterReadMarker</remarks>
    public class ChapterApi : MangaDexApi
    {
        internal ChapterApi(MangaDexClient client) : base(client)
        {
            BaseApiPath = MangaDexApiPath + "/chapter";
        }

        /// <summary>
        /// Gets specific chapter
        /// </summary>
        /// <param name="chapterId">Chapter Id</param>
        /// <param name="includes">Relations expansions</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="ForbiddenException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Chapter> GetChapter(
            Guid chapterId,
            IncludeParameters? includes,
            CancellationToken cancelToken = default)
        {
            ChapterDto dto = await GetObjectRequest<ChapterDto>(
                BaseApiPath + "/" + chapterId,
                includes,
                cancelToken);

            if (mangaDexClient.Resources.TryRetrieve(dto, out Chapter? result) && result != null)
            {
                return result;
            }
            throw new Exception($"Cannot retrieve chapter with Id {dto.Id}");
        }


        /// <summary>
        /// Gets list of chapters
        /// </summary>
        /// <param name="parameters">Additional filter parameters</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="ForbiddenException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ResourceCollection<Chapter>> GetList(
            GetChapterListParameters? parameters,
            CancellationToken cancelToken = default)
        {
            CollectionResponse<ChapterDto> dto = await GetCollectionRequest<ChapterDto>(
               BaseApiPath,
               parameters,
               cancelToken);

            return mangaDexClient.Resources.MapResponseCollection<ChapterDto, Chapter>(dto);
        }

        /// <summary>
        /// Mark chapter as read for <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task MarkChapterRead(Guid chapterId, CancellationToken cancelToken = default)
        {
            await PostRequest<MangaDexResponse>(
                BaseApiPath + "/" + chapterId + "/read",
                cancelToken);
        }

        /// <summary>
        /// Mark chapter as unread for <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task MarkChapterUnread(Guid chapterId, CancellationToken cancelToken = default)
        {
            await DeleteRequest<MangaDexResponse>(
                   BaseApiPath + "/" + chapterId + "/read",
                   cancelToken);
        }
    }
}
