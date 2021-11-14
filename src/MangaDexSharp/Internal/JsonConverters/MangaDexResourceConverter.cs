#nullable disable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Internal.Dto.Resources;

namespace MangaDexSharp.Internal.JsonConverters
{
    internal class MangaDexResourceConverter<TResource> : JsonConverter<TResource> 
        where TResource : ResourceDto, new()
    {
        public const string PropertyNameAttributes = "attributes";
        public const string PropertyNameId = "id";
        public const string PropertyNameRelationships = "relationships";
        public const string PropertyNameType = "type";

        private static readonly Dictionary<Type, IEnumerable<RelationshipMetadata>> _relationshipDataCache 
            = new Dictionary<Type, IEnumerable<RelationshipMetadata>>();

        private void ReadAndApplyRelationships(ref Utf8JsonReader reader, TResource resource, JsonSerializerOptions options)
        {
            IEnumerable<RelationshipMetadata> relationsMetadata = GetRelationshipsMetadata(typeof(TResource));

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }
                else if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException("Unpredicted token: " + reader.TokenType + ": " + reader.GetString());
                }

                Utf8JsonReader copy = reader;

                reader.Read();
                //TODO: Check id propertyName
                reader.Read();
                Guid currentId = reader.GetGuid();

                reader.Read();
                //TODO: Check type propertyName
                reader.Read();
                string typeName = reader.GetString();

                //Attempt to get relation metadata
                RelationshipMetadata metadata = relationsMetadata.SingleOrDefault(x => x.RelationshipName.Equals(typeName, StringComparison.OrdinalIgnoreCase));

                if (metadata == null)
                {
                    throw new JsonException("Unknown relationship type: " 
                        + typeName 
                        + " for object "
                        + typeof(TResource).Name 
                        + "(Id: " + resource.Id 
                        + ")");
                }

                ResourceDto relation = JsonSerializer.Deserialize(
                    ref copy,
                    metadata.RelationshipType,
                    options) as ResourceDto;

                //Set known properties
                relation.Id = currentId;
                relation.Type = typeName;

                PropertyInfo prop = resource
                    .GetType()
                    .GetProperty(metadata.Property.Name);

                IList relationList;
                if(prop == null || prop.GetMethod.Invoke(resource, null) == null)
                {
                    Type listType = typeof(List<>)
                        .MakeGenericType(metadata.RelationshipType);

                    relationList = Activator.CreateInstance(listType) as IList;
                    metadata.Property.SetMethod.Invoke(resource, new object[] { relationList });
                }
                else
                {
                    relationList = prop.GetMethod.Invoke(resource, null) as IList;
                }
                //Add relation to relations list
                relationList.Add(relation);
                resource.AllRelations.Add(relation);

                reader = copy;

                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    reader.Read();
                    break;
                }
            }
        }

        protected static IEnumerable<RelationshipMetadata> GetRelationshipsMetadata(Type typeToConvert)
        {
            if (_relationshipDataCache.ContainsKey(typeToConvert))
            {
                return _relationshipDataCache[typeToConvert];
            }

            IEnumerable<PropertyInfo> relationships = typeToConvert
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.PropertyType.IsAssignableTo(typeof(IEnumerable<ResourceDto>))
                            && x.IsDefined(typeof(RelationshipAttribute)));

            var result = new List<RelationshipMetadata>();
            foreach (PropertyInfo relationship in relationships)
            {
                IEnumerable<RelationshipAttribute> attributes = relationship.GetCustomAttributes<RelationshipAttribute>();
                Type returnType = relationship.PropertyType.GetGenericArguments().FirstOrDefault();

                foreach (RelationshipAttribute attribute in attributes)
                {
                    result.Add(new RelationshipMetadata(attribute.Relationship, relationship, returnType));
                }
            }

            _relationshipDataCache.Add(typeToConvert, result);

            return result;
        }

        protected virtual void HandleUncommonToken(ref Utf8JsonReader reader, TResource resource, JsonSerializerOptions options)
        {
            throw new JsonException("Unknown Property: " + reader.GetString());
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(TResource) == typeToConvert;
        }

        public override TResource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                TResource result = new TResource();
                while (reader.Read())
                {
                    if(reader.TokenType == JsonTokenType.EndObject)
                    {
                        //if (reader.IsFinalBlock == false)
                        //{
                        //    reader.Read();
                        //}
                        break;
                    }

                    //Reading current property name
                    string propertyName = reader.GetString();
                    if (options.PropertyNameCaseInsensitive)
                    {
                        propertyName = propertyName.ToLower();
                    }

                    switch (propertyName)
                    {

                        case PropertyNameId:
                            reader.Read();
                            result.Id = reader.GetGuid();
                            break;

                        case PropertyNameType:
                            reader.Read();
                            result.Type = reader.GetString();
                            break;

                        case PropertyNameAttributes:
                            {
                                reader.Read();
                                Type resourceType = typeof(TResource)
                                    .GetCustomAttribute<HasAttributesOtTypeAttribute>().AttributesType;

                                Utf8JsonReader copy = reader;

                                BaseResourceAttributes attributes = JsonSerializer.Deserialize(
                                    ref copy,
                                    resourceType,
                                    options) as BaseResourceAttributes;
                                (result as IResourceDtoWithAttributes).Attributes = attributes;

                                reader = copy;
                            }
                            break;

                        case PropertyNameRelationships:
                            {
                                reader.Read();
                                if (reader.TokenType != JsonTokenType.StartArray)
                                {
                                    throw new JsonException("Unpredicted relationships beginning. Must be StartArray");
                                }
                                ReadAndApplyRelationships(ref reader, result, options);
                            }
                            break;

                        default:
                            HandleUncommonToken(ref reader, result, options);
                            break;
                    }
                }

                return result;

            }
            throw new JsonException("Cannot read " + typeof(TResource).Name);
        }

        public override void Write(Utf8JsonWriter writer, TResource value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        protected class RelationshipMetadata
        {
            public Type RelationshipType { get; }
            public string RelationshipName { get; }
            public PropertyInfo Property { get; set; }

            public RelationshipMetadata(string key, PropertyInfo member, Type type)
            {
                RelationshipName = key;
                Property = member;
                RelationshipType = type;
            }
        }
    }
}
