#nullable disable
using MangaDexSharp.Internal.Dto.Responses.Objects;

namespace MangaDexSharp.Internal.Dto.Responses.Auth
{
    internal class RefreshTokenResponse : MangaDexResponse
    {
        public TokenDto Token { get; set; }
        public string Message { get; set; }
    }
}
