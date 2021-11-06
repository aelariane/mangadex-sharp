using System;

namespace MangaDexSharp.Internal.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    internal class EnumFieldStringValueAttribute : Attribute
    {
        public string Value { get; }

        public EnumFieldStringValueAttribute(string value) 
        {
            Value = value;
         }
    }
}
