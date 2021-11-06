using MangaDexSharp.Constants;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Objects
{
    /// <summary>
    /// Container of links for <see cref="MangaDexSharp.Resources.Manga"/>
    /// </summary>
    public class MangaLinks
    {
        public Manga Manga { get; }

        internal MangaLinks(Manga manga)
        {
            Manga = manga;
        }

        public string? Anilist
        {
            get
            {
                if (Manga.LinksDictionary.TryGetValue(MangaLinksKeys.Anilist, out string? id) == false)
                {
                    return null;
                }
                return string.Format("https://anilist.co/manga/{0}", id);
            }
        }

        public string? AnimePlanet
        {
            get
            {
                if (Manga.LinksDictionary.TryGetValue(MangaLinksKeys.AnimePlanet, out string? slug) == false)
                {
                    return null;
                }
                return string.Format("https://www.anime-planet.com/manga/{0}", slug);
            }
        }

        public string? BookwalkerJp
        {
            get
            {
                if (Manga.LinksDictionary.TryGetValue(MangaLinksKeys.BookwalkerJp, out string? slug) == false)
                {
                    return null;
                }
                return string.Format("https://bookwalker.jp/{0}", slug);
            }
        }

        public string? MangaUpdates
        {
            get
            {
                if (Manga.LinksDictionary.TryGetValue(MangaLinksKeys.MangaUpdates, out string? id) == false)
                {
                    return null;
                }
                return string.Format("https://www.mangaupdates.com/series.html?id={0}", id);
            }
        }

        public string? NovelUpdates
        {

            get
            {
                if (Manga.LinksDictionary.TryGetValue(MangaLinksKeys.NovelUpdates, out string? slug) == false)
                {
                    return null;
                }
                return string.Format("https://www.novelupdates.com/series/{0}", slug);
            }
        }

        public string? KitsuIo
        {
            get
            {
                if (Manga.LinksDictionary.TryGetValue(MangaLinksKeys.KitsuIo, out string? val) == false)
                {
                    return null;
                }
                if (int.TryParse(val, out int id))
                {
                    return string.Format("https://kitsu.io/api/edge/manga/{0}", id);
                }
                return string.Format("https://kitsu.io/api/edge/manga?filter[slug]={0}", val);
            }
        }

        public string? Amazon => GetRawUrl(MangaLinksKeys.Amazon);

        public string? EBookJapan => GetRawUrl(MangaLinksKeys.EBookJapan);

        public string? MyAnimeList
        {
            get
            {
                if (Manga.LinksDictionary.TryGetValue(MangaLinksKeys.MyAnimeList, out string? id) == false)
                {
                    return null;
                }
                return string.Format("https://myanimelist.net/manga/{0}", id);
            }
        }

        public string? Raw => GetRawUrl(MangaLinksKeys.Raw);

        public string? EnglishTranslation => GetRawUrl(MangaLinksKeys.EngTranslation);

        private string? GetRawUrl(string key)
        {
            if(Manga.LinksDictionary.TryGetValue(key, out string? result))
            {
                return result;
            }
            return null;
        }
    }
}
