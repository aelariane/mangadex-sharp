using System;

namespace MangaDexSharp.Constants
{
    /// <summary>
    /// Contains set of restrictions for some query parameters
    /// </summary>
    public class ListQueryRestrictions
    {
        public const int AmountDefault = 10;
        public const int AmountMinumumPossibleValue = 1;
        public const int AmountMaximumPossibleValueDefault = 100;

        public const int PositionMinimumPossibleValue = 0;
        public const int PositionMaximumPossibleIndex = 10000;

        public const int MangaFeedDefaultAmount = 100;
        public const int MangaFeedMaximumAmount = 500;

        public const int UserFollowedFeedDefaultAmount = 100;
        public const int UserFollowedFeedMaximumAmount = 500;
    }
}
