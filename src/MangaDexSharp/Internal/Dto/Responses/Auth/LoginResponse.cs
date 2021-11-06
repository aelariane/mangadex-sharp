#nullable disable
using MangaDexSharp.Internal.Dto.Responses.Objects;

namespace MangaDexSharp.Internal.Dto.Responses.Auth
{
    internal class LoginResponse : MangaDexResponse
    {
        public TokenDto Token { get; set; }
    }
}
