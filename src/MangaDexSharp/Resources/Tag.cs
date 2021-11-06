using System;

using MangaDexSharp.Enums;
using MangaDexSharp.Objects;

namespace MangaDexSharp.Resources
{
    /// <summary>
    /// Represents Tag of <seealso cref="Manga"/>
    /// </summary>
    public class Tag : MangaDexResource
    {
        /// <summary>
        /// Group of Tag
        /// </summary>
        public TagGroup Group { get; }

        /// <summary>
        /// Name of Tag
        /// </summary>
        public LocalizedString Name { get; }

        /// <summary>
        /// Description of Tag
        /// </summary>
        public LocalizedString Description { get; }

        internal Tag(
            MangaDexClient client,
            Guid id,
            TagGroup group,
            LocalizedString name,
            LocalizedString description)
            : base(client, id)
        {
            Group = group;
            Name = name;
            Description = description;
        }
    }
}
