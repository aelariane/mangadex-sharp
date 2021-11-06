using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Chapter;
using MangaDexSharp.Parameters.Enums;
using MangaDexSharp.Parameters.Order.Chapter;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Objects
{
    /// <summary>
    /// Represents Volume of <seealso cref="Manga"/>
    /// </summary>
    public class Volume
    {
        private WeakReference<IReadOnlyCollection<IGrouping<string, Chapter>>>? _chapters;
        private readonly MangaDexClient _client;
        private readonly int _count;
        private Guid _mangaId;

        public string Name { get; }

        internal Volume(
            MangaDexClient client,
            Guid mangaId,
            string name,
            int count)
        {
            _client = client;
            Name = name;
            _count = count;
            _mangaId = mangaId;
        }

        /// <summary>
        /// Gets <see cref="Manga"/> this volume belongs to
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<Manga> GetManga(CancellationToken cancelToken = default)
        {
            if(_client.Resources.TryRetrieve(_mangaId, out Manga? result) && result != null)
            {
                return result;
            }
            return await _client.Manga.ViewManga(_mangaId, null, cancelToken);
        }

        /// <summary>
        /// Gets chapter of volume
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>Chapters ordered by <seealso cref="Chapter.TranslatedLanguage"/></returns>
        public async Task<IReadOnlyCollection<IGrouping<string, Chapter>>> LoadChapters(CancellationToken cancelToken = default)
        {
            if(_chapters != null && _chapters.TryGetTarget(out IReadOnlyCollection<IGrouping<string, Chapter>>? result) && result != null)
            {
                return result;
            }

            var orderOptions = new GetChapterListOrderParameters()
            {
                Chapter = OrderByType.Ascending
            };
            var parameters = new GetChapterListParameters(orderOptions);
            parameters.ApplySettings(_client.Settings);

            parameters.Amount = _count;
            parameters.Volumes = new string[] { Name };
            parameters.Manga = _mangaId;

            ResourceCollection<Chapter> chapters = await _client.Chapter.GetList(parameters, cancelToken);

            var grouped = chapters.GroupBy(x => x.TranslatedLanguage).ToArray();
            _chapters = new WeakReference<IReadOnlyCollection<IGrouping<string, Chapter>>>(grouped);

            return grouped;
        }
    }
}
