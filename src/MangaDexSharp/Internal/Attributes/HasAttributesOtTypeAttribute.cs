using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDexSharp.Internal.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal class HasAttributesOtTypeAttribute : Attribute
    {
        public Type AttributesType { get; }
        public HasAttributesOtTypeAttribute(Type attributeType)
        {
            AttributesType = attributeType;
        }
    }
}
