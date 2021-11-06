using System;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Exceptions;
using MangaDexSharp.Internal;
using MangaDexSharp.Internal.Dto;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Author;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Api
{
    /// <summary>
    /// Provides acess to api endpoints related to <seealso cref="Author"/>
    /// </summary>
    /// <remarks>Learn more here: https://api.mangadex.org/docs.html#tag/Author </remarks>
    public class AuthorApi : MangaDexApi
    {
        internal AuthorApi(MangaDexClient client) : base(client)
        {
            BaseApiPath = MangaDexApiPath + "/author";
        }

        /// <summary>
        /// Gets information about <seealso cref="Author"/> with provided id
        /// </summary>
        /// <param name="authorId">Author Id</param>
        /// <param name="includes"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="ForbiddenException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Author> GetAuthor(Guid authorId, IncludeParameters? includes = null, CancellationToken cancelToken = default)
        {
            AuthorDto response = await GetObjectRequest<AuthorDto>(
                BaseApiPath + "/" + authorId,
                includes,
                cancelToken);

            if (mangaDexClient.Resources.TryRetrieve(response, out Author? result) && result != null)
            {
                return result;
            }
            throw new Exception($"Cannot retrieve {nameof(Author)} with Id {response.Id}");
        }


        /// <summary>
        /// Gets list of <seealso cref="Author"/>
        /// </summary>
        /// <param name="parameters">Additional filter parameters</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="ForbiddenException"></exception>
        public async Task<ResourceCollection<Author>> GeList(GetAuthorListParameters? parameters, CancellationToken cancelToken = default)
        {
            CollectionResponse<AuthorDto> response = await GetCollectionRequest<AuthorDto>(
                BaseApiPath,
                parameters,
                cancelToken);

            return mangaDexClient.Resources.MapResponseCollection<AuthorDto, Author>(response);
        }
    }
}
