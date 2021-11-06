#nullable disable
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.ResourceAttributes
{
    internal class UserAttributes : BaseResourceAttributes
    {
        public string Username { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
