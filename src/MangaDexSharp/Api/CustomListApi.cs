using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Enums;
using MangaDexSharp.Exceptions;
using MangaDexSharp.Internal;
using MangaDexSharp.Internal.Dto;
using MangaDexSharp.Internal.Dto.Requests.CustomList;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Internal.ResourceFactories;
using MangaDexSharp.Parameters.CustomList;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Api
{
    /// <summary>
    /// Provides access to <seealso cref="CustomList"/> related api endpoints
    /// </summary>
    /// <remarks>Learn more here: https://api.mangadex.org/docs.html#tag/CustomList </remarks>
    public class CustomListApi : MangaDexApi
    {
        internal CustomListApi(MangaDexClient client) : base(client)
        {
            BaseApiPath = MangaDexApiPath + "/list";
        }

        /// <summary>Adds <seealso cref="Manga"/> to <seealso cref="CustomList"/></summary>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="ForbiddenException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task AddMangaToList(Guid listId, Guid mangaId, CancellationToken cancelToken = default)
        {
            await PostRequest<MangaDexResponse>(
                mangaDexClient.Manga.BaseApiPath + "/" + mangaId + "/list/" + listId,
                cancelToken);
        }

        /// <summary>
        /// Creates <seealso cref="CustomList"/> with given parameters for <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="listName">Name of list</param>
        /// <param name="visibility">Visibility of list (public by default)</param>
        /// <param name="initialMangas">Initial list of <seealso cref="Manga"/></param>
        /// <param name="cancelToken"></param>
        /// <returns>Created <seealso cref="CustomList"/></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<CustomList> CreateCustomList(
            string listName,
            CustomListVisibility visibility = CustomListVisibility.Public,
            IEnumerable<Guid>? initialMangas = null,
            CancellationToken cancelToken = default)
        {
            if(listName == null)
            {
                throw new ArgumentNullException(nameof(listName));
            }
            initialMangas ??= Array.Empty<Guid>();

            CreateCustomListRequest createRequest = new CreateCustomListRequest(listName, visibility, initialMangas);

            ObjectResponse<CustomListDto> response = await PostRequestWithBody<CreateCustomListRequest, ObjectResponse<CustomListDto>>(
               createRequest,
               BaseApiPath,
               cancelToken,
               null);

            if (mangaDexClient.Resources.TryRetrieve(response.Data, out CustomList? result) && result != null)
            {
                return result;
            }
            throw new Exception($"Cannot retrieve {nameof(CustomList)} with Id {response.Data.Id}");
        }

        /// <summary>
        /// Deletes <seealso cref="CustomList"/> of <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="listId">Id of <seealso cref="CustomList"/> to delete</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="ForbiddenException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task DeleteCustomList(Guid listId, CancellationToken cancelToken = default)
        {
            await DeleteRequest<MangaDexResponse>(
                BaseApiPath + "/" + listId,
                cancelToken);
        }

        /// <summary>
        /// Gets <seealso cref="CustomList"/> by Id
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<CustomList> GetCustomList(Guid listId, CancellationToken cancelToken = default)
        {
            CustomListDto response = await GetObjectRequest<CustomListDto>(
                BaseApiPath + "/" + listId,
                cancelToken,
                false);

            if (mangaDexClient.Resources.TryRetrieve(response, out CustomList? result) && result != null)
            {
                return result;
            }
            throw new Exception($"Cannot retrieve {nameof(CustomList)} with Id {response.Id}");
        }

        /// <summary>
        /// Gets Feed of the lst
        /// </summary>
        /// <param name="listId">Id of list</param>
        /// <param name="parameters"></param>
        /// <param name="cancelToken"></param>
        /// <returns>Chapters</returns>
        public async Task<ResourceCollection<Chapter>> GetFeed(
            Guid listId,
            GetCustomListFeedParameters? parameters = null,
            CancellationToken cancelToken = default)
        {
            CollectionResponse<ChapterDto> response = await GetCollectionRequest<ChapterDto>(
                BaseApiPath + "/" + listId + "/feed",
                parameters,
                cancelToken,
                true);

            return mangaDexClient.Resources.MapResponseCollection<ChapterDto, Chapter>(response);
        }

        /// <summary>
        /// Gets all lists of <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cancelToken"></param>
        /// <returns>Collection of all <seealso cref="CustomList"/> that belong to <see cref="MangaDexClient.CurrentUser"/></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<ResourceCollection<CustomList>> GetListsOfLoggedInUser(
            GetUserCustomListsParameters? parameters = null,
            CancellationToken cancelToken = default)
        {
            CollectionResponse<CustomListDto> response = await GetCollectionRequest<CustomListDto>(
                mangaDexClient.User.BaseApiPath + "/list",
                parameters,
                cancelToken,
                true);

            return mangaDexClient.Resources.MapResponseCollection<CustomListDto, CustomList>(response);
        }

        /// <summary>
        /// Gets list of public <seealso cref="CustomList"/> of specific <seealso cref="User"/>
        /// </summary>
        /// <param name="userId">Id of User</param>
        /// <param name="parameters"></param>
        /// <param name="cancelToken"></param>
        /// <returns>Collection of public lists for <seealso cref="User"/> with provided Id</returns>
        public async Task<ResourceCollection<CustomList>> GetListsOfUser(
            Guid userId,
            GetUserCustomListsParameters? parameters = null,
            CancellationToken cancelToken = default)
        {
            CollectionResponse<CustomListDto> response = await GetCollectionRequest<CustomListDto>(
                mangaDexClient.User.BaseApiPath + "/" + userId + "/list",
                parameters,
                cancelToken);

            return mangaDexClient.Resources.MapResponseCollection<CustomListDto, CustomList>(response);
        }

        /// <summary>
        /// Removes <seealso cref="Manga"/> from <seealso cref="CustomList"/>
        /// </summary>
        /// <param name="listId">Id of <seealso cref="CustomList"/> to remove from</param>
        /// <param name="mangaId">Id of <seealso cref="Manga"/> to remove</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="ForbiddenException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task RemoveMangaFromList(Guid listId, Guid mangaId, CancellationToken cancelToken = default)
        {
            await DeleteRequest<MangaDexResponse>(
                mangaDexClient.Manga.BaseApiPath + "/" + mangaId + "/list/" + listId,
                cancelToken);
        }

        /// <summary>
        /// Updates name of <seealso cref="CustomList"/>
        /// </summary>
        /// <param name="listId">Id of <seealso cref="CustomList"/> to update</param>
        /// <param name="newName">New name to set</param>
        /// <param name="version">Current version of <seealso cref="CustomList"/></param>
        /// <param name="cancelToken"></param>
        /// <returns>Updated <seealso cref="CustomList"/></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="ForbiddenException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<CustomList> UpdateCustomListName(Guid listId, string newName, int version, CancellationToken cancelToken = default)
        {
            if(newName == null)
            {
                throw new ArgumentNullException(nameof(newName));
            }
            var updateRequest = new UpdateCustomListNameRequest(newName, version);

            ObjectResponse<CustomListDto> response = await SendRequestWithBody<UpdateCustomListNameRequest, ObjectResponse<CustomListDto>>(
                HttpMethod.Put,
                updateRequest,
                BaseApiPath + "/" + listId,
                cancelToken,
                null,
                true);

            var factory = new CustomListFactory(mangaDexClient);
#pragma warning disable CS8603 // Possible null reference return.
            return factory.Create(response.Data) as CustomList;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Updates visibility of <seealso cref="CustomList"/>
        /// </summary>
        /// <param name="listId">Id of <seealso cref="CustomList"/> to update</param>
        /// <param name="newVisibility">Updated visibility</param>
        /// <param name="version">Current version of <seealso cref="CustomList"/></param>
        /// <param name="cancelToken"></param>
        /// <returns>Updated <seealso cref="CustomList"/></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="UnauthorizedException"></exception>
        /// <exception cref="ForbiddenException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<CustomList> UpdateCustomListVisibility(
            Guid listId,
            CustomListVisibility newVisibility,
            int version,
            CancellationToken cancelToken = default)
        {
            var updateRequest = new UpdateCustomListVisibilityRequest(newVisibility, version);

            ObjectResponse<CustomListDto> response = await SendRequestWithBody<UpdateCustomListVisibilityRequest, ObjectResponse<CustomListDto>>(
                HttpMethod.Put,
                updateRequest,
                BaseApiPath + "/" + listId,
                cancelToken,
                null,
                true);

            var factory = new CustomListFactory(mangaDexClient);
#pragma warning disable CS8603 // Possible null reference return.
            return factory.Create(response.Data) as CustomList;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
