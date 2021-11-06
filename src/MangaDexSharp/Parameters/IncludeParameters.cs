using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MangaDexSharp.Constants;
using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Parameters
{
    /// <summary>
    /// Parameters for applying for mangadex reference expansion
    /// </summary>
    /// <remarks>Learn more here: https://api.mangadex.org/docs.html#section/Reference-Expansion </remarks>
    public class IncludeParameters : IQueryParameters
    {
        /// <summary>
        /// Include artists
        /// </summary>
        [IncludeName(RelationshipNames.Artist)]
        public bool IncludeArtist { get; set; }
        
        /// <summary>
        /// Include authors
        /// </summary>
        [IncludeName(RelationshipNames.Author)]
        public bool IncludeAuthor { get; set; }

        /// <summary>
        /// Include cover art
        /// </summary>
        [IncludeName(RelationshipNames.CovertArt)]
        public bool IncludeCover { get; set; }

        /// <summary>
        /// Include leader of scanlation group
        /// </summary>
        [IncludeName(RelationshipNames.GroupLeader)]
        public bool IncludeLeader { get; set; }

        /// <summary>
        /// Include related mangas
        /// </summary>
        [IncludeName(RelationshipNames.Manga)]
        public bool IncludeManga { get; set; }

        /// <summary>
        /// Include members of scanlation group
        /// </summary>
        [IncludeName(RelationshipNames.GroupMember)]
        public bool IncludeMembers { get; set; }

        /// <summary>
        /// Include scanlation group
        /// </summary>
        [IncludeName(RelationshipNames.ScanlationGroup)]
        public bool IncludeScanlationGroup { get; set; }

        /// <summary>
        /// Include users
        /// </summary>
        [IncludeName(RelationshipNames.User)]
        public bool IncludeUser { get; set; }

        public IncludeParameters()
        {
        }

        /// <inheritdoc/>
        public string? ToQueryString()
        {
            StringBuilder includesQuery = new StringBuilder();
            IEnumerable<PropertyInfo> includeProps = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttributes(typeof(IncludeNameAttribute)) != null
                            && x.PropertyType == typeof(bool)
                            && x.GetMethod?.Invoke(this, null) as bool? == true);

            foreach(PropertyInfo includeProperty in includeProps)
            {
                if(includesQuery.Length > 0)
                {
                    includesQuery.Append('&');
                }
                includesQuery.Append("includes[]=");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                includesQuery.Append(includeProperty.GetCustomAttribute<IncludeNameAttribute>().IncludeName);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            if(includesQuery.Length == 0)
            {
                return null;
            }
            return includesQuery.ToString();
        }
    }
}
