using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Enums
{
    /// <summary>
    /// Used to indicate relation of <seealso cref="Manga"/>
    /// </summary>
    [MappableEnum("related")]
    public enum MangaRelation
    {
        [EnumFieldStringValue("monochrome")] Monochrome,
        [EnumFieldStringValue("main_story")] MainStory,
        [EnumFieldStringValue("adapted_from")] AdaptedFrom,
        [EnumFieldStringValue("based_on")] BasedOn,
        [EnumFieldStringValue("prequel")] Prequel,
        [EnumFieldStringValue("side_story")] SideStory,
        [EnumFieldStringValue("doujinshi")]  Doujinshi,
        [EnumFieldStringValue("same_franchise")] SameFranchise,
        [EnumFieldStringValue("shared_universe")] SharedUniverse,
        [EnumFieldStringValue("sequel")] Sequel,
        [EnumFieldStringValue("spin_off")] SpinOff,
        [EnumFieldStringValue("alternate_story")] AlternateStory,
        [EnumFieldStringValue("alternate_version")] AlternateVersion,
        [EnumFieldStringValue("preserialization")] Preserialization,
        [EnumFieldStringValue("colored")] Colored,
        [EnumFieldStringValue("serialization")] Serialization
    }
}
