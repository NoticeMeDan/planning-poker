namespace PlanningPoker.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Shared;

    public class EntityMapper
    {
        public static ICollection<Item> ToItemEntities(ICollection<ItemDTO> dtos)
        {
            var entities = new HashSet<Item>();
            dtos.ToList().ForEach(i => entities.Add(
                new Item
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Rounds = ToRoundEntities(i.Rounds),
                }));

            return entities;
        }

        public static ICollection<ItemDTO> ToItemDtos(ICollection<Item> entities)
        {
            var dtos = new HashSet<ItemDTO>();
            entities.ToList().ForEach(i => dtos.Add(
                new ItemDTO
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Rounds = ToRoundDtos(i.Rounds),
                }));

            return dtos;
        }

        public static ICollection<Round> ToRoundEntities(ICollection<RoundDTO> dtos)
        {
            var entities = new HashSet<Round>();
            dtos.ToList().ForEach(r => entities.Add(
                new Round
                {
                    Id = r.Id,
                    Votes = ToVoteEntities(r.Votes)
                }));

            return entities;
        }

        public static ICollection<RoundDTO> ToRoundDtos(ICollection<Round> entities)
        {
            var dtos = new HashSet<RoundDTO>();
            entities.ToList().ForEach(r => dtos.Add(
                new RoundDTO
                {
                    Id = r.Id,
                    Votes = ToVoteDtos(r.Votes)
                }));

            return dtos;
        }

        public static ICollection<Vote> ToVoteEntities(ICollection<VoteDTO> dtos)
        {
            var entities = new HashSet<Vote>();
            dtos.ToList().ForEach(v => entities.Add(
                new Vote
                {
                    Id = v.Id,
                    UserId = v.UserId,
                    Estimate = v.Estimate
                }));

            return entities;
        }

        public static ICollection<VoteDTO> ToVoteDtos(ICollection<Vote> entities)
        {
            var dtos = new HashSet<VoteDTO>();
            entities.ToList().ForEach(v => dtos.Add(
                new VoteDTO
                {
                    Id = v.Id,
                    UserId = v.UserId,
                    Estimate = v.Estimate
                }));

            return dtos;
        }

        public static ICollection<User> ToUserEntities(ICollection<UserDTO> dtos)
        {
            var entities = new HashSet<User>();
            dtos.ToList().ForEach(u => entities.Add(
                new User
                {
                    Id = u.Id,
                    IsHost = u.IsHost,
                    Email = u.Email,
                    Nickname = u.Nickname
                }));

            return entities;
        }

        public static ICollection<UserDTO> ToUserDtos(ICollection<User> entities)
        {
            var dtos = new HashSet<UserDTO>();
            entities.ToList().ForEach(u => dtos.Add(
                new UserDTO
                {
                    Id = u.Id,
                    IsHost = u.IsHost,
                    Email = u.Email,
                    Nickname = u.Nickname
                }));

            return dtos;
        }

        public static ICollection<ItemEstimate> ToItemEstimateEntities(ICollection<ItemEstimateDTO> dtos)
        {
            var entities = new HashSet<ItemEstimate>();
            dtos.ToList().ForEach(ie => entities.Add(
                new ItemEstimate
                {
                    Id = ie.Id,
                    ItemTitle = ie.ItemTitle,
                    Estimate = ie.Estimate
                }));

            return entities;
        }

        public static ICollection<ItemEstimateDTO> ToItemEstimateDtos(ICollection<ItemEstimate> entities)
        {
            var dtos = new HashSet<ItemEstimateDTO>();
            entities.ToList().ForEach(ie => dtos.Add(
                new ItemEstimateDTO
                {
                    Id = ie.Id,
                    ItemTitle = ie.ItemTitle,
                    Estimate = ie.Estimate
                }));

            return dtos;
        }

        public static SessionDTO ToSessionDTO(Session session)
        {
            return new SessionDTO
            {
                Id = session.Id,
                Items = ToItemDtos(session.Items),
                Users = ToUserDtos(session.Users),
                SessionKey = session.SessionKey
            };
        }
    }
}
