using System;
using Xunit;

using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Enums;

namespace MangaDexSharp.Tests.Parameters
{
    public class QueryParametersTests
    {
        [Fact]
        public void BooleanPropertyDisplaysCorrectly_True()
        {
            var parameters = new QueryTestParameters();
            parameters.TestBoolean = true;

            string query = parameters.ToQueryString();

            Assert.Equal("propertyTestBoolean=1", query);
        }

        
        [Fact]
        public void BooleanPropertyDisplaysCorrectly_False()
        {
            var parameters = new QueryTestParameters();
            parameters.TestBoolean = false;

            string query = parameters.ToQueryString();

            Assert.Equal("propertyTestBoolean=0", query);
        }

        
        [Fact]
        public void IntegerPropertyDisplaysCorrectly()
        {
            var parameters = new QueryTestParameters();
            parameters.TestInteger = 32;

            string query = parameters.ToQueryString();

            Assert.Equal("propertyTestInt=32", query);
        }

        [Fact]
        public void PropertyDisplaysCorrectly()
        {
            var parameters = new QueryTestParameters();
            parameters.TestProperty = "testValue";

            string query = parameters.ToQueryString();

            Assert.Equal("propertyTest=testValue", query);
        }

        [Fact]
        public void CollectionDisplaysCorrectly_SingleValue()
        {
            var parameters = new QueryTestParameters();
            parameters.TestCollection = new string[]
            {
                "value1"
            };

            string query = parameters.ToQueryString();

            Assert.Equal("collectionTest[]=value1", query);
        }
        

        [Fact]
        public void CollectionDisplaysCorrectly_MultipleValues()
        {
            var parameters = new QueryTestParameters();
            parameters.TestCollection = new string[]
            {
                "value1",
                "valueSecond"
            };

            string query = parameters.ToQueryString();

            Assert.Equal("collectionTest[]=value1&collectionTest[]=valueSecond", query);
        }

        [Fact]
        public void EnumDisplaysCorrectly()
        {
            var parameters = new QueryTestParameters();
            parameters.TestEnumProperty = TestEnum.Second;

            string query = parameters.ToQueryString();

            Assert.Equal("enumPropertyTest=two", query);
        }

        [Fact]
        public void EnumCollectionDisplaysCorrectly_SingleValue()
        {
            var parameters = new QueryTestParameters();
            parameters.TestEnumCollection = new TestEnum[]
            {
                TestEnum.Third
            };

            string query = parameters.ToQueryString();

            Assert.Equal("enumCollectionTest[]=three", query);
        }

        [Fact]
        public void EnumCollectionDisplaysCorrectly_MultipleValues()
        {
            var parameters = new QueryTestParameters();
            parameters.TestEnumCollection = new TestEnum[]
            {
                TestEnum.First,
                TestEnum.Second
            };

            string query = parameters.ToQueryString();

            Assert.Equal("enumCollectionTest[]=one&enumCollectionTest[]=two", query);
        }

        [Fact]
        public void DateTimeDisplaysCorrectly()
        {
            var parameters = new QueryTestParameters();
            parameters.DateTimeTest = new DateTime(2019, 11, 9, 12, 32, 10);
            //"yyyy'-'MM'-'dd'T'HH':'mm':'ss"
            string query = parameters.ToQueryString();

            Assert.Equal("timeTest=2019-11-09T12:32:10", query);
        }

        [Fact]
        public void MultipleValuesDisplaysCorrectly()
        {
            var parameters = new QueryTestParameters();
            parameters.TestProperty = "te1";
            parameters.TestEnumProperty = TestEnum.Second;

            string query = parameters.ToQueryString();

            Assert.Equal("propertyTest=te1&enumPropertyTest=two", query);
        }

        [Fact]
        public void GuidDisplaysCorrectly()
        {
            var parameters = new QueryTestParameters();
            parameters.Id = new Guid("25aaabb1-9f74-4469-a8d6-1eac5924cc79");

            string query = parameters.ToQueryString();

            Assert.Equal("guidTest=25aaabb1-9f74-4469-a8d6-1eac5924cc79", query);
        }

        [Fact]
        public void CombinedWithIncludesDisplayCorrectly()
        {
            var parameters = new QueryTestParameters();
            parameters.TestProperty = "testValue";
            parameters.Includes = new IncludeParameters()
            {
                IncludeArtist = true
            };

            string query = parameters.ToQueryString();

            Assert.Equal("includes[]=artist&propertyTest=testValue", query);
        }

        [Fact]
        public void CombinedWithMultipleIncludesDisplayCorrectly()
        {
            var parameters = new QueryTestParameters();
            parameters.TestProperty = "testValue";
            parameters.Includes = new IncludeParameters()
            {
                IncludeArtist = true,
                IncludeAuthor = true
            };

            string query = parameters.ToQueryString();

            Assert.Equal("includes[]=artist&includes[]=author&propertyTest=testValue", query);
        }

        

        [Fact]
        public void CombinedWithOrderDisplayCorrectly()
        {
            var parameters = new QueryTestParameters(new OrderTestParameters()
            {
                FirstParameter = OrderByType.Ascending
            });
            parameters.TestProperty = "testValue";
            parameters.Amount = 15;

            string query = parameters.ToQueryString();

            Assert.Equal("propertyTest=testValue&limit=15&order[firstParameter]=asc", query);
        }
    }
}