using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Collections.Internal;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Parameters;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Objects.Feed
{
    /// <summary>
    /// Represents Feed of <seealso cref="Resources.Manga"/> containing <seealso cref="Chapter"/> grouped by volumues
    /// </summary>
    public class ChapterFeed : IPaginatedCollection<ChapterFeedElement>, IFixedPaginatedCollection<Chapter>
    {
        private readonly string _baseUrl;
        private readonly PaginatedCollection<ChapterDto, Chapter> _chapterCollection;
        private readonly MangaDexClient _client;
        private CollectionPage<ChapterFeedElement>? _currentPage;
        private readonly Dictionary<int, WeakReference<CollectionPage<ChapterFeedElement>>> _pages = new Dictionary<int, WeakReference<CollectionPage<ChapterFeedElement>>>();
        private readonly ListQueryParameters _queryParameters;
        private IReadOnlyCollection<Guid>? _readChapterIds;

        /// <inheritdoc/>
        public CollectionPage<ChapterFeedElement> CurrentPage => _currentPage
            ?? throw new InvalidOperationException($"{nameof(InitializeFirstPage)} should be called before requesting {nameof(CurrentPage)}");


        public Manga Manga { get; }

        /// <inheritdoc>/>
        public int Page => _chapterCollection.Page;

        /// <inheritdoc/>
        public int TotalPages => _chapterCollection.TotalPages;

        int IFixedPaginatedCollection<Chapter>.ItemsPerPage => _chapterCollection.ItemsPerPage;

        CollectionPage<Chapter> IPaginatedCollection<Chapter>.CurrentPage => _chapterCollection.CurrentPage;

        int IPaginatedCollection<Chapter>.Page => _chapterCollection.Page;

        int IPaginatedCollection<Chapter>.TotalPages => _chapterCollection.TotalPages;

        internal ChapterFeed(
            MangaDexClient client,
            Manga manga,
            ResourceCollection<Chapter> initialCollection,
            string baseUrl,
            ListQueryParameters parameters)
        {
            if (parameters.Includes == null)
            {
                throw new InvalidOperationException($"{nameof(ChapterFeed)} is meant to be used with includes");
            }
            else if (parameters.Includes.IncludeScanlationGroup == false
                || parameters.Includes.IncludeUser == false)
            {
                throw new InvalidOperationException($"{nameof(ChapterFeed)} should be used with all included: scanlation_group, user");
            }
            _client = client;

            _chapterCollection = new PaginatedCollection<ChapterDto, Chapter>(
                client,
                initialCollection,
                baseUrl,
                parameters,
                initialCollection.Total,
                _client.IsLoggedIn);

            _queryParameters = parameters;
            _baseUrl = baseUrl;
            Manga = manga;
        }

        private async Task LoadReadMarkers(CancellationToken cancelToken)
        {
            if (_client.IsLoggedIn == false)
            {
                _readChapterIds = null;
                return;
            }
            _readChapterIds = await _client.Manga.GetReadMarkersOfManga(Manga.Id, cancelToken);
        }

        private CollectionPage<ChapterFeedElement> CreateCurrentPage()
        {
            if (_pages.TryGetValue(Page, out WeakReference<CollectionPage<ChapterFeedElement>>? reference))
            {
                if (reference != null && reference.TryGetTarget(out CollectionPage<ChapterFeedElement>? cachedResult))
                {
                    return cachedResult;
                }
            }

            CollectionPage<Chapter> chapters = _chapterCollection.CurrentPage;

            List<ChapterFeedElement> feedElements = chapters
                .GroupBy(x => x.Volume)
                .Select(volumeGroup =>
                {
                    var grouped = volumeGroup
                        .GroupBy(x => x.ChapterName)
                        .Select(chapterGroup =>
                        {
                            var infos = chapterGroup
                                .Select(chapter =>
                                {
                                    bool? markRead = null;
                                    if (_readChapterIds != null)
                                    {
                                        markRead = _readChapterIds.Contains(chapter.Id);
                                    }
                                    return new ChapterInfo(
                                        chapter,
                                        markRead);
                                });
                            return new KeyValuePair<string?, IEnumerable<ChapterInfo>>(chapterGroup.Key, infos);
                        });
                    return new ChapterFeedElement(volumeGroup.Key, grouped);
                })
                .ToList();

            var result = new CollectionPage<ChapterFeedElement>(feedElements, Page);
            reference = new WeakReference<CollectionPage<ChapterFeedElement>>(result);

            if (_pages.ContainsKey(Page))
            {
                _pages[Page] = reference;
            }
            else
            {
                _pages.Add(Page, reference);
            }

            return result;
        }
        internal async Task InitializeFirstPage(CancellationToken cancelToken = default)
        {
            await LoadReadMarkers(cancelToken);
            _currentPage = CreateCurrentPage();
        }

        /// <inheritdoc/>
        public async Task<CollectionPage<ChapterFeedElement>> NavigateTo(int page, CancellationToken cancelToken = default)
        {
            await _chapterCollection.NavigateTo(page, cancelToken);
            await LoadReadMarkers(cancelToken);

            return CreateCurrentPage();
        }

        /// <inheritdoc/>
        public async Task<CollectionPage<ChapterFeedElement>> NextPage(CancellationToken cancelToken = default)
        {
            await _chapterCollection.NextPage(cancelToken);
            await LoadReadMarkers(cancelToken);

            return CreateCurrentPage();
        }

        /// <inheritdoc/>
        public async Task<CollectionPage<ChapterFeedElement>> PreviousPage(CancellationToken cancelToken = default)
        {
            await _chapterCollection.PreviousPage(cancelToken);
            await LoadReadMarkers(cancelToken);

            return CreateCurrentPage();
        }

        async Task<CollectionPage<Chapter>> IPaginatedCollection<Chapter>.NavigateTo(int page, CancellationToken cancelToken)
        {
            return await _chapterCollection.NavigateTo(page, cancelToken);
        }

        async Task<CollectionPage<Chapter>> IPaginatedCollection<Chapter>.NextPage(CancellationToken cancelToken)
        {
            return await _chapterCollection.NextPage(cancelToken);
        }

        async Task<CollectionPage<Chapter>> IPaginatedCollection<Chapter>.PreviousPage(CancellationToken cancelToken)
        {
            return await _chapterCollection.PreviousPage(cancelToken);
        }
    }
}
