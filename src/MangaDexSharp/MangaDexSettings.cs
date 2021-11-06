using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Enums;
using MangaDexSharp.Objects;
using MangaDexSharp.Resources;

namespace MangaDexSharp
{
    public class MangaDexSettings
    {

        private int _chapterListMultiplier = 3;
        private readonly HashSet<ContentRating> _contentRatings = new HashSet<ContentRating>()
        {
            ContentRating.Safe,
            ContentRating.Suggestive,
            ContentRating.Erotica
        };
        private int _fetchFollowedManga = 10;
        private int _fetchFollowedGroups = 10;
        private int _itemsPerPage = 32;
        private readonly HashSet<string> _originalLanguages = new HashSet<string>();
        private readonly HashSet<string> _translatedLanguages = new HashSet<string>();

        /// <summary>
        /// Used only in requests lists/collections of <seealso cref="Chapter"/> as multiplier for <see cref="ItemsPerPage"/>
        /// </summary>
        /// <remarks>3 by default. Range 1-5</remarks>
        public int ChapterListMultiplier
        {
            get => _chapterListMultiplier;
            set
            {
                _chapterListMultiplier = ReturnInRange(value, 1, 5);
            }
        }

        /// <summary>
        /// Returns set of allowed <seealso cref="ContentRating"/>.
        /// </summary>
        /// <remarks>Contains <seealso cref="ContentRating.Safe"/>, <seealso cref="ContentRating.Suggestive"/>, <seealso cref="ContentRating.Erotica"/> by default</remarks>
        public IReadOnlyCollection<ContentRating> ContentFilter => _contentRatings;

        /// <summary>
        /// Amount of <seealso cref="ScanlationGroup"/> requested via <seealso cref="LocalUser.GetFollowedGroups(CancellationToken)"/>
        /// </summary>
        /// <remarks>10 be default. Range 1-100</remarks>
        public int FetchFollowedGroups
        {
            get => _fetchFollowedGroups;
            set
            {
                _fetchFollowedGroups = ReturnInRange(value, 1, 100);
            }
        }

        /// <summary>
        /// Amount of <seealso cref="Manga"/> requested via <seealso cref="LocalUser.GetFollowedManga(CancellationToken)"/>
        /// </summary>
        /// <remarks>10 be default. Range 1-100</remarks>
        public int FetchFollowedManga
        {
            get => _fetchFollowedManga;
            set
            {
                _fetchFollowedManga = ReturnInRange(value, 1, 100);
            }
        }

        /// <summary>
        /// Amount of items returned in requests that return <seealso cref="IPaginatedCollection{T}"/> and <seealso cref="IFixedPaginatedCollection{T}"/>
        /// </summary>
        /// <remarks>32 by default. Range 1-100</remarks>
        public int ItemsPerPage
        {
            get => _itemsPerPage;
            set
            {
                _itemsPerPage = ReturnInRange(value, 1, 100);
            }
        }

        /// <summary>
        /// Forces to automatically filter <seealso cref="Chapter"/> where <seealso cref="Chapter.TranslatedLanguage"/> is one of these languages
        /// </summary>
        public IReadOnlyCollection<string> TranslatedLanguages => _translatedLanguages;

        /// <summary>
        /// Only includes <seealso cref="Manga"/> where <seealso cref="Manga.OriginalLanguage"/> is one of these languages 
        /// </summary>
        public IReadOnlyCollection<string> OriginalLanguages => _originalLanguages;

        /// <summary>
        /// If compressed images of <seealso cref="Chapter"/> should be used by default while in <seealso cref="ChapterReadSession"/>
        /// </summary>
        /// <remarks>Disabled (false) by default</remarks>
        public bool UseDataSaver { get; set; } = false;

        internal MangaDexSettings()
        {
        }

        private static int ReturnInRange(int value, int min, int max)
        {
            if(value < min)
            {
                return min;
            }
            else if(value > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        /// Adds provided rating to <seealso cref="ContentFilter"/>
        /// </summary>
        /// <param name="rating">Rating to allow</param>
        /// <remarks><seealso cref="Manga"/> and <seealso cref="Chapter"/> with <paramref name="rating"/> will be displayed.</remarks>
        public MangaDexSettings AddContentFilter(ContentRating rating)
        {
            if (_contentRatings.Contains(rating))
            {
                return this;
            }
            _contentRatings.Add(rating);
            return this;
        }
        
        public MangaDexSettings AddOriginalLanguage(string language)
        {
            if (_originalLanguages.Contains(language))
            {
                return this;
            }
            _originalLanguages.Add(language);
            return this;
        }

        public MangaDexSettings AddTranslatedLanguage(string language)
        {
            if (_translatedLanguages.Contains(language))
            {
                return this;
            }
            _translatedLanguages.Add(language);
            return this;
        }

        /// <summary>
        /// Removes provided rating from <seealso cref="ContentFilter"/>
        /// </summary>
        /// <param name="rating">Rating to exclude</param>
        /// <remarks><seealso cref="Manga"/> and <seealso cref="Chapter"/> with <paramref name="rating"/> will NOT be displayed.</remarks>
        public MangaDexSettings RemoveContentFilter(ContentRating rating)
        {
            if(_contentRatings.Contains(rating) == false)
            {
                return this;
            }
            _contentRatings.Remove(rating);
            return this;
        }

        public MangaDexSettings RemoveOriginalLanguage(string language)
        {
            if(_originalLanguages.Contains(language) == false)
            {
                return this;
            }
            _originalLanguages.Remove(language);
            return this;
        }

        public MangaDexSettings RemoveTranslatedLanguage(string language)
        {
            if (_translatedLanguages.Contains(language) == false)
            {
                return this;
            }
            _translatedLanguages.Remove(language);
            return this;
        }
    }
}
