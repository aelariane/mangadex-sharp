#nullable disable
using MangaDexSharp.Internal.Dto.Resources;

namespace MangaDexSharp.Internal
{
    internal class ObjectResponse<T> : MangaDexResponse
        where T : ResourceDto
    {
        public T Data { get; set; }
    }
}
