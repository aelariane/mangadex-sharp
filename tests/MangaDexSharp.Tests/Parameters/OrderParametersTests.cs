using System;
using Xunit;

using MangaDexSharp.Parameters.Enums;
using MangaDexSharp.Parameters.Order;

namespace MangaDexSharp.Tests.Parameters
{
    public class OrderParametersTests
    {
        [Fact]
        public void ExtensionsCanSetAscending()
        {
            var order = new OrderTestParameters();

            order.OrderByAscending(x => x.FirstParameter);

            Assert.Equal(OrderByType.Ascending, order.FirstParameter);
        }
        
        [Fact]
        public void ExtensionsCanSetDescending()
        {
            var order = new OrderTestParameters();

            order.OrderByDescending(x => x.FirstParameter);

            Assert.Equal(OrderByType.Descending, order.FirstParameter);
        }

        [Fact]
        public void OrderByAscendingFirstDisplaysCorrectly()
        {
            var order = new OrderTestParameters();
            order.FirstParameter = OrderByType.Ascending;

            string query = order.ToQueryString();

            Assert.Equal("order[firstParameter]=asc", query);
        }
        
        [Fact]
        public void OrderByAscendingSecondDisplaysCorrectly()
        {
            var order = new OrderTestParameters();
            order.SecondParameter = OrderByType.Ascending;

            string query = order.ToQueryString();

            Assert.Equal("order[secondParameter]=asc", query);
        }
        
        [Fact]
        public void OrderByDescendingFirstDisplaysCorrectly()
        {
            var order = new OrderTestParameters();
            order.FirstParameter = OrderByType.Descending;

            string query = order.ToQueryString();

            Assert.Equal("order[firstParameter]=desc", query);
        }
        
        [Fact]
        public void OrderByDescendingSecondDisplaysCorrectly()
        {
            var order = new OrderTestParameters();
            order.SecondParameter = OrderByType.Descending;

            string query = order.ToQueryString();

            Assert.Equal("order[secondParameter]=desc", query);
        }
        
        [Fact]
        public void OrderByTwoDifferentValuesDisplayCorrectly()
        {
            var order = new OrderTestParameters();
            order.FirstParameter = OrderByType.Ascending;
            order.SecondParameter = OrderByType.Descending;

            string query = order.ToQueryString();

            Assert.Equal("order[firstParameter]=asc&order[secondParameter]=desc", query);
        }
    }
}