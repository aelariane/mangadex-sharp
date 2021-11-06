using System;
using System.Text.Json.Serialization;

using MangaDexSharp.Enums;

namespace MangaDexSharp.Internal.Dto.Requests.CustomList
{
    internal class UpdateCustomListVisibilityRequest
    {
        [JsonPropertyName("visibility")]
        public CustomListVisibility Visibility { get; }

        [JsonPropertyName("version")]
        public int Version { get;  }

        public UpdateCustomListVisibilityRequest(CustomListVisibility visibility, int version)
        {
            Visibility = visibility;
            Version = version;
        }
    }
}
