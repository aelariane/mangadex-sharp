using System;

using MangaDexSharp.Constants;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Parameters.Order;

namespace MangaDexSharp.Parameters
{
    /// <summary>
    /// Base class for requests that return collection
    /// </summary>
    public abstract class ListQueryParameters : QueryParameters
    {
        private int? _offset;
        private int? _amount;

        /// <summary>
        /// Amount of items that will be fetched
        /// </summary>
        /// <remarks>See <see cref="ListQueryParameters"/> for more restrictions for specific query parameters</remarks>
        /// <exception cref="InvalidOperationException"></exception>
        [QueryParameterName("limit")]
        public int? Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if(value < ListQueryRestrictions.AmountMinumumPossibleValue)
                {
                    throw new InvalidOperationException($"{nameof(value)} cannot be lower then {ListQueryRestrictions.AmountMinumumPossibleValue}");
                }
                if(value > MaximumAmount)
                {
                    value = MaximumAmount;
                }
                else if(Position + value > ListQueryRestrictions.PositionMaximumPossibleIndex) 
                {
                    throw new InvalidOperationException(
                        $"Cannot retrieve items from positions more then {ListQueryRestrictions.PositionMaximumPossibleIndex}." +
                        $"Setting Amount resulted to {Position + value}");
                }

                _amount = value;
            }
        }

        /// <summary>
        /// Maximum allowed amount for the type of request
        /// </summary>
        public readonly int MaximumAmount;

        /// <summary>
        /// Offset position
        /// </summary>
        /// <remarks>See <see cref="ListQueryParameters"/> for more restrictions for specific query parameters</remarks>
        /// <exception cref="InvalidOperationException"></exception>
        [QueryParameterName("offset")]
        public int? Position
        {
            get
            {
                return _offset;
            }
            set
            {
                if (value < ListQueryRestrictions.PositionMinimumPossibleValue)
                {
                    throw new InvalidOperationException($"{nameof(value)} cannot be lower then {ListQueryRestrictions.PositionMinimumPossibleValue}");
                }
                if (value + Amount > ListQueryRestrictions.PositionMaximumPossibleIndex)
                {
                    value = ListQueryRestrictions.PositionMaximumPossibleIndex - Amount;
                }

                _offset = value;
            }
        }

        /// <summary>
        /// Order parameters
        /// </summary>
        public OrderParameters? Order {  get; protected set; }

        protected ListQueryParameters() : this(ListQueryRestrictions.AmountMaximumPossibleValueDefault)
        {
        }

        protected ListQueryParameters(int maximumAmount)
        {
            MaximumAmount = maximumAmount;
        }

        /// <inheritdoc/>
        public override string? ToQueryString()
        {
            string? query = base.ToQueryString();

            if(Order == null)
            {
                return query;
            }

            string? orderQuery = Order.ToQueryString();
            if(orderQuery == null)
            {
                return query;
            }
            else if(query == null)
            {
                return orderQuery;
            }

            if(query.Length > 0)
            {
                query += "&";
            }
            query += orderQuery;

            return query;
        }
    }
}
