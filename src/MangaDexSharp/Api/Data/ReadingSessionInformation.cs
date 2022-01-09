using MangaDexSharp.Resources;
using System;

namespace MangaDexSharp.Api.Data
{
    /// <summary>
    /// Response for GetMangaDex@Home operation
    /// </summary>
    /// <remarks>Read more here: https://api.mangadex.org/docs.html#operation/get-at-home-server-chapterId</remarks>
    public class ReadingSessionInformation
    {
        /// <summary> 
        /// The base URL to construct final image URLs from.The URL returned is valid for the requested chapter only, and for a duration of 15 minutes from the time of the response.
        /// </summary>
        public string BaseUrl { get; }

        /// <summary>
        /// Information for reading chapter
        /// </summary>
        public ChapterReadingMetadata Chapter { get; }

        /// <summary>
        /// Chapter the session was requested for
        /// </summary>
        public Chapter SourceChapter { get; }

        public ReadingSessionInformation(
            string baseUrl,
            ChapterReadingMetadata chapter,
            Chapter sourceChapter)
        {
            BaseUrl = baseUrl;
            Chapter = chapter;
            SourceChapter = sourceChapter;
        }
    }
}
