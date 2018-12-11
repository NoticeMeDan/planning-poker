namespace PlanningPoker.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Shared;

    public class EntityMapper
    {
        public static List<Item> ToItemEntities(List<ItemDTO> dtos)
        {
            if(dtos == null)
            {
                return new List<Item>();
            }
            var entities = new List<Item>();
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

        public static List<Item> ToItemEntities(List<ItemCreateUpdateDTO> dtos)
        {
            if (dtos == null)
            {
                return new List<Item>();
            }
            var entities = new List<Item>();
            dtos.ToList().ForEach(i => entities.Add(
                new Item
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Rounds = ToRoundEntities(i.Rounds)
                }));

            return entities;
        }

        public static List<ItemCreateUpdateDTO> ToItemCreateUpdateDtos(List<ItemDTO> dtos)
        {
            if (dtos == null)
            {
                return new List<ItemCreateUpdateDTO>();
            }

            var entities = new List<ItemCreateUpdateDTO>();
            dtos.ToList().ForEach(i => entities.Add(
                new ItemCreateUpdateDTO
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Rounds = i.Rounds
                }));

            return entities;
        }

        public static List<ItemDTO> ToItemDtos(List<Item> entities)
        {
            if (entities == null)
            {
                return new List<ItemDTO>();
            }
            var dtos = new List<ItemDTO>();
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
            if (dtos == null)
            {
                return new List<Round>();
            }
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
            if (entities == null)
            {
                return new List<RoundDTO>();
            }
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

        public static ICollection<User> ToUserEntities(ICollection<UserCreateDTO> dtos)
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

        public static ICollection<UserCreateDTO> ToUserCreateDtos(ICollection<UserDTO> users)
        {
            var dtos = new HashSet<UserCreateDTO>();
            users.ToList().ForEach(u => dtos.Add(
                new UserCreateDTO
                {
                    Id = u.Id,
                    IsHost = u.IsHost,
                    Email = u.Email,
                    Nickname = u.Nickname
                }));

            return dtos;
        }

        public static List<ItemEstimate> ToItemEstimateEntities(List<ItemEstimateDTO> dtos)
        {
            var entities = new List<ItemEstimate>();
            dtos.ToList().ForEach(ie => entities.Add(
                new ItemEstimate
                {
                    Id = ie.Id,
                    ItemTitle = ie.ItemTitle,
                    Estimate = ie.Estimate
                }));

            return entities;
        }

        public static List<ItemEstimateDTO> ToItemEstimateDtos(List<ItemEstimate> entities)
        {
            var dtos = new List<ItemEstimateDTO>();
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

        public static SessionCreateUpdateDTO ToSessionCreateUpdateDTO(SessionDTO session)
        {
            return new SessionCreateUpdateDTO
            {
                Id = session.Id,
                SessionKey = session.SessionKey,
                Items = ToItemCreateUpdateDtos(session.Items),
                Users = ToUserCreateDtos(session.Users)
            };
        }
    }
}
