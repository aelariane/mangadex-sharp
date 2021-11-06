#nullable disable
using System;
using System.Text.Json.Serialization;

namespace MangaDexSharp.Internal.Dto.Requests.Auth
{
    internal class RefreshTokenRequest
    {
        [JsonPropertyName("token")]
        public string Token { get; }

        public RefreshTokenRequest(string token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }
    }
}
