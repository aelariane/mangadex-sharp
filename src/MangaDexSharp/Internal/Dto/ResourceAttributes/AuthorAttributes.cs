#nullable disable
using System;

using MangaDexSharp.Objects;

namespace MangaDexSharp.Internal.Dto.ResourceAttributes
{
    internal class AuthorAttributes : AuditableResourceAttributes
    {
        public string Name { get; set; }
        public LocalizedString Biography { get; set; }
        public Uri ImageUrl { get; set; }
        public Uri Twitter { get; set; }
        public Uri Pixiv { get; set; }
        public Uri MelonBook { get; set; }
        public Uri FanBox { get; set; }
        public Uri Booth { get; set; }
        public Uri NicoVideo { get; set; }
        public Uri Skeb { get; set; }
        public Uri Fantia { get; set; }
        public Uri Tumblr { get; set; }
        public Uri Youtube { get; set; }
        public Uri Website { get; set; }
    }
}
    