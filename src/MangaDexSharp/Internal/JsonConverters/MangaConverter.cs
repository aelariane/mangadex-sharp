#nullable disable
using System.Text.Json;

using MangaDexSharp.Enums;
using MangaDexSharp.Internal.Dto.Resources;

namespace MangaDexSharp.Internal.JsonConverters
{
    internal class MangaConverter : MangaDexResourceConverter<MangaDto>
    {
        protected override void HandleUncommonToken(ref Utf8JsonReader reader, MangaDto resource, JsonSerializerOptions options)
        {
            string propertyName = reader.GetString();
            if (options.PropertyNameCaseInsensitive)
            {
                propertyName = propertyName.ToLower();
            }
            if (propertyName == "related")
            {
                reader.Read();
                Utf8JsonReader copy = reader;
                resource.Related = (MangaRelation) JsonSerializer.Deserialize(ref copy, typeof(MangaRelation), options);
                reader = copy;
            }
            else
            {
                base.HandleUncommonToken(ref reader, resource, options);
            }
        }
    }
}
