#nullable disable
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Internal.JsonConverters
{
    internal class MappableEnumConverter<TEnum> : JsonConverter<TEnum> 
        where TEnum : struct
    {
        private Dictionary<string, TEnum> _map;
        private Dictionary<TEnum, string> _reverseMap;

        public MappableEnumConverter()
        {
            var myType = typeof(TEnum);
            var namedMembers = myType.GetFields(BindingFlags.Public | BindingFlags.Static);

            _map = new Dictionary<string, TEnum>();
            _reverseMap = new Dictionary<TEnum, string>();
            foreach(FieldInfo field in namedMembers)
            {
                IEnumerable<EnumFieldStringValueAttribute> stringNames = field.GetCustomAttributes<EnumFieldStringValueAttribute>();

                foreach(EnumFieldStringValueAttribute attribute in stringNames)
                {
                    TEnum value = (TEnum)field.GetValue(null);
                    _map.Add(attribute.Value, value);
                    _reverseMap.Add(value, attribute.Value);
                }
            }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(TEnum);
        }

        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType == JsonTokenType.String)
            {
                string propertyName = reader.GetString();
                if(_map.TryGetValue(propertyName, out TEnum result))
                {
                    return result;
                }
                return default(TEnum);
            }
            throw new JsonException("Enum can be converted only from string");
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            if (_reverseMap.TryGetValue(value, out string stringValue))
            {
                writer.WriteStringValue(stringValue);
                return;
            }
            throw new JsonException("Enum can be converted only from string");
        }
    }
}
