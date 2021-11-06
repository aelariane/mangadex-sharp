#nullable disable
using MangaDexSharp.Enums;

namespace MangaDexSharp.Internal.Dto.ResourceAttributes
{
    internal class CustomListAttributes : BaseResourceAttributes
    {
        public string Name { get; set; }
        public CustomListVisibility Visibility {  get; set; }
    }
}
