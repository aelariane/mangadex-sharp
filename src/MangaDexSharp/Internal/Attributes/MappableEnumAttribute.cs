using System;

namespace MangaDexSharp.Internal.Attributes
{
    [AttributeUsage(AttributeTargets.Enum)]
    internal class MappableEnumAttribute : Attribute
    {
        public string MappedName { get; }

        public MappableEnumAttribute(string mapName)
        {
            MappedName = mapName;
        }
    }
}
