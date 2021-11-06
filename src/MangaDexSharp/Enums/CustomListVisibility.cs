using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Enums
{
    /// <summary>
    /// Visibility of <seealso cref="Resources.CustomList"/>
    /// </summary>
    [MappableEnum("visibility")]
    public enum CustomListVisibility
    {
        /// <summary>
        /// List is private and only availible for <seealso cref="Resources.User"/> who owns it
        /// </summary>
        [EnumFieldStringValue("private")] Private,
        /// <summary>
        /// List is public and visible for anyone
        /// </summary>
        [EnumFieldStringValue("public")] Public
    }
}
