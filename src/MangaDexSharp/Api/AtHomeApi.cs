using System;
using System.Threading;
using System.Threading.Tasks;
using MangaDexSharp.Api.Data;
using MangaDexSharp.Exceptions;
using MangaDexSharp.Internal.Dto.Responses.AtHome;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Api
{
    /// <summary>
    /// Provides access to at-home endpoints, for retrieving and reading <seealso cref="Chapter"/>
    /// </summary>
    /// <remarks>https://api.mangadex.org/docs.html#tag/AtHome</remarks>
    public class AtHomeApi : MangaDexApi
    {
        internal AtHomeApi(MangaDexClient client) : base(client)
        {
            BaseApiPath = MangaDexApiPath + "/at-home";
        }

        /// <summary>
        /// Gets server url for reading chapter wit given Id
        /// </summary>
        /// <param name="chapterId">Id of <seealso cref="Chapter"/></param>
        /// <param name="forcePort443">If port 443 should be forced</param>
        /// <param name="cancelToken"></param>
        /// <returns>Base server url for reading <seealso cref="Chapter"/>. Valid for 15 minutes from the time of response</returns>
        /// <remarks>
        /// Read more about reading chapter via Api here: https://api.mangadex.org/docs.html#section/Reading-a-chapter-using-the-API
        /// </remarks>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ReadingSessionInformation> GetServerUrlForChapter(Guid chapterId, bool forcePort443, CancellationToken cancelToken = default)
        {
            Chapter source = await mangaDexClient.Chapter.GetChapter(chapterId, new Parameters.IncludeParameters()
            {
                IncludeManga = true,
                IncludeUser = true,
                IncludeScanlationGroup = true
            });

            AtHomeServerResponse response = await GetRequest<AtHomeServerResponse>(
                BaseApiPath + "/server/" + chapterId + (forcePort443 ? "forcePort443=true" : string.Empty),
                cancelToken);

            return new ReadingSessionInformation(
                response.BaseUrl,
                new ChapterReadingMetadata(
                    response.Chapter.Data,
                    response.Chapter.DataSaver,
                    response.Chapter.Hash),
                source);
        }
    }
}
