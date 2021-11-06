using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

using MangaDexSharp.Constants;

namespace MangaDexSharp.Objects
{
    /// <summary>
    /// Represents localized string object (Contains texts translated to several languages)
    /// </summary>
    public class LocalizedString : Dictionary<string, string>
    {
        /// <summary>
        /// Returns value of first entry
        /// </summary>
        /// <remarks>Returns <seealso="null"> if empty</remarks>
        [JsonIgnore]
        public string Default
        {
            get
            {
                //TODO:
                if(Count == 0)
                {
                    return string.Empty;
                }
                return First.Value;
            }
        }

        /// <summary>
        /// Returns language key of first entry
        /// </summary>
        /// <remarks>Returns <seealso="null"> if empty</remarks>
        [JsonIgnore]
        public string? DefaultLanguageKey
        {
            get
            {
                if(Count == 0)
                {
                    return "none";
                }
                return First.Key;
            }
        }

        /// <summary>
        /// Attempts to return content for English language(en)
        /// </summary>
        [JsonIgnore]
        public string? English => this[LanguageKeys.English];

        /// <summary>
        /// Attempts to return content for English language(en). If it does not exist, returns <see cref="Default"/>
        /// </summary>
        [JsonIgnore]
        public string EnglishOrDefault
        {
            get
            {
                if (ContainsKey(LanguageKeys.English))
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return English;
#pragma warning restore CS8603 // Possible null reference return.
                }
                return Default;
            }
        }

        [JsonIgnore]
        public KeyValuePair<string, string> First
        {
            get
            {
                return this.First();
            }
        }

        internal LocalizedString() : base()
        {
        }

        internal LocalizedString(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }

        [JsonIgnore]
        /// <summary>
        /// Returns all availible languages of <see cref="LocalizedString"/> instance
        /// </summary>
        public IEnumerable<string> Languages => Keys;

        [JsonIgnore]
        public bool HasEnglish => ContainsKey(LanguageKeys.English);

        /// <summary>
        /// If current <see cref="LocalizedString"/> object has language availible
        /// </summary>
        /// <param name="lang">Language to check</param>
        /// <returns>If language is availible in this instance</returns>
        public bool HasLanguage(string lang) => ContainsKey(lang);

        public override string ToString()
        {
            if(Count == 0)
            {
                return string.Empty;
            }
            else if (HasEnglish)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return English;
#pragma warning restore CS8603 // Possible null reference return.
            }
            return First.Value ?? string.Empty;
        }

        public static implicit operator string(LocalizedString locString)
        {
            return locString?.ToString() ?? string.Empty;
        }
    }
}
