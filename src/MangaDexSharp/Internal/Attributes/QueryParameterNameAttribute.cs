using System;

namespace MangaDexSharp.Internal.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal class QueryParameterNameAttribute : Attribute
    {
        public string QueryName { get; }

        public QueryParameterNameAttribute(string queryName)
        {
            QueryName = queryName;
        }
    }
}
