using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using MangaDexSharp.Parameters.Enums;

namespace MangaDexSharp.Parameters.Order
{
    /// <summary>
    /// Base class for order object of <seealso cref="ListQueryParameters"/>
    /// </summary>
    public abstract class OrderParameters : IQueryParameters
    {
        /// <inheritdoc/>
        public string? ToQueryString()
        {
            IEnumerable<PropertyInfo> properties = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => (p.PropertyType == typeof(OrderByType) || p.PropertyType == typeof(OrderByType?))
                            && p.GetMethod != null);

            string order = string.Empty;
            foreach (PropertyInfo prop in properties)
            {
                OrderByType? value = prop.GetMethod?.Invoke(this, null) as OrderByType?;
                if (value.HasValue)
                {
                    string stringValue = value == OrderByType.Ascending ? "asc" : "desc";
                    string propertyName = char.ToLower(prop.Name[0]) + prop.Name[1..];
                    if(order != string.Empty)
                    {
                        order += "&";
                    }
                    order += $"order[{propertyName}]={stringValue}";
                }
            }

            return order == string.Empty ? null : order;
        }
    }
}
