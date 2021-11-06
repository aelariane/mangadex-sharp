using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Exceptions;
using MangaDexSharp.Internal;
using MangaDexSharp.Internal.Dto;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Follows;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Api
{
    /// <summary>
    /// Provides access to api endpoints related to <seealso cref="User"/>
    /// </summary>
    /// <remarks>Learm more here: https://api.mangadex.org/docs.html#tag/User </remarks>
    public class UserApi : MangaDexApi
    {
        public UserApi(MangaDexClient client) : base(client)
        {
            BaseApiPath = MangaDexApiPath + "/user";
        }

        //TODO: Move follows to dedicated follows api?

        /// <summary>
        /// Get details about logged in user of <seealso cref="MangaDexClient"/>
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<LocalUser> GetLoggedInUserDetails(CancellationToken cancelToken = default)
        {
            UserDto userDto = await GetObjectRequest<UserDto>(
                BaseApiPath + "/me",
                cancelToken,
                true);

            mangaDexClient.Resources.TryRetrieve(userDto, out User? me);

#pragma warning disable CS8604 // Possible null reference argument.
            return new LocalUser(me);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        /// <summary>
        /// Gets information about <seealso cref="User"/> by provided ID
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="includes">Reference expansion</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <remarks>Requires to be logged in</remarks>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<User> GetUser(Guid userId, IncludeParameters? includes = null, CancellationToken cancelToken = default)
        {
            UserDto response = await GetObjectRequest<UserDto>(
                BaseApiPath + "/" + userId,
                includes,
                cancelToken,
                true);

            if (mangaDexClient.Resources.TryRetrieve(response, out User? result) && result != null)
            {
                return result;
            }
            throw new Exception($"Cannot retrieve {nameof(User)} with Id {response.Id}");
        }
    }
}
