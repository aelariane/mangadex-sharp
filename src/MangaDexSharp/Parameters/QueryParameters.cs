using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Parameters
{
    /// <summary>
    /// Base class for query parameters
    /// </summary>
    public abstract class QueryParameters : IQueryParameters
    {
        /// <summary>
        /// Applies mangadex reference expansions
        /// </summary>
        /// <remarks>Learn more here: https://api.mangadex.org/docs.html#section/Reference-Expansion </remarks>
        public IncludeParameters? Includes { get; set; }

        private string GetMappableEnumValue(object obj, Type enumType)
        {
            IEnumerable<FieldInfo> namedMembers = enumType
                           .GetFields(BindingFlags.Public | BindingFlags.Static)
                           .Where(x => x.GetCustomAttribute<EnumFieldStringValueAttribute>() != null);

            foreach (FieldInfo enumField in namedMembers)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (enumField.GetValue(obj).Equals(obj))
                {
                    return enumField.GetCustomAttribute<EnumFieldStringValueAttribute>().Value;
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            throw new InvalidOperationException($"Cannot find string map for value {obj} of enum {enumType.Name}");
        }

        /// <inheritdoc/>
        public virtual string? ToQueryString()
        {

            IEnumerable<PropertyInfo> props = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<QueryParameterNameAttribute>() != null);
            StringBuilder queryBuilder = new StringBuilder();

            if(Includes != null)
            {
                string? includeString = Includes.ToQueryString();
                if(includeString != null)
                {
                    queryBuilder.Append(includeString);
                }
            }

            foreach (var property in props)
            {
                if (property.GetMethod == null)
                {
                    continue;
                }

                object? value = property.GetMethod.Invoke(this, null);
                if (value == null)
                {
                    continue;
                }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                string propertyName = property.GetCustomAttribute<QueryParameterNameAttribute>().QueryName;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                if(queryBuilder.Length > 0)
                {
                    queryBuilder.Append('&');
                }

                if (value.GetType().IsAssignableTo(typeof(ICollection)))
                {
                    propertyName += "[]";
                    IEnumerable collectionValue = (ICollection)value;
                    int counter = 0;
                    foreach(object val in collectionValue)
                    {
                        if(counter > 0)
                        {
                            queryBuilder.Append('&');
                        }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        string valueString = val.ToString();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                        if (val.GetType().IsEnum
                            && val.GetType().GetCustomAttribute<MappableEnumAttribute>() != null)
                        {
                            valueString = GetMappableEnumValue(val, val.GetType());
                        }

                        queryBuilder.Append(propertyName + "=" + valueString);
                        counter++;
                    }
                }
                else if (value.GetType().IsEnum 
                         && value.GetType().GetCustomAttribute<MappableEnumAttribute>() != null)
                {

                    queryBuilder.Append(propertyName + "=" + GetMappableEnumValue(value, value.GetType()));
                }
                else if(value is DateTime time)
                {
                    queryBuilder.Append(propertyName + "=" + time.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
                }
                else if (value is bool boolValue)
                {
                    queryBuilder.Append(propertyName + "=");
                    queryBuilder.Append(boolValue ? "1" : "0");
                }
                else
                {
                    queryBuilder.Append(propertyName + "=" + value.ToString());
                }
            }

            if(queryBuilder.Length == 0)
            {
                return null;
            }
            return queryBuilder.ToString();
        }
    }
}
