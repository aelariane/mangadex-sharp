using System;

namespace MangaDexSharp.Internal.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class IncludeNameAttribute : Attribute
    {
        public string IncludeName { get; }

        public IncludeNameAttribute(string includeName)
        {
            IncludeName = includeName;
        }
    }
}
