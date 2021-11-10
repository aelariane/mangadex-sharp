using System;

using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Internal.ResourceFactories
{
    internal class UserFactory : IResourceFactory
    {

        private MangaDexClient _client;

        public UserFactory(MangaDexClient client)
        {
            _client = client;
        }

        public MangaDexResource Create(ResourceDto dto)
        {
            var userDto = dto as UserDto;
            if (userDto == null)
            {
                throw new ArgumentException($"Invald dto type {dto.GetType()}. {nameof(UserDto)} expected");
            }

            UserAttributes attributes = userDto.Attributes;
            var user = new User(_client, dto.Id, attributes.Username, attributes.Roles)
            { 
                Version = attributes.Version
            };

            if(userDto.ScanlationGroupRelations != null)
            {
                foreach(ScanlationGroupDto group in userDto.ScanlationGroupRelations)
                {
                    user.RelatedGroupIds.Add(group.Id);
                }
            }

            return user;
        }

        public void Sync(MangaDexResource resource, ResourceDto dto)
        {
            User user = (User)resource;
            if(dto is UserDto userDto)
            {
                if (userDto.ScanlationGroupRelations != null)
                {
                    foreach (ScanlationGroupDto group in userDto.ScanlationGroupRelations)
                    {
                        if (user.RelatedGroupIds.Contains(group.Id) == false)
                        {
                            user.RelatedGroupIds.Add(group.Id);
                        }
                    }
                }
            }
        }
    }
}
