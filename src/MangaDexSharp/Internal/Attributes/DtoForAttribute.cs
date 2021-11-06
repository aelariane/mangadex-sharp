using System;

namespace MangaDexSharp.Internal.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal class DtoForAttribute : Attribute
    {
        public Type ResourceType { get; }

        public DtoForAttribute(Type resourceType)
        {
            ResourceType = resourceType;
        }
    }
}
