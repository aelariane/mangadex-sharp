using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using MangaDexSharp.Enums;
using MangaDexSharp.Internal.Attributes;

namespace MangaDexSharp.Parameters
{
    /// <summary>
    /// Extensions related to <seealso cref="IQueryParameters"/> and <seealso cref="QueryParameters"/>
    /// </summary>
    public static class ParametersExtensions
    {
        private static readonly Dictionary<Type, List<PropertyInfo>> _originalLanguagePropertiesCache = new Dictionary<Type, List<PropertyInfo>>();
        private static readonly Dictionary<Type, List<PropertyInfo>> _contentRatingPropertiesCache = new Dictionary<Type, List<PropertyInfo>>();
        private static readonly Dictionary<Type, List<PropertyInfo>> _translatedLanguagePropertiesCache = new Dictionary<Type, List<PropertyInfo>>();

        private static readonly string[] _contentRatingNames = new string[]
        {
            "contentRating"
        };

        private static readonly string[] _originalLanguageNames = new string[]
        {
            "originalLanguage"
        };

        //List of properties where to add languages
        private static readonly string[] _languageAddNames = new string[]
        {
            "availableTranslatedLanguage",
            "translatedLanguage"
        };

        /// <summary>
        /// Adds <see cref="MangaDexSettings.ContentFilter"/> to requests where filters can be applied, if possible
        /// </summary>
        /// <typeparam name="TParameters">Input parameters type</typeparam>
        /// <param name="parameters">Parameters</param>
        /// <param name="settings">Settings to take filters from</param>
        /// <returns>Modified parameters with applied languages filters</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TParameters AddContentFilters<TParameters>(
            this TParameters parameters,
            MangaDexSettings settings)
            where TParameters : QueryParameters
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            else if(settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            else if (settings.ContentFilter.Count == 0)
            {
                return parameters;
            }

            if (_contentRatingPropertiesCache.TryGetValue(typeof(TParameters), out List<PropertyInfo>? props) && props != null)
            {
                ICollection<ContentRating> collection = new List<ContentRating>(settings.ContentFilter);
                var args = new object[] { collection };
                foreach (var prop in props)
                {
                    prop.SetMethod?.Invoke(parameters, args);
                }
                return parameters;
            }

            IEnumerable<PropertyInfo> properties = typeof(TParameters)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<QueryParameterNameAttribute>() != null);

            List<PropertyInfo> toCache = new List<PropertyInfo>();
            foreach (PropertyInfo property in properties)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                string propertyName = property.GetCustomAttribute<QueryParameterNameAttribute>().QueryName;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                if (_contentRatingNames.Contains(propertyName))
                {
                    toCache.Add(property);
                }
            }
            _contentRatingPropertiesCache.Add(typeof(TParameters), toCache);

            return parameters.AddContentFilters(settings);
        }

        /// <summary>
        /// Adds <see cref="MangaDexSettings.OriginalLanguages"/> to requests where language filters can be applied, if possible
        /// </summary>
        /// <typeparam name="TParameters">Input parameters type</typeparam>
        /// <param name="parameters">Parameters</param>
        /// <param name="settings">Settings to take languages from</param>
        /// <returns>Modified parameters with applied languages filters</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TParameters AddOriginalLanguages<TParameters>(
            this TParameters parameters,
            MangaDexSettings settings)
            where TParameters : QueryParameters
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            else if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            else if (settings.OriginalLanguages.Count == 0)
            {
                return parameters;
            }

            if (_originalLanguagePropertiesCache.TryGetValue(typeof(TParameters), out List<PropertyInfo>? props) && props != null)
            {
                ICollection<string> collection = new List<string>(settings.OriginalLanguages);
                var args = new object[] { collection };
                foreach (var prop in props)
                {
                    prop.SetMethod?.Invoke(parameters, args);
                }
                return parameters;
            }

            IEnumerable<PropertyInfo> properties = typeof(TParameters)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<QueryParameterNameAttribute>() != null);

            List<PropertyInfo> toCache = new List<PropertyInfo>();
            foreach (PropertyInfo property in properties)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                string propertyName = property.GetCustomAttribute<QueryParameterNameAttribute>().QueryName;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                if (_originalLanguageNames.Contains(propertyName))
                {
                    toCache.Add(property);
                }
            }
            _originalLanguagePropertiesCache.Add(typeof(TParameters), toCache);

            return parameters.AddOriginalLanguages(settings);
        }

        /// <summary>
        /// Adds <see cref="MangaDexSettings.TranslatedLanguages"/> to requests where language filters can be applied, if possible
        /// </summary>
        /// <typeparam name="TParameters">Input parameters type</typeparam>
        /// <param name="parameters">Parameters</param>
        /// <param name="settings">Settings to take languages from</param>
        /// <returns>Modified parameters with applied languages filters</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TParameters AddTranslatedLanguages<TParameters>(
            this TParameters parameters,
            MangaDexSettings settings)
            where TParameters : QueryParameters
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            else if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            else if (settings.TranslatedLanguages.Count == 0)
            {
                return parameters;
            }

            if (_translatedLanguagePropertiesCache.TryGetValue(typeof(TParameters), out List<PropertyInfo>? props) && props != null)
            {
                ICollection<string> collection = new List<string>(settings.TranslatedLanguages);

                var args = new object[] { collection };

                foreach (var prop in props)
                {
                    prop.SetMethod?.Invoke(parameters, args);
                }

                return parameters;
            }

            IEnumerable<PropertyInfo> properties = typeof(TParameters)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<QueryParameterNameAttribute>() != null);

            List<PropertyInfo> toCache = new List<PropertyInfo>();
            foreach (PropertyInfo property in properties)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                string propertyName = property.GetCustomAttribute<QueryParameterNameAttribute>().QueryName;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                if (_languageAddNames.Contains(propertyName))
                {
                    toCache.Add(property);
                }
            }
            _translatedLanguagePropertiesCache.Add(typeof(TParameters), toCache);

            return parameters.AddTranslatedLanguages(settings);
        }

        /// <summary>
        /// Applies <seealso cref="MangaDexSettings"/> to the parameters
        /// </summary>
        /// <typeparam name="TParameters"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static TParameters ApplySettings<TParameters>(
           this TParameters parameters,
           MangaDexSettings settings)
           where TParameters : QueryParameters
        {
            parameters.AddContentFilters(settings);
            parameters.AddTranslatedLanguages(settings);
            parameters.AddOriginalLanguages(settings);
            return parameters;
        }

        /// <summary>
        /// Builds query based on base url
        /// </summary>
        /// <param name="url">Base url</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns>Built query with applied parameters</returns>
        public static string BuildQuery(this string url, IQueryParameters? parameters)
        {
            if (parameters == null)
            {
                return url;
            }

            string? queryString = parameters.ToQueryString();
            if (queryString == null)
            {
                return url;
            }
            if (url.EndsWith('/'))
            {
                return url.Remove(url.Length - 1, 1) + "?" + queryString;
            }
            return url + "?" + queryString;
        }

        /// <summary>
        /// Creates new instance of <typeparamref name="TOptions"/> and automatically applies <seealso cref="MangaDexClient.Settings"/>
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="client"></param>
        /// <returns></returns>
        public static TOptions CreateParameters<TOptions>(this MangaDexClient client)
            where TOptions : QueryParameters, new()
        {
            var result = new TOptions();
            result.ApplySettings(client.Settings);
            return result;
        }
    }
}
