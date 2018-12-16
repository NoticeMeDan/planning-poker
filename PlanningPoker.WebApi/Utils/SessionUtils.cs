namespace PlanningPoker.WebApi.Utils
{
    using System.Collections.Generic;
    using System.Linq;
    using Optional;
    using Optional.Collections;
    using Shared;

    public static class SessionUtils
    {
        /// <summary>
        /// The definition of "current active" is that the item has at least 1 round,
        /// and that none of the rounds have reached consensus.
        /// </summary>
        /// <param name="items">list of ItemDTOs</param>
        /// <param name="userCount">number of users in session</param>
        /// <returns>Option.Some if a current Item is found, Option.None otherwise</returns>
        public static Option<ItemDTO> GetCurrentActiveItem(List<ItemDTO> items, int userCount)
        {
            return items
                .LastOrNone(item => item.Rounds.Count > 0)
                .Filter(item =>
                {
                    var isOngoing = item.Rounds.Any(round =>
                    {
                        var firstEstimate = round.Votes.First().Estimate;
                        return round.Votes.Any(v => v.Estimate != firstEstimate) || round.Votes.Count != userCount;
                    });

                    return isOngoing;
                });
        }

        /// <summary>
        /// The definition of "current active" is that the round has less votes than the amount of users in the session.
        /// </summary>
        /// <param name="rounds"></param>
        /// <param name="userCount"></param>
        /// <returns>Option.Some if a current round is found, Option.None otherwise</returns>
        public static Option<RoundDTO> GetCurrentActiveRound(List<RoundDTO> rounds, int userCount)
        {
            return rounds.LastOrNone(round => round.Votes.Count < userCount);
        }
    }
}
