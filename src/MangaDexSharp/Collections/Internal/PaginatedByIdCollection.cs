using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Parameters;

namespace MangaDexSharp.Collections.Internal
{
    internal class PaginatedByIdCollection<TDto, TResource>
        : IFixedPaginatedCollection<TResource>
        where TDto : ResourceDto
        where TResource : MangaDexResource
    {
        private CollectionPage<TResource> _currentPage;
        private int _pageIndex;
        private Guid[] _allIds;
        private WeakReference<CollectionPage<TResource>>[] _pageReferences;
        private bool _requiresAuth;
        private string _baseAddress;
        private ICanQueryByIdCollection _queryParameters;
        private IResourceRetriever<TResource> _retriever;

        /// <inheritdoc/>
        public CollectionPage<TResource> CurrentPage => _currentPage;

        /// <inheritdoc/>
        public int Page
        {
            get
            {
                return _pageIndex + 1;
            }
            private set
            {
                _pageIndex = value - 1;
            }
        }

        /// <inheritdoc/>
        public int TotalPages => (int)Math.Ceiling((double)_allIds.Length / ItemsPerPage);

        /// <inheritdoc/>
        public int ItemsPerPage { get; }

        public PaginatedByIdCollection(
            MangaDexClient client,
            IReadOnlyCollection<TResource> initialCollection,
            IReadOnlyCollection<Guid> allIds,
            string query,
            ListQueryParameters parameters,
            bool requiresAuth = false)
        {
            if(parameters is not ICanQueryByIdCollection)
            {
                throw new ArgumentException(nameof(parameters) + $" should be {nameof(ICanQueryByIdCollection)}");
            }

            _baseAddress = query;
            _queryParameters = (ICanQueryByIdCollection) parameters;

            _pageIndex = 0;
            _allIds = allIds.ToArray();
            ItemsPerPage = parameters.Amount ?? 10;
            _pageReferences = new WeakReference<CollectionPage<TResource>>[TotalPages];
            _currentPage = new CollectionPage<TResource>(initialCollection, Page);
            _pageReferences[0] = new WeakReference<CollectionPage<TResource>>(_currentPage);

            _retriever = new SimpleResourceRetriever<TDto, TResource>(client);
            _requiresAuth = requiresAuth;
        }

        private async Task<CollectionPage<TResource>> LoadPage(int page, CancellationToken cancelToken)
        {
            int index = page - 1;
            ICollection<Guid> idsToGet = _allIds
                .Skip(index * ItemsPerPage)
                .Take(ItemsPerPage)
                .ToList();

            _queryParameters.Ids = idsToGet;
            ResourceCollection<TResource> result = await _retriever.GetAsync(
                _baseAddress,
                _queryParameters,
                _requiresAuth,
                cancelToken);

            return new CollectionPage<TResource>(result, page);

        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<CollectionPage<TResource>> NavigateTo(int page, CancellationToken cancelToken = default)
        {
            if(page <= 0 || page > TotalPages)
            {
                throw new ArgumentOutOfRangeException(nameof(page));
            }

            Page = page;
            if (_pageReferences[_pageIndex] != null 
                && _pageReferences[_pageIndex].TryGetTarget(out CollectionPage<TResource>? resultCollection))
            {
                _currentPage = resultCollection;
                return _currentPage;
            }

            _currentPage = await LoadPage(page, cancelToken);
            _pageReferences[_pageIndex] = new WeakReference<CollectionPage<TResource>>(_currentPage);
            return _currentPage;
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<CollectionPage<TResource>> NextPage(CancellationToken cancelToken = default)
        {
            if(CurrentPage.Page >= TotalPages)
            {
                throw new InvalidOperationException("Collection reached last page");
            }

            return await NavigateTo(Page + 1, cancelToken);
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<CollectionPage<TResource>> PreviousPage(CancellationToken cancelToken = default)
        {
            if (CurrentPage.Page <= 0)
            {
                throw new InvalidOperationException("Collection is at first page already");
            }

            return await NavigateTo(Page - 1, cancelToken);
        }
    }
}
