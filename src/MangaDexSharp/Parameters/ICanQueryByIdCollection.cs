using System;
using System.Collections.Generic;

namespace MangaDexSharp.Parameters
{
    /// <summary>
    /// Indicates that <seealso cref="IQueryParameters"/> has property <seealso cref="ICollection{Guid}"/> of <seealso cref="Guid"/> that can be used to fetch resources by Id
    /// </summary>
    public interface ICanQueryByIdCollection : IQueryParameters
    {
        /// <summary>
        /// Collection of Ids
        /// </summary>
        ICollection<Guid> Ids { get; set;  }
    }
}
