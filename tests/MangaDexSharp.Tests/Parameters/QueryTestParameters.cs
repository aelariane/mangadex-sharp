using System;
using System.Collections.Generic;

using MangaDexSharp.Parameters;
using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Tests.Parameters
{
    public class QueryTestParameters : ListQueryParameters
    {
        [QueryParameterName("propertyTestBoolean")]
        public bool? TestBoolean { get; set; }

        [QueryParameterName("propertyTestInt")]
        public int? TestInteger { get; set;}

        [QueryParameterName("propertyTest")]
        public string TestProperty { get; set; }

        [QueryParameterName("collectionTest")]
        public ICollection<string> TestCollection { get; set; }

        [QueryParameterName("enumPropertyTest")]
        public TestEnum? TestEnumProperty { get; set; }

        [QueryParameterName("enumCollectionTest")]
        public ICollection<TestEnum> TestEnumCollection { get; set; }

        [QueryParameterName("timeTest")]
        public DateTime? DateTimeTest { get; set; }
        
        [QueryParameterName("guidTest")]
        public Guid? Id { get; set; }

        public QueryTestParameters(OrderTestParameters parameters = null)
        {
            base.Order = parameters;
        }
    }

    [MappableEnum("enumTest")]
    public enum TestEnum
    {
        [EnumFieldStringValue("one")]
        First,
        [EnumFieldStringValue("two")]
        Second,
        [EnumFieldStringValue("three")]
        Third
    }
}