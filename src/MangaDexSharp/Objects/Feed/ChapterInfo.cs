using System;
using System.Collections.Generic;

using MangaDexSharp.Resources;

namespace MangaDexSharp.Objects.Feed
{
    /// <summary>
    /// Provides information about <seealso cref="Resources.Chapter"/> element in feed
    /// </summary>
    public class ChapterInfo
    {
        /// <summary>
        /// Chapter reference
        /// </summary>
        public Chapter Chapter { get; }

        /// <summary>
        /// Translated language code
        /// </summary>
        public string Language => Chapter.TranslatedLanguage;

        /// <summary>
        /// If <seealso cref="Chapter"/> marked as read for <seealso cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <remarks>null if <seealso cref="MangaDexClient.CurrentUser"/> is null (not logged in)</remarks>
        public bool? MarkedRead { get; }

        /// <summary>
        /// Time difference between upload and now
        /// </summary>
        public DateTime TimeStamp { get; }

        /// <summary>
        /// References to <seealso cref="ScanlationGroup"/>
        /// </summary>
        public IReadOnlyCollection<ScanlationGroup> TranslatedGroups { get; }

        /// <summary>
        /// <seealso cref="User"/> who uploaded the chapter
        /// </summary>
        public User? UploadedUser { get; }

        internal ChapterInfo(
           Chapter chapter,
           bool? markRead = null)
        {
            Chapter = chapter;

            chapter.TryGetRelationCollection(chapter.RelatedGroupIds, out List<ScanlationGroup> groups);
            TranslatedGroups = groups;

            if (chapter.RelatedUserId.HasValue)
            {
                chapter.TryGetRelation(chapter.RelatedUserId.Value, out User? user);
                UploadedUser = user;
            }

            TimeStamp = Chapter.CreatedAt;
            MarkedRead = markRead;
        }

        internal ChapterInfo(
            Chapter chapter,
            IReadOnlyCollection<ScanlationGroup> groups,
            User? user,
            bool? markRead = null)
        {
            Chapter = chapter;

            chapter.TryGetRelationCollection(chapter.RelatedGroupIds, out List<ScanlationGroup> relatedGroups);
            TranslatedGroups = relatedGroups;

            TranslatedGroups = groups;
            UploadedUser = user;
            TimeStamp = Chapter.CreatedAt;
            MarkedRead = markRead;
        }
    }
}
