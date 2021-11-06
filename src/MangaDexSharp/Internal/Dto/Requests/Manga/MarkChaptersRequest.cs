using System;
using System.Collections.Generic;

using System.Text.Json.Serialization;

namespace MangaDexSharp.Internal.Dto.Requests.Manga
{
    internal class MarkChaptersRequest
    {
        [JsonPropertyName("chapterIdsRead")]
        public ICollection<Guid> MarkAsRead { get; set; }

        [JsonPropertyName("chapterIdsUnread")]
        public ICollection<Guid> MarkAsUnread { get; set; }

        public MarkChaptersRequest()
        {
            MarkAsRead = new List<Guid>();
            MarkAsUnread = new List<Guid>();
        }

        public MarkChaptersRequest(ICollection<Guid>? read, ICollection<Guid>? unread)
        {
            MarkAsRead = read ?? new List<Guid>();
            MarkAsUnread = unread ?? new List<Guid>();
        }
    }
}
