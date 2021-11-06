using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Internal;
using MangaDexSharp.Internal.Dto.Requests.Auth;
using MangaDexSharp.Internal.Dto.Responses.Auth;

namespace MangaDexSharp.Api
{
    /// <summary>
    /// Api for acessing Auth endpoints
    /// </summary>
    /// <remarks>Learn more at https://api.mangadex.org/docs.html#tag/Auth </remarks>
    public class AuthApi : MangaDexApi
    {
        private TokenContainer? _refreshToken;
        private TokenContainer? _token;

        private DateTime? _lastRefresh;

        /// <summary>
        /// Gets last refresh token
        /// </summary>
        public string? LastRefreshToken => _refreshToken?.Token;

        internal AuthApi(MangaDexClient client) : base(client)
        {
            BaseApiPath = MangaDexApiPath + "/auth";
        }

        internal async Task AddAuthorizationHeaders(HttpRequestMessage message, CancellationToken cancelToken)
        {
            if (_lastRefresh != null && DateTime.Now - _lastRefresh.Value > TimeSpan.FromMinutes(14))
            {
                if (_token == null)
                {
                    await Login(cancelToken);
                }
                else
                {
                    await RefreshToken(cancelToken);
                }
            }

            if(_token == null)
            {
                return;
            }

            message.Headers.Add("Authorization", "Bearer " + _token.Token);
        }

        //TODO: Check endpoint

        /// <summary>
        /// Login with <seealso="UserCredentials">
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task Login(CancellationToken cancelToken = default)
        {
            if(mangaDexClient.Credentials == null)
            {
                throw new InvalidOperationException(nameof(UserCredentials) + " should be initialized for auth requests");
            }

            var loginRequest = new LoginRequest(
                mangaDexClient.Credentials.Value.Username,
                mangaDexClient.Credentials.Value.Email,
                mangaDexClient.Credentials.Value.Password);


            var requestMessage = new HttpRequestMessage(HttpMethod.Post, BaseApiPath + "/login");
            requestMessage.Content = JsonContent.Create(loginRequest, new MediaTypeHeaderValue("application/json"), jsonOptions);

            HttpResponseMessage response = await httpClient.SendAsync(requestMessage, cancelToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new HttpRequestException("Request failed with code: " + response.StatusCode);
            }
            
            Stream jsonStream = await response.Content.ReadAsStreamAsync(cancelToken);
            var loginResponse = await JsonSerializer.DeserializeAsync<LoginResponse>(
                jsonStream,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true },
                cancelToken);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _token = new TokenContainer(loginResponse.Token.Session);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            _refreshToken = new TokenContainer(loginResponse.Token.Refresh);
            _lastRefresh = DateTime.Now;

            mangaDexClient.CurrentUser = await mangaDexClient.User.GetLoggedInUserDetails(cancelToken);
        }

        /// <summary>
        /// Login with refresh token
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task LoginWithToken(string refreshToken, CancellationToken cancelToken = default)
        {
            _refreshToken = new TokenContainer(refreshToken);
            await RefreshToken(cancelToken);
            
            mangaDexClient.CurrentUser = await mangaDexClient.User.GetLoggedInUserDetails(cancelToken);
        }

        /// <summary>
        /// Requests token refresh
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task RefreshToken(CancellationToken cancelToken = default)
        {
            if(_refreshToken == null)
            {
                throw new InvalidOperationException("Cannot update token without refresh-token");
            }

            var refreshTokenRequest = new RefreshTokenRequest(_refreshToken.Token);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, BaseApiPath + "/refresh");
            requestMessage.Content = JsonContent.Create(refreshTokenRequest, new MediaTypeHeaderValue("application/json"), jsonOptions);

            HttpResponseMessage response = await httpClient.SendAsync(requestMessage, cancelToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new HttpRequestException("Request failed with code: " + response.StatusCode);
            }
            Stream jsonStream = await response.Content.ReadAsStreamAsync(cancelToken);
            var refreshTokenResponse = await JsonSerializer.DeserializeAsync<RefreshTokenResponse>(
                jsonStream,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true },
                cancelToken);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _token = new TokenContainer(refreshTokenResponse.Token.Session);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            _refreshToken = new TokenContainer(refreshTokenResponse.Token.Refresh);
            _lastRefresh = DateTime.Now;
        }

        /// <summary>
        /// Requests log out for current session
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            await Logout(default);
        }

        /// <summary>
        /// Requests log out for current session
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task Logout(CancellationToken cancelToken)
        {
            //TODO: checks
            var response = await PostRequest<MangaDexResponse>(BaseApiPath + "/logout", cancelToken);
            if (response.IsOk)
            {
                mangaDexClient.CurrentUser = null;
                _lastRefresh = null;
            }
        }

        private sealed class TokenContainer
        {
            public string Token { get; }

            public TokenContainer(string token)
            {
                Token = token;
            }

            public override string ToString()
            {
                return Token;
            }
        }
    }
}
