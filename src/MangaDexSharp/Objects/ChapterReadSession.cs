using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Resources;

namespace MangaDexSharp.Objects
{
    public sealed class ChapterReadSession : IDisposable
    {
        private string _baseUrl;
        private Chapter _chapter;
        private MangaDexClient _client;
        private bool _dataSaver;
        private int _pageIndex = 0;
        private List<ChapterPage> _pages;
        private bool _port443WasForced;

        public ChapterPage CurrentPage => _pages[_pageIndex];

        public int CurrentPageNumber => _pageIndex + 1;

        public bool IsClosed { get; private set; }

        public bool MarkAsReadOnClose { get; set; } = true;

        public DateTime OpenedAt { get; }

        public TimeSpan RemainedTime => DateTime.Now - OpenedAt;

        public int TotalPages { get; }

        internal ChapterReadSession(string baseUrl, Chapter chapter, bool dataSaverMode, bool forcePort443)
        {
            _baseUrl = baseUrl;
            _chapter = chapter;
            _dataSaver = dataSaverMode;

            var data = chapter.Data.ToArray();
            var dataSaver = chapter.DataSaver.ToArray();

            _pages = new List<ChapterPage>();
            for(int i = 0; i < data.Length; i++)
            {
                _pages.Add(new ChapterPage(
                    //_client,
                    baseUrl,
                    chapter.Hash,
                    new string[] { data[i].ToString(), dataSaver[i].ToString() },
                    dataSaverMode));
            }

            TotalPages = data.Length;
            _client = chapter.Client;
            _port443WasForced = forcePort443;
            OpenedAt = DateTime.Now;
        }

        public async Task Close(CancellationToken cancelToken = default)
        {
            if (MarkAsReadOnClose)
            {
                await _client.Chapter.MarkChapterRead(_chapter.Id, cancelToken);
            }
            IsClosed = true;
        }

        public void Dispose()
        {
            if (_client.IsLoggedIn
                && MarkAsReadOnClose
                && !IsClosed)
            {
                Close().Start();
            }
        }

        public ChapterPage JumpToPage(int page)
        {
            if(page <= 0)
            {
                page = 1;
            }
            else if(page > TotalPages)
            {
                page = TotalPages;
            }

            _pageIndex = page - 1;
            return CurrentPage;
        }

        public ChapterPage NextPage()
        {
            if (IsClosed)
            {
                return CurrentPage;
            }
            else if (CurrentPageNumber >= TotalPages)
            {
                return CurrentPage;
            }

            _pageIndex++;
            return _pages[_pageIndex];
        }

        public ChapterPage PreviousPage()
        {
            if (IsClosed)
            {
                return CurrentPage;
            }
            else if(_pageIndex == 0)
            {
                return CurrentPage;
            }

            _pageIndex--;
            return CurrentPage;
        }
        public async Task<ChapterReadSession> Renew(CancellationToken cancelToken = default)
        {
            return await _chapter.StartReadingSession(_dataSaver, _port443WasForced, cancelToken);
        }
    }
}
