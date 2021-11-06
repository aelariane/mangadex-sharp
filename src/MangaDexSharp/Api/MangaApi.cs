using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Api.Data;
using MangaDexSharp.Collections;
using MangaDexSharp.Enums;
using MangaDexSharp.Exceptions;
using MangaDexSharp.Internal;
using MangaDexSharp.Internal.Dto;
using MangaDexSharp.Internal.Dto.Requests.Manga;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Internal.Dto.Responses.Manga;
using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Manga;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Api
{
    /// <summary>
    /// Provides access to api endpoints related t <seealso cref="Manga"/> and some chapter read markers endpoints
    /// </summary>
    /// <remarks>Learn more here: https://api.mangadex.org/docs.html#tag/Manga and https://api.mangadex.org/docs.html#tag/ChapterReadMarker </remarks>
    public class MangaApi : MangaDexApi
    {
        internal MangaApi(MangaDexClient client) : base(client)
        {
            BaseApiPath = MangaDexApiPath + "/manga";
        }

        /// <summary>
        /// Gets information about volumes and chapters of specific manga
        /// </summary>
        /// <param name="mangaId">Manga Id</param>
        /// <param name="parameters">Additinal filter parameters</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<MangaVolumeInfo>> Aggregate(
            Guid mangaId,
            AggregateMangaParameters? parameters = null,
            CancellationToken cancelToken = default)
        {
            string query = BaseApiPath + "/" + mangaId + "/aggregate";
            query = query.BuildQuery(parameters);

            MangaVolumeInfoResponse volumeInfoResponse = await GetRequest<MangaVolumeInfoResponse>(query, cancelToken);

            return volumeInfoResponse.Volumes.Select(x =>
                new MangaVolumeInfo(
                    x.Value.Volume,
                    x.Value.Chapters.Select(ci =>
                       new KeyValuePair<string, MangaChapterInfo>(
                            ci.Key,
                            new MangaChapterInfo(ci.Value.Id, ci.Value.Chapter, ci.Value.Count))),
                    x.Value.Count))
                .ToList();
        }

        /// <summary>
        /// Follows <see cref="MangaDexClient.CurrentUser"/> to specifc manga
        /// </summary>
        /// <param name="mangaId">Manga id</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task FollowManga(Guid mangaId, CancellationToken cancelToken = default)
        {
            await PostRequest<MangaDexResponse>(
              BaseApiPath + "/" + mangaId + "/follow",
              cancelToken);
        }

        /// <summary>
        /// Gets list of manga
        /// </summary>
        /// <param name="parameters">Additional filter parameters</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public async Task<ResourceCollection<Manga>> GetList(GetMangaListParameters? parameters = null, CancellationToken cancelToken = default)
        {
            CollectionResponse<MangaDto> dto = await GetCollectionRequest<MangaDto>(
                BaseApiPath,
                parameters,
                cancelToken);

            return mangaDexClient.Resources.MapResponseCollection<MangaDto, Manga>(dto);
        }

        /// <summary>
        /// Gets feed of manga
        /// </summary>
        /// <param name="mangaId">Manga id</param>
        /// <param name="parameters">Additional filter parameters</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public async Task<ResourceCollection<Chapter>> GetMangaFeed(
            Guid mangaId,
            GetMangaFeedParameters? parameters,
            CancellationToken cancelToken = default)
        {
            CollectionResponse<ChapterDto> dto = await GetCollectionRequest<ChapterDto>(
                BaseApiPath + "/" + mangaId + "/feed",
                parameters,
                cancelToken);

            return mangaDexClient.Resources.MapResponseCollection<ChapterDto, Chapter>(dto);
        }

        /// <summary>
        /// Gets list of mangas with given status of <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="status">Status filter</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<IReadOnlyCollection<Guid>> GetMangasWithReadingStatusOfUser(
            MangaReadingStatus status,
            CancellationToken cancelToken = default)
        {
            AllMangaReadingStatusesResponse response = await GetRequest<AllMangaReadingStatusesResponse>(
                BaseApiPath + "/status",
                new GetReadingStatusesParameters(status),
                cancelToken,
                true);

            if (response.Statuses == null || response.Statuses.Any())
            {
                return new Guid[0];
            }

            return response.Statuses
                .Select(X => X.Key)
                .ToArray();
        }


        /// <summary>
        /// Gets reading status of specific manga for <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="mangaId">Manga id</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<MangaReadingStatus?> GetMangaReadingStatus(Guid mangaId, CancellationToken cancelToken = default)
        {
            MangaReadingStatusResponse response = await GetRequest<MangaReadingStatusResponse>(
                BaseApiPath + "/" + mangaId + "/status",
                cancelToken,
                true);

            return response.Status;
        }

        /// <summary>
        /// Gets random manda
        /// </summary>
        /// <param name="includes"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<Manga> GetRandomManga(IncludeParameters? includes, CancellationToken cancelToken = default)
        {
            MangaDto dto = await GetObjectRequest<MangaDto>(
                BaseApiPath + "/random",
                includes,
                cancelToken);

            if (mangaDexClient.Resources.TryRetrieve(dto, out Manga? result) && result != null)
            {
                return result;
            }
            throw new Exception("Something went wrong when requesting random manga");
        }


        /// <summary>
        /// Gets list of chapter ids that are marked as read for the given manga
        /// </summary>
        /// <param name="mangaId">Manga id</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="BadRequestException"></exception>
        public async Task<IReadOnlyCollection<Guid>> GetReadMarkersOfManga(
            Guid mangaId,
            CancellationToken cancelToken = default)
        {
            MangaReadMarkersResponse response = await GetRequest<MangaReadMarkersResponse>(
                BaseApiPath + "/" + mangaId + "/read",
                cancelToken,
                true);

            return response.Data;
        }

        /// <summary>
        /// Gets list of chapter ids that are marked as read for the given manga ids
        /// </summary>
        /// <param name="mangaIds">Manga ids</param>
        /// <param name="cancelToken"></param>
        /// <returns>Ids of <seealso cref="Chapter"/> marked as read for given Mangas</returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="BadRequestException"></exception>
        public async Task<IReadOnlyCollection<Guid>> GetReadMarkers(ICollection<Guid> mangaIds, CancellationToken cancelToken = default)
        {
            var parameters = new GetReadMarkersParameters(mangaIds);
            parameters.Grouped = false;

            MangaReadMarkersResponse response = await GetRequest<MangaReadMarkersResponse>(
                BaseApiPath + "/read",
                parameters,
                cancelToken,
                true);

            return response.Data;
        }

        /// <summary>
        /// Gets list of chapter ids that are marked as read for the given manga ids, grouped by manga id
        /// </summary>
        /// <param name="mangaIds">Manga ids</param>
        /// <param name="cancelToken"></param>
        /// <returns>Ids of <seealso cref="Chapter"/> marked as read grouped by <see cref="Manga"/> Id</returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="BadRequestException"></exception>
        public async Task<IReadOnlyDictionary<Guid, IReadOnlyCollection<Guid>>> GetReadMarkersGrouped(ICollection<Guid> mangaIds, CancellationToken cancelToken = default)
        {
            var parameters = new GetReadMarkersParameters(mangaIds);

            GroupedMangaReadMarkersResponse response = await GetRequest<GroupedMangaReadMarkersResponse>(
                BaseApiPath + "/read",
                parameters,
                cancelToken,
                true);

            return new Dictionary<Guid, IReadOnlyCollection<Guid>>(response.Data);
        }

        /// <summary>
        /// Gets information about related mangas
        /// </summary>
        /// <param name="mangaId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Related mangas ids with information about relation</returns>
        /// <exception cref="BadRequestException"></exception>
        public async Task<IReadOnlyDictionary<Guid, MangaRelation>> GetRelatedMangas(
            Guid mangaId,
            CancellationToken cancellationToken = default)
        {
            CollectionResponse<MangaRelationDto> response = await GetCollectionRequest<MangaRelationDto>(
                BaseApiPath + "/" + mangaId + "/relation",
                cancellationToken);

            IEnumerable<KeyValuePair<Guid, MangaRelation>> pairs = response.Data.Select(x =>
                new KeyValuePair<Guid, MangaRelation>(
                    x.MangaRelations.First().Id,
                    x.Attributes.Relation));

            return new Dictionary<Guid, MangaRelation>(pairs);
        }

        /// <summary>
        /// Gets all reading statuses of mangas of <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>All reading statuses of <see cref="MangaDexClient.CurrentUser"/></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<IReadOnlyDictionary<Guid, MangaReadingStatus>> GetReadingStatusesOfUser(CancellationToken cancelToken = default)
        {
            AllMangaReadingStatusesResponse response = await GetRequest<AllMangaReadingStatusesResponse>(
                BaseApiPath + "/status",
                cancelToken,
                true);

            return new Dictionary<Guid, MangaReadingStatus>(response.Statuses);
        }

        /// <summary>
        /// Gets list of all tags
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>List of tags</returns>
        public async Task<ResourceCollection<Tag>> GetTagList(CancellationToken cancelToken = default)
        {
            CollectionResponse<TagDto> dto = await GetCollectionRequest<TagDto>(
                BaseApiPath + "/tag",
                cancelToken);

            return mangaDexClient.Resources.MapResponseCollection<TagDto, Tag>(dto);
        }

        /// <summary>
        /// Gets list of chapter ids that are marked as read for the given manga
        /// </summary>
        /// <param name="mangaIds">Manga id</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="BadRequestException"></exception>
        public async Task MarkChaptersOfManga(
            Guid mangaId,
            ICollection<Guid>? markChaptersRead,
            ICollection<Guid>? markChaptersUnread,
            CancellationToken cancelToken = default)
        {
            var request = new MarkChaptersRequest(markChaptersRead, markChaptersUnread);

            await PostRequestWithBody<MarkChaptersRequest, MangaDexResponse>(
                request,
                BaseApiPath + "/" + mangaId + "/read",
                cancelToken);
        }

        /// <summary>
        /// Unfollows <see cref="MangaDexClient.CurrentUser"/> from specifc manga
        /// </summary>
        /// <param name="mangaId">Manga id</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task UnfollowManga(Guid mangaId, CancellationToken cancelToken = default)
        {
            await DeleteRequest<MangaDexResponse>(
              BaseApiPath + "/" + mangaId + "/follow",
              cancelToken);
        }

        /// <summary>
        /// Update reading status of <see cref="MangaDexClient.CurrentUser"/> for specific manga
        /// </summary>
        /// <param name="mangaId">Manga Id</param>
        /// <param name="newStatus">New reading status</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task UpdateReadingStatus(Guid mangaId, MangaReadingStatus? newStatus, CancellationToken cancelToken = default)
        {
            var request = new UpdateMangaReadingStatusRequest(newStatus);

            await PostRequestWithBody<UpdateMangaReadingStatusRequest, MangaDexResponse>(
                request,
                BaseApiPath + "/" + mangaId + "/status",
                cancelToken);
        }

        /// <summary>
        /// Gets information about manga with provided id
        /// </summary>
        /// <param name="mangaId">Manga Id</param>
        /// <param name="includes"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Manga> ViewManga(
            Guid mangaId,
            IncludeParameters? includes = null,
            CancellationToken cancelToken = default)
        {
            MangaDto dto = await GetObjectRequest<MangaDto>(
                BaseApiPath + "/" + mangaId,
                includes,
                cancelToken);

            if (mangaDexClient.Resources.TryRetrieve(dto, out Manga? result) && result != null)
            {
                return result;
            }
            throw new Exception($"Cannot retrieve manga with Id {dto.Id}");
        }
    }
}
