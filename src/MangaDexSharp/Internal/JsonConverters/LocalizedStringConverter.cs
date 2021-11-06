#nullable disable
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using MangaDexSharp.Objects;

namespace MangaDexSharp.Internal.JsonConverters
{
    internal class LocalizedStringConverter : JsonConverter<LocalizedString>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return base.CanConvert(typeToConvert);
        }

        public override LocalizedString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read();
                if(reader.TokenType != JsonTokenType.EndArray)
                {
                    throw new JsonException("EndArray expected after StartArray for LocalizedString");
                }
                return new LocalizedString();
            }
            else if(reader.TokenType == JsonTokenType.StartObject)
            {
                LocalizedString result = new LocalizedString();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        break;
                    }

                    string lang = reader.GetString();
                    reader.Read();
                    string value = reader.GetString();

                    result.Add(lang, value);
                }
                return result;
            }
            throw new JsonException("Unknown start token for LocalizedString: " + reader.TokenType.ToString());
        }

        public override void Write(Utf8JsonWriter writer, LocalizedString value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
