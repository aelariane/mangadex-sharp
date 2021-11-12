using System;
using System.Linq;
using System.Collections.Generic;

namespace MangaDexSharp
{
    /// <summary>
    /// Represents base class for common used MangaDex resources (entities)
    /// </summary>
    public class MangaDexResource
    {
        private HashSet<MangaDexResource>? _relatedResources;

        internal MangaDexClient Client { get; }

        /// <summary>
        /// Id of resource
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Version of resource
        /// </summary>
        public int Version { get; internal set; } = 1;

        internal MangaDexResource(MangaDexClient client, Guid id)
        {
            Id = id;
            Client = client;
        }

        internal virtual void RegisterRelation(MangaDexResource other)
        {
            _relatedResources ??= new HashSet<MangaDexResource>();
            if(_relatedResources.Contains(other) == false)
            {
                _relatedResources.Add(other);
            }
        }

        internal bool TryGetRelation<TResource>(Guid resourceId, out TResource? resource)
            where TResource : MangaDexResource
        {
            if (_relatedResources != null && _relatedResources.Count > 0)
            {
                MangaDexResource? result = _relatedResources.FirstOrDefault(x =>
                   x.Id == resourceId
                   && (x.GetType() == typeof(TResource) || x.GetType().IsSubclassOf(typeof(TResource))));

                if (result != null)
                {
                    resource = (TResource)result;
                    return true;
                }
            }

            //To reduce amount of requests
            if (Client.Resources.TryRetrieve(resourceId, out resource) && resource != null)
            {
                RegisterRelation(resource);
                return true;
            }

            resource = null;
            return false;
        }


        /// <summary>
        /// Attempts to get collection of related resources
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="relationIds"></param>
        /// <param name="resources"></param>
        /// <returns>true if count of given ids matches count of found related resources. false otherwise, even if some relations were found</returns>
        internal bool TryGetRelationCollection<TResource>(IReadOnlyCollection<Guid> relationIds, out List<TResource> resources)
            where TResource : MangaDexResource
        {
            if(_relatedResources != null && _relatedResources.Count > 0)
            {
                IEnumerable<TResource> sorted = _relatedResources
                    .Where(x => x is TResource)
                    .Select(x => (TResource)x);

                resources = new List<TResource>();

                foreach(Guid id in relationIds)
                {
                    TResource? relation = sorted.FirstOrDefault(x => x.Id == id);

                    if(relation == null)
                    {
                        if (Client.Resources.TryRetrieve(id, out relation) && relation != null)
                        {
                            RegisterRelation(relation);
                            resources.Add(relation);
                        }
                        continue;
                    }
                    resources.Add(relation);
                }

                return relationIds.Count == resources.Count;

            }

            resources = new List<TResource>();
            return false;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not MangaDexResource toCompare)
            {
                return false;
            }
            else if (GetType() != toCompare.GetType()
                    && !GetType().IsSubclassOf(toCompare.GetType())
                    && !toCompare.GetType().IsSubclassOf(GetType()))
            {
                return false;
            }

            return Id.Equals(toCompare.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(MangaDexResource? source, MangaDexResource? toCompare)
        {
            if (source is null && toCompare is null)
            {
                return true;
            }
            else if(source is null || toCompare is null)
            {
                return false;
            }

            return source.Equals(toCompare);
        }

        public static bool operator !=(MangaDexResource? source, MangaDexResource? toCompare)
        {
            return (source == toCompare) == false;
        }
        public override string ToString()
        {
            return $"{GetType().Name} (Id: {Id})";
        }
    }
}
