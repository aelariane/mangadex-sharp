#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Internal.JsonConverters
{
    internal static class ConverterExtensions
    {
        private static List<JsonConverter> _converters;

        public static IEnumerable<JsonConverter> GetMappableEnumConverters()
        {
            if (_converters != null)
            {
                return _converters;
            }

            IEnumerable<Type> typesToUse = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => x.IsEnum && x.IsDefined(typeof(MappableEnumAttribute)));

            _converters = new List<JsonConverter>();
            foreach (Type enumType in typesToUse)
            {
                JsonConverter converter = Activator.CreateInstance(typeof(MappableEnumConverter<>)
                    .MakeGenericType(enumType)) as JsonConverter;

                JsonConverter nullableConverter = Activator.CreateInstance(typeof(NullableMappableEnumConverter<>)
                    .MakeGenericType(enumType)) as JsonConverter;

                _converters.Add(converter);
                _converters.Add(nullableConverter);
            }

            return _converters;
        }
    }
}
