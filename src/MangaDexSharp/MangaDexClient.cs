using System;
using System.Net.Http;
using System.Threading.Tasks;

using MangaDexSharp.Api;
using MangaDexSharp.Internal;
using MangaDexSharp.Resources;

namespace MangaDexSharp
{
    public class MangaDexClient : IDisposable
    {
        internal UserCredentials? Credentials;
        internal readonly HttpClient HttpClient;
        internal ResourcePool Resources { get; }

        /// <summary>
        /// Exposes at-home api
        /// </summary>
        public AtHomeApi AtHome { get; }

        /// <summary>
        /// Exposes auth related api
        /// </summary>
        public AuthApi Auth { get; }

        /// <summary>
        /// Exposes Author related api
        /// </summary>
        public AuthorApi Author { get; }

        /// <summary>
        /// Defines settings which will be used in entity calls
        /// </summary>
        public MangaDexSettings Settings { get; } = new MangaDexSettings();

        /// <summary>
        /// Lifetime of objects in cache in milliseconds (0 if cache disabled)
        /// </summary>
        public double CacheLifetimeMilliseconds
        {
            get
            {
                if (Resources.CachingEnabled == false)
                {
                    return 0;
                }
                return Resources.CacheLifetime;
            }
            set
            {
                if (Resources.CachingEnabled == false)
                {
                    return;
                }
                Resources.CacheLifetime = value;
            }
        }

        /// <summary>
        /// Exposes chapter related api
        /// </summary>
        public ChapterApi Chapter { get; }

        /// <summary>
        /// Exposes cover art related api
        /// </summary>
        public CoverApi Cover { get; }

        /// <summary>
        /// Current logged in User
        /// </summary>
        /// <remarks>null if not logged in</remarks>
        public LocalUser? CurrentUser { get; internal set; }

        /// <summary>
        /// Exposes follows related api
        /// </summary>
        public FollowsApi Follows { get; }

        /// <summary>
        /// If client instance has logged in User
        /// </summary>
        public bool IsLoggedIn => CurrentUser != null;

        /// <summary>
        /// Exposes custom list related api
        /// </summary>
        public CustomListApi List { get; }

        /// <summary>
        /// Exposes manga related api
        /// </summary>
        public MangaApi Manga { get; }

        /// <summary>
        /// Exposes scanlation group related api
        /// </summary>
        public ScanlationGroupApi ScanlationGroup { get; }

        /// <summary>
        /// If internal cache is used.
        /// </summary>
        /// <remarks>
        /// If enabled, all objects received directly or via relationships caches in memory
        /// If request which uses id finds object by id in cache, it returns it instead of doing request for this object
        /// </remarks>
        public bool UseCache
        {
            get
            {
                return Resources.CachingEnabled;
            }
            set
            {
                Resources.CachingEnabled = value;
            }
        }

        /// <summary>
        /// Exposes User related api
        /// </summary>
        public UserApi User { get; }

        /// <summary>
        /// Creates new instance of <see cref="MangaDexClient"/>
        /// </summary>
        public MangaDexClient() : this(new HttpClient())
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="MangaDexClient"/>
        /// </summary>
        /// <param name="client">Client, provided externally, to avoid socket exhaustion</param>
        public MangaDexClient(HttpClient client)
        {
            HttpClient = client;
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36");

            Resources = new ResourcePool(this);

            AtHome = new AtHomeApi(this);
            Auth = new AuthApi(this);
            Author = new AuthorApi(this);
            Chapter = new ChapterApi(this);
            Follows = new FollowsApi(this);
            Cover = new CoverApi(this);
            List = new CustomListApi(this);
            Manga = new MangaApi(this);
            ScanlationGroup = new ScanlationGroupApi(this);
            User = new UserApi(this);
        }

        /// <summary>
        /// Creates new instance of <see cref="MangaDexClient"/> with user credentials set by default
        /// </summary>
        /// <param name="client">Client, provided externally, to avoid socket exhaustion</param>
        /// <param name="credentials">User's credentials</param>
        public MangaDexClient(HttpClient client, UserCredentials credentials) : this(client)
        {
            this.Credentials = credentials;
        }

        /// <summary>
        /// Resets current user credentials
        /// </summary>
        public void CleanCredentials()
        {
            if (IsLoggedIn)
            {
                throw new InvalidOperationException("Cannot reset credentials while in user session. " 
                    + "Make sure to logout before cleaning credentials");
            }
            Credentials = null;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsLoggedIn) 
            {
                Task.Run(Auth.Logout);
            }
        }
        
        /// <summary>
        /// Sets user credentials
        /// </summary>
        /// <param name="credentials">Credentials to set</param>
        /// <remarks>Credentials must be cleaned before setting new ones</remarks>
        public void SetUserCredentials(UserCredentials credentials)
        {
            if(Credentials != null)
            {
                throw new InvalidOperationException("Credentials are already initialized. Make sure you used " 
                    + nameof(CleanCredentials) 
                    + " before setting new credentials"); 
            }
            else if (IsLoggedIn)
            {
                throw new InvalidOperationException("Cannot reset credentials while in user session."
                    + "Make sure to logout before changing user credentials");
            }

            Credentials = credentials;
        }
    }
}
