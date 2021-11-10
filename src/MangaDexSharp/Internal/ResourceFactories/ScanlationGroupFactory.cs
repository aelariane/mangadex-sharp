using System;
using System.Linq;

using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Resources;

namespace MangaDexSharp.Internal.ResourceFactories
{
    internal class ScanlationGroupFactory : IResourceFactory
    {

        private MangaDexClient _client;

        public ScanlationGroupFactory(MangaDexClient client)
        {
            _client = client;
        }

        public MangaDexResource Create(ResourceDto dto)
        {
            var groupDto = dto as ScanlationGroupDto;
            if(groupDto == null)
            {
                throw new ArgumentException($"Invald dto type {dto.GetType()}. {nameof(ScanlationGroupDto)} expected");
            }

            ScanlationGroupAttributes attributes = groupDto.Attributes;

            var group = new ScanlationGroup(
                _client,
                groupDto.Id,
                attributes.Name,
                attributes.AltNames,
                attributes.Official,
                attributes.Locked,
                attributes.CreatedAt,
                attributes.UpdatedAt);
            group.Version = attributes.Version;

            if (attributes.Description != null)
            {
                group.Description = attributes.Description;
            }
            if(attributes.FocusedLanguage != null) 
            {
                group.FocusedLanguages = attributes.FocusedLanguage;
            }
            if(attributes.ContactEmail != null)
            {
                group.ContactEmail = attributes.ContactEmail;
            }
            if (attributes.Discord != null)
            {
                group.DiscordCode = attributes.Discord;
            }
            if (attributes.IrcServer != null)
            {
                group.IrcServer = attributes.IrcServer;
            }
            if (attributes.IrcChannel != null)
            {
                group.IrcChannel = attributes.IrcChannel;
            }
            if (attributes.Website != null)
            {
                group.Website = attributes.Website;
            }

            if(groupDto.LeaderRelations != null && groupDto.LeaderRelations.Any())
            {
                group.RelatedLeaderId = groupDto.LeaderRelations.First().Id;
            }
            if(groupDto.MemberRelations != null)
            {
                foreach(UserDto user in groupDto.MemberRelations)
                {
                    group.RelatedMemberIds.Add(user.Id);
                }
            }

            return group;
        }

        public void Sync(MangaDexResource resource, ResourceDto dto)
        {
            ScanlationGroup group = (ScanlationGroup)resource;
            if(dto is ScanlationGroupDto groupDto)
            {
                if (groupDto.LeaderRelations != null && groupDto.LeaderRelations.Any() && group.LeaderId == null)
                {
                    group.RelatedLeaderId = groupDto.LeaderRelations.First().Id;
                }
                if (groupDto.MemberRelations != null)
                {
                    foreach (UserDto user in groupDto.MemberRelations)
                    {
                        if (group.RelatedMemberIds.Contains(user.Id) == false)
                        {
                            group.RelatedMemberIds.Add(user.Id);
                        }
                    }
                }
            }
        }
    }
}
