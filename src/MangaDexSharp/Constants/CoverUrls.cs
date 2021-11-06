using System;

using MangaDexSharp.Resources;

namespace MangaDexSharp.Constants
{
    /// <summary>
    /// Contains constant urls to retrieve <seealso cref="CoverArt"/> image of <seealso cref="Manga"/>
    /// </summary>
    /// <remarks>Learn more here: https://api.mangadex.org/docs.html#section/Retrieving-Covers-from-the-API </remarks>
    public class CoverUrls
    {
        public const string Source = "https://uploads.mangadex.org/covers/{0}/{1}";
        public const string Thumbnail256Px = "https://uploads.mangadex.org/covers/{0}/{1}.256.jpg";
        public const string Thumbnail512Px = "https://uploads.mangadex.org/covers/{0}/{1}.512.jpg";
    }
}
