#nullable disable
using System.Collections.Generic;

using MangaDexSharp.Objects;

namespace MangaDexSharp.Internal.Dto.ResourceAttributes
{
    internal class ScanlationGroupAttributes : AuditableResourceAttributes
    {
        public IEnumerable<LocalizedString> AltNames { get; set; }
        public string ContactEmail { get; set; }
        public string Description { get; set; }
        public string Discord { get; set; }
        public IEnumerable<string> FocusedLanguage { get; set; }
        public string IrcChannel { get; set; }
        public string IrcServer { get; set; }
        public bool Locked { get; set; }
        public string Name { get; set; }
        public bool Official { get; set; }
        public string Website { get; set; }
    }
}
