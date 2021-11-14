using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MangaDexSharp.Collections;
using MangaDexSharp.Collections.Internal;
using MangaDexSharp.Enums;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Objects.Feed;
using MangaDexSharp.Parameters;
using MangaDexSharp.Parameters.Cover;
using MangaDexSharp.Parameters.CustomList;
using MangaDexSharp.Parameters.Enums;
using MangaDexSharp.Parameters.Manga;
using MangaDexSharp.Parameters.Order.CustomList;

namespace MangaDexSharp.Resources
{
    /// <summary>
    /// Represents custom list with <seealso cref="Manga"/> created by Users
    /// </summary>
    public class CustomList : MangaDexResource
    {
        internal Guid RelatedUserId { get; set; }
        internal HashSet<Guid> RelatedMangaIds { get; } = new HashSet<Guid>();

        /// <summary>
        /// If <see cref="MangaDexClient.CurrentUser"/> is owner of the list
        /// </summary>
        public bool IsOwnedByCurrentUser => RelatedUserId == Client.CurrentUser?.Id;

        /// <summary>
        /// Name of list
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Visibility of list
        /// </summary>
        public CustomListVisibility Visibility { get; private set; }

        internal CustomList(
            MangaDexClient client,
            Guid id,
            string name,
            CustomListVisibility visibility,
            int version)
            : base(client, id)
        {
            Name = name;
            Visibility = visibility;
            Version = version;
        }

        /// <summary>
        /// Gets Feed of the List
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>Feed of the List</returns>
        public async Task<MangaFeed> GetFeed(CancellationToken cancelToken = default)
        {
            var feedOrder = new GetCustomListFeedOrderParameters();
            feedOrder.CreatedAt = OrderByType.Descending;

            GetCustomListFeedParameters parameters = new GetCustomListFeedParameters(feedOrder);
            parameters.ApplySettings(Client.Settings);

            parameters.Amount = Client.Settings.ItemsPerPage * Client.Settings.ChapterListMultiplier;
            parameters.Includes = new IncludeParameters()
            {
                IncludeManga = true,
                IncludeScanlationGroup = true,
                IncludeUser = true
            };

            ResourceCollection<Chapter> initialFeed = await Client.List.GetFeed(Id, parameters, cancelToken);

            var feed = new MangaFeed(
                Client,
                initialFeed,
                Client.List.BaseApiPath + "/" + Id + "/feed",
                parameters);
            await feed.InitializeFirstPage(cancelToken);

            return feed;
        }

        /// <summary>
        /// Adds <seealso cref="Manga"/> to the List owned by <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="toAdd">Manga to add</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task AddManga(Manga toAdd, CancellationToken cancelToken = default)
        {
            if (RelatedMangaIds.Contains(toAdd.Id))
            {
                return;
            }

            await Client.List.AddMangaToList(Id, toAdd.Id, cancelToken);
            RelatedMangaIds.Add(toAdd.Id);
        }

        /// <summary>
        /// Updates <seealso cref="Name"/> of the List owned by <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="newName">New name to set</param>
        /// <param name="cancelToken"></param>
        /// <returns>Updated name</returns>
        public async Task<string> ChangeName(string newName, CancellationToken cancelToken = default)
        {
            if (newName == Name)
            {
                return Name;
            }

            CustomList updated = await Client.List.UpdateCustomListName(Id, newName, Version, cancelToken);

            Name = newName;
            Version = updated.Version;

            return newName;
        }

        /// <summary>
        /// Updates <seealso cref="Visibility"/> of the List owned by <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="newName">New visibility to set</param>
        /// <param name="cancelToken"></param>
        /// <returns>Updated visibility</returns>
        public async Task<CustomListVisibility> ChangeVisibility(CustomListVisibility newVisibility, CancellationToken cancelToken = default)
        {
            if(newVisibility == Visibility)
            {
                return Visibility;
            }

            CustomList updated = await Client.List.UpdateCustomListVisibility(Id, newVisibility, Version, cancelToken);

            Visibility = newVisibility;
            Version = updated.Version;

            return newVisibility;
        }

        /// <summary>
        /// Gets paginated collection of <seealso cref="Manga"/> in the List
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <remarks>Includes <seealso cref="CoverArt"/> for <seealso cref="Manga"/></remarks>
        /// <returns>Paginated collection of <seealso cref="Manga"/></returns>
        public async Task<IFixedPaginatedCollection<Manga>> GetTitles(CancellationToken cancelToken = default)
        {
            var parameters = new GetMangaListParameters();
            parameters.Amount = Client.Settings.ItemsPerPage;
            parameters.MangaIds = RelatedMangaIds
                .Take(Client.Settings.ItemsPerPage)
                .ToList();

            parameters.Includes = new IncludeParameters()
            {
                IncludeCover = true
            };

            var collection = await Client.Manga.GetList(parameters, cancelToken);
            return new PaginatedByIdCollection<MangaDto, Manga>(
                Client,
                collection,
                RelatedMangaIds,
                Client.Manga.BaseApiPath,
                parameters,
                false);
        }

        /// <summary>
        /// Gets <seealso cref="User"/> who is owner of the list
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns><seealso cref="User"/> who own the list</returns>
        public async Task<User> GetUser(CancellationToken cancelToken = default)
        {
            if(TryGetRelation(RelatedUserId, out User? user) && user != null)
            {
                return user;
            }

            user = await Client.User.GetUser(RelatedUserId, null, cancelToken);
            RegisterRelation(user);

            return user;
        }

        /// <summary>
        /// Removes <seealso cref="Manga"/> from the List owned by <see cref="MangaDexClient.CurrentUser"/>
        /// </summary>
        /// <param name="toRemove">Manga to remove</param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task RemoveManga(Manga toRemove, CancellationToken cancelToken = default)
        {
            if (RelatedMangaIds.Contains(toRemove.Id) == false)
            {
                return;
            }

            await Client.List.RemoveMangaFromList(Id, toRemove.Id, cancelToken);
            RelatedMangaIds.Remove(toRemove.Id);
        }
    }
}
