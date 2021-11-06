using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using MangaDexSharp.Enums;

namespace MangaDexSharp.Internal.Dto.Requests.CustomList
{
    internal class CreateCustomListRequest
    {
        [JsonPropertyName("manga")]
        public IEnumerable<Guid> MangaIds { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("version")]
        public int Version { get; set; } = 1;

        [JsonPropertyName("visibility")]
        public CustomListVisibility Visibilty { get; }

        public CreateCustomListRequest(
            string name,
            CustomListVisibility visibility,
            IEnumerable<Guid>? mangas)
        {
            Name = name;
            MangaIds = mangas ?? new Guid[0];
            Visibilty = visibility;
        }
    }
}
