#nullable disable
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MangaDexSharp.Internal.JsonConverters
{
    internal class GenericDictionaryConverter<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(Dictionary<TKey, TValue>)
                || typeof(Dictionary<TKey, TValue>).IsAssignableTo(typeToConvert);
        }

        public override Dictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read();
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return new Dictionary<TKey, TValue>();
                }
                throw new JsonException($"Cannot read sequence with starting with StartArray for"
                    + $"IDictionary<{typeof(TKey).Name}, {typeof(TValue).Name}");
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                var cOptions = new JsonSerializerOptions(options);
                cOptions.Converters.Remove(this);

                Utf8JsonReader copy = reader;
                object value = JsonSerializer.Deserialize(ref copy, typeof(Dictionary<TKey, TValue>), cOptions);
                reader = copy;
                return value as Dictionary<TKey, TValue>;
            }
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializerOptions options)
        {
            var cOptions = new JsonSerializerOptions(options);
            cOptions.Converters.Remove(this);
            JsonSerializer.Serialize(writer, value, typeof(Dictionary<TKey, TValue>), cOptions);
        }
    }
}


//#nullable disable
//using System;
//using System.Collections.Generic;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace MangaDexSharp.Internal.JsonConverters
//{
//    internal class GenericDictionaryConverter<TKey, TValue> : JsonConverter<IDictionary<TKey, TValue>>
//    {
//        public override bool CanConvert(Type typeToConvert)
//        {
//            return typeToConvert.IsAssignableTo(typeof(IDictionary<TKey, TValue>));
//        }

//        public override IDictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            if(reader.TokenType == JsonTokenType.StartArray)
//            {
//                reader.Read();
//                if(reader.TokenType == JsonTokenType.EndArray)
//                {
//                    return new Dictionary<TKey, TValue>();
//                }
//                throw new JsonException($"Cannot read sequence with starting with StartArray for"
//                    + $"IDictionary<{typeof(TKey).Name}, {typeof(TValue).Name}");
//            }
//            else if(reader.TokenType == JsonTokenType.StartObject)
//            {
//                var cOptions = new JsonSerializerOptions(options);
//                cOptions.Converters.Remove(this);

//                Utf8JsonReader copy = reader;
//                object value = JsonSerializer.Deserialize(ref copy, typeof(IDictionary<TKey, TValue>), cOptions);
//                reader = copy;
//                return value as IDictionary<TKey, TValue>;
//            }
//            throw new NotImplementedException();
//        }

//        public override void Write(Utf8JsonWriter writer, IDictionary<TKey, TValue> value, JsonSerializerOptions options)
//        {
//            var cOptions = new JsonSerializerOptions(options);
//            cOptions.Converters.Remove(this);
//            JsonSerializer.Serialize(writer, value, typeof(IDictionary<TKey, TValue>), cOptions);
//        }
//    }
//}
