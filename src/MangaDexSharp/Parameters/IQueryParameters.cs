using System;

namespace MangaDexSharp.Parameters
{
    /// <summary>
    /// Represents parameters that can be applied to url query
    /// </summary>
    public interface IQueryParameters
    {
        /// <summary>
        /// Gets query string of parameters instace
        /// </summary>
        /// <returns>Parameters serialized to query string</returns>
        string? ToQueryString();
    }
}
