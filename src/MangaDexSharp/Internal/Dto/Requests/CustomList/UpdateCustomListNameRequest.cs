using System;
using System.Text.Json.Serialization;

namespace MangaDexSharp.Internal.Dto.Requests.CustomList
{
    internal class UpdateCustomListNameRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("version")]
        public int Version { get; }

        public UpdateCustomListNameRequest(string name, int version)
        {
            Name = name;
            Version = version;
        }
    }
}
