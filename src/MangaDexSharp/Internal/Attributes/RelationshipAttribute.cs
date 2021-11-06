using System;

namespace MangaDexSharp.Internal.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal class RelationshipAttribute : Attribute
    {
        public string Relationship { get; set; }

        public RelationshipAttribute(string relationship)
        {
            Relationship = relationship;
        }
    }
}
