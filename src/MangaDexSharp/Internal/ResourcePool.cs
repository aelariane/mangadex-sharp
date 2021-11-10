using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Caching.Memory;

using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Internal.ResourceFactories;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Internal
{
    internal class ResourcePool
    {
        private Dictionary<Type, Dictionary<Guid, WeakReference>> _references = new Dictionary<Type, Dictionary<Guid, WeakReference>>();
        private Dictionary<Type, IMemoryCache> _cache = new Dictionary<Type, IMemoryCache>();
        private Dictionary<Type, IResourceFactory> _factories;
        internal MangaDexClient Client { get; set; }

        public bool CachingEnabled { get; set; } = true;
        public double CacheLifetime { get; set; } = 300000;

        internal ResourcePool(MangaDexClient client)
        {
            Client = client;
            _factories = new Dictionary<Type, IResourceFactory>();
            _factories.Add(typeof(Manga), new MangaFactory(Client, this));
            _factories.Add(typeof(Chapter), new ChapterFactory(Client));
            _factories.Add(typeof(Author), new AuthorFactory(Client));
            _factories.Add(typeof(Tag), new TagFactory(Client));
            _factories.Add(typeof(User), new UserFactory(Client));
            _factories.Add(typeof(CoverArt), new CoverArtFactory(Client));
            _factories.Add(typeof(ScanlationGroup), new ScanlationGroupFactory(Client));
            _factories.Add(typeof(CustomList), new CustomListFactory(Client));
        }

        private IMemoryCache GetCacheByType(Type resourceType)
        {
            bool success = _cache.TryGetValue(resourceType, out IMemoryCache? result);
            if(success && result != null)
            {
                return result;
            }

            var memoryCacheOptions = new MemoryCacheOptions()
            {
                ExpirationScanFrequency = TimeSpan.FromSeconds(CachingEnabled ? 60 : 15)
            };
            var memoryCacheIOptions = Microsoft.Extensions.Options.Options.Create(memoryCacheOptions);

            result = new MemoryCache(memoryCacheIOptions);
            _cache.Add(resourceType, result);
            return result;
        }

        private void UpdateReferenceEntry(Type resourceType, MangaDexResource resource)
        {
            if (_references.TryGetValue(resourceType, out Dictionary<Guid, WeakReference>? referenceDict))
            {
                if(referenceDict.TryGetValue(resource.Id, out WeakReference? weakRef) && weakRef != null && weakRef.IsAlive)
                {
                    return;
                }
                if (referenceDict.ContainsKey(resource.Id))
                {
                    referenceDict[resource.Id] = new WeakReference(resource);
                    return;
                }
                referenceDict.Add(resource.Id, new WeakReference(resource));
            }
            else
            {
                referenceDict = new Dictionary<Guid, WeakReference>(16);
                _references.Add(resourceType, referenceDict);
                referenceDict.Add(resource.Id, new WeakReference(resource));
            }
        }

        private void UpdateReferenceEntry<TResource>(TResource resource)
            where TResource : MangaDexResource
        {
            UpdateReferenceEntry(typeof(TResource), resource);
        }

        private bool TryGetByReference<TResource>(Guid id, out TResource? resource)
            where TResource : MangaDexResource
        {
            if (TryGetByReference(typeof(TResource), id, out MangaDexResource? resourceBase) && resourceBase != null)
            {
                resource = (TResource)resourceBase;
                return true;
            }
            resource = null;
            return false;
        }

        private bool TryGetByReference(Type resourceType, Guid id, out MangaDexResource? resource)
        {
            if (_references.TryGetValue(resourceType, out Dictionary<Guid, WeakReference>? referenceDict))
            {
                if (referenceDict.TryGetValue(id, out WeakReference? resultReference))
                {
                    if (resultReference.IsAlive && resultReference.Target?.GetType() == resourceType)
                    {
                        resource = (MangaDexResource) resultReference.Target;
                        return true;
                    }
                    referenceDict.Remove(id);
                }
            }
            else
            {
                _references.Add(resourceType, new Dictionary<Guid, WeakReference>(16));
                resource = null;
                return false;
            }
            resource = null;
            return false;
        }

        public bool TryRetrieve<TResource>(Guid resourceId, out TResource? resource)
            where TResource : MangaDexResource
        {
            if(TryGetByReference(resourceId, out resource))
            {
                return true;
            }

            IMemoryCache cache = GetCacheByType(typeof(TResource));

            if (cache.TryGetValue(resourceId, out object result))
            {
                resource = (TResource) result;
                UpdateReferenceEntry(resource);
                return true;
            }

            resource = null;
            return false;
        }

        public bool TryRetrieve<TResource>(ResourceDto dto, out TResource? resource)
            where TResource : MangaDexResource
        {
            if(TryRetrieve(dto, typeof(TResource), out MangaDexResource? dexResource) && dexResource != null)
            {
                resource = (TResource) dexResource;
                UpdateReferenceEntry(resource);
                return true;
            }
            resource = null;
            return false;
        }

        public bool TryRetrieve(ResourceDto dto, Type resourceType, out MangaDexResource? resource)
        {
            bool byRef = TryGetByReference(resourceType, dto.Id, out resource);

            if(byRef == false)
            {
                IMemoryCache cache = GetCacheByType(resourceType);

                if (cache.TryGetValue(dto.Id, out object resultObject))
                {
                    resource = (MangaDexResource)resultObject;
                    UpdateReferenceEntry(resourceType, resource);
                }
                else
                {
                    IResourceDtoWithAttributes resourceWithAttributes = dto;
                    if (resourceWithAttributes.Attributes == null)
                    {
                        resource = null;
                        return false;
                    }

                    if (_factories.TryGetValue(resourceType, out IResourceFactory? factory) == false)
                    {
                        throw new NotImplementedException("Unknown " + nameof(IResourceFactory) + " request, type: " + resourceType.Name);
                    }

                    resource = factory.Create(dto);
                    UpdateReferenceEntry(resource.GetType(), resource);

                    using ICacheEntry entry = cache.CreateEntry(resource.Id);
                    entry.Value = resource;
                    if (CachingEnabled)
                    {
                        entry.SetAbsoluteExpiration(DateTime.Now.Add(TimeSpan.FromMilliseconds(CacheLifetime)));
                    }
                    else
                    {
                        entry.SetAbsoluteExpiration(DateTime.Now.Add(TimeSpan.FromSeconds(10)));
                    }
                }
            }
#pragma warning disable CS8604 // Possible null reference argument.
            _factories[resourceType].Sync(resource, dto);
#pragma warning restore CS8604 // Possible null reference argument.

            //Adding or updating relations for alreay existing resource
            if (dto.AllRelations.Any())
            {
                foreach (ResourceDto relation in dto.AllRelations)
                {
                    DtoForAttribute? dtoAttribute = relation.GetType().GetCustomAttribute<DtoForAttribute>();
                    if (dtoAttribute == null)
                    {
                        continue;
                    }
                    if (TryRetrieve(relation, dtoAttribute.ResourceType, out MangaDexResource? relations))
                    {
                        resource.RegisterRelation(resource);
                    }
                }
            }

            return true;
        }

        public bool TryRetrieveCollection<TResource>(IEnumerable<Guid> ids, out List<TResource> resources)
            where TResource : MangaDexResource
        {
            List<TResource> result = new List<TResource>();
            resources = result;

            foreach(Guid id in ids)
            {
                if(TryRetrieve(id, out TResource? resource) && resource != null)
                {
                    result.Add(resource);
                }
            }

            return result.Count > 0;
        }
    }
}
