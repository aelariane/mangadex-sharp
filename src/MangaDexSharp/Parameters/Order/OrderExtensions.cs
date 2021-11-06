using System;
using System.Linq.Expressions;
using System.Reflection;

using MangaDexSharp.Parameters.Enums;

namespace MangaDexSharp.Parameters.Order
{
    /// <summary>
    /// Set of extensions related to <seealso cref="OrderParameters"/>
    /// </summary>
    public static class OrderExtensions
    {
        //TODO Exception exctioption
        private static PropertyInfo VerifyProperty(MemberInfo member)
        {
            PropertyInfo? info = member as PropertyInfo;
            if (info == null)
            {
                throw new Exception();
            }
            else if (info.PropertyType != typeof(OrderByType) && info.PropertyType != typeof(OrderByType?))
            {
                throw new Exception();
            }
            //else if (info.SetMethod == null)
            //{
            //    throw new Exception();
            //}

            return info;
        }

        /// <summary>
        /// Set property of <seealso cref="OrderParameters"/> as <seealso cref="OrderByType.Ascending"/>
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="options"></param>
        /// <param name="expression">Peoperty to set</param>
        /// <returns>Updated instance of order parameters</returns>
        public static TOptions OrderByAscending<TOptions>(this TOptions options, Expression<Func<TOptions, object>> expression)
           where TOptions : OrderParameters
        {
            UnaryExpression? unaryExpression = expression.Body as UnaryExpression;

            if (unaryExpression == null)
            {
                throw new Exception();
            }
            MemberExpression? expr = unaryExpression.Operand as MemberExpression;
            if(expr == null)
            {
                throw new Exception();
            }

            PropertyInfo propertyToSet = VerifyProperty(expr.Member);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            propertyToSet.SetMethod.Invoke(options, new object[] { OrderByType.Ascending });
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return options;
        }

        /// <summary>
        /// Set property of <seealso cref="OrderParameters"/> as <seealso cref="OrderByType.Descending"/>
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="options"></param>
        /// <param name="expression">Peoperty to set</param>
        /// <returns>Updated instance of order parameters</returns>
        public static TOptions OrderByDescending<TOptions>(this TOptions options, Expression<Func<TOptions, object>> expression)
           where TOptions : OrderParameters
        {
            UnaryExpression? unaryExpression = expression.Body as UnaryExpression;

            if (unaryExpression == null)
            {
                throw new Exception();
            }
            MemberExpression? expr = unaryExpression.Operand as MemberExpression;
            if (expr == null)
            {
                throw new Exception();
            }

            PropertyInfo propertyToSet = VerifyProperty(expr.Member);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            propertyToSet.SetMethod.Invoke(options, new object[] { OrderByType.Descending});
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return options;
        }
    }
}
