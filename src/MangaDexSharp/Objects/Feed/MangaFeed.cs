using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Collections.Internal;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Manga;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Objects.Feed
{
    /// <summary>
    /// Represents Feed, where chapters grouped by <seealso cref="Manga"/>, additionally with <seealso cref="CoverArt"/> provided. Also includes uploaded <seealso cref="User"/> and <seealso cref="ScanlationGroup"/>
    /// </summary>
    public class MangaFeed : IPaginatedCollection<MangaFeedElement>, IFixedPaginatedCollection<Chapter>
    {
        private readonly string _baseUrl;
        private readonly PaginatedCollection<ChapterDto, Chapter> _chapterCollection;
        private readonly MangaDexClient _client;
        private CollectionPage<MangaFeedElement>? _currentPage;
        private readonly Dictionary<int, WeakReference<CollectionPage<MangaFeedElement>>> _pages = new Dictionary<int, WeakReference<CollectionPage<MangaFeedElement>>>();
        private readonly ListQueryParameters _queryParameters;
        private IReadOnlyCollection<Guid>? _readChapterIds;

        /// <inheritdoc/>
        public CollectionPage<MangaFeedElement> CurrentPage => _currentPage 
            ?? throw new InvalidOperationException($"{nameof(InitializeFirstPage)} should be called before requesting {nameof(CurrentPage)}");

        /// <inheritdoc>/>
        public int Page => _chapterCollection.Page;

        /// <inheritdoc/>
        public int TotalPages => _chapterCollection.TotalPages;

        int IFixedPaginatedCollection<Chapter>.ItemsPerPage => _chapterCollection.ItemsPerPage;

        CollectionPage<Chapter> IPaginatedCollection<Chapter>.CurrentPage => _chapterCollection.CurrentPage;

        int IPaginatedCollection<Chapter>.Page => _chapterCollection.Page;

        int IPaginatedCollection<Chapter>.TotalPages => _chapterCollection.TotalPages;

        internal MangaFeed(
            MangaDexClient client,
            ResourceCollection<Chapter> initialCollection,
            string baseUrl,
            ListQueryParameters parameters)
        {
            if (parameters.Includes == null)
            {
                throw new InvalidOperationException($"{nameof(MangaFeed)} is mean to be used with includes");
            }
            else if (parameters.Includes.IncludeManga == false
                || parameters.Includes.IncludeScanlationGroup == false
                || parameters.Includes.IncludeUser == false)
            {
                throw new InvalidOperationException($"{nameof(MangaFeed)} should be used with all included: manga, scanlation_group, user");
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
        }

        private async Task FetchAdditionalInformation(CancellationToken cancelToken)
        {
            List<Guid> mangaIds = new List<Guid>();
            CollectionPage<Chapter> chapters = _chapterCollection.CurrentPage;

            foreach (Chapter chapter in chapters)
            {
                if (mangaIds.Contains(chapter.RelatedMangaId) == false)
                {
                    mangaIds.Add(chapter.RelatedMangaId);
                }
            }

            var mangaListParameters = new GetMangaListParameters();
            mangaListParameters.Includes = new IncludeParameters()
            {
                IncludeCover = true
            };
            mangaListParameters.Amount = mangaIds.Count;
            mangaListParameters.MangaIds = mangaIds;
            await _client.Manga.GetList(mangaListParameters, cancelToken);

            if (_client.IsLoggedIn == false)
            {
                _readChapterIds = null;
                return;
            }
            _readChapterIds = await _client.Manga.GetReadMarkers(mangaIds, cancelToken);
        }

        private CollectionPage<MangaFeedElement> CreateCurrentPage()
        {
            if(_pages.TryGetValue(Page, out WeakReference<CollectionPage<MangaFeedElement>>? reference))
            {
                if (reference != null && reference.TryGetTarget(out CollectionPage<MangaFeedElement>? cachedResult))
                {
                    return cachedResult;
                }
            }

            CollectionPage<Chapter> chapters = _chapterCollection.CurrentPage;

            List<Manga> usedManga = chapters.Select(chapter =>
                {
                    if (chapter.TryGetRelation(chapter.RelatedMangaId, out Manga? manga) && manga != null)
                    {
                        return manga;
                    }
                    throw new InvalidOperationException($"Expected to have {nameof(Manga)} in relations of {nameof(Chapter)}");
                })
                .Distinct()
                .ToList();

            List<MangaFeedElement> managFeedElements = new List<MangaFeedElement>();
            foreach (Manga manga in usedManga)
            {
                List<ChapterInfo> infos = chapters
                    .Where(ch => ch.RelatedMangaId == manga.Id)
                        .Select(chapter =>
                        {
                            bool? markRead = null;
                            if(_readChapterIds != null)
                            {
                                markRead = _readChapterIds.Contains(chapter.Id);
                            }

                            return new ChapterInfo(
                                chapter,
                                markRead);
                        })
                    .ToList();

                manga.TryGetRelation(manga.MainCoverId, out CoverArt? cover);
                var element = new MangaFeedElement(
                    manga,
                    cover,
                    infos);

                managFeedElements.Add(element);
            }

            var result = new CollectionPage<MangaFeedElement>(managFeedElements, Page);
            reference = new WeakReference<CollectionPage<MangaFeedElement>>(result);

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
            await FetchAdditionalInformation(cancelToken);
            _currentPage = CreateCurrentPage();
        }

        /// <inheritdoc/>
        public async Task<CollectionPage<MangaFeedElement>> NavigateTo(int page, CancellationToken cancelToken = default)
        {
            await _chapterCollection.NavigateTo(page, cancelToken);
            await FetchAdditionalInformation(cancelToken);

            return CreateCurrentPage();
        }

        /// <inheritdoc/>
        public async Task<CollectionPage<MangaFeedElement>> NextPage(CancellationToken cancelToken = default)
        {
            await _chapterCollection.NextPage(cancelToken);
            await FetchAdditionalInformation(cancelToken);

            return CreateCurrentPage();
        }

        /// <inheritdoc/>
        public async Task<CollectionPage<MangaFeedElement>> PreviousPage(CancellationToken cancelToken = default)
        {
            await _chapterCollection.PreviousPage(cancelToken);
            await FetchAdditionalInformation(cancelToken);

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
