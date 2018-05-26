using System.Collections.Generic;
using System.Linq;

namespace TheTennisProject.Graphics.Bindings
{
    /// <summary>
    /// Représente le joueur dans un classement ATP live, pour liaison avec le composant WPF.
    /// </summary>
    internal class LiveRanking
    {
        /// <summary>
        /// Nom du joueur.
        /// </summary>
        public string PlayerName { get; private set; }
        /// <summary>
        /// Classement du joueur.
        /// </summary>
        public uint Ranking { get; private set; }
        /// <summary>
        /// Classement du joueur l'échéance précédente.
        /// </summary>
        public uint PreviousRanking { get; private set; }
        /// <summary>
        /// Nombre de points (ATP ou ELO).
        /// </summary>
        public uint Points { get; private set; }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="playerName">Nom du joueur.</param>
        /// <param name="ranking">Classement.</param>
        /// <param name="points">Nombre de points.</param>
        /// <param name="previousRanking">Classement échéance précédente.</param>
        public LiveRanking(string playerName, uint points, uint ranking, uint previousRanking)
        {
            PlayerName = playerName;
            Points = points;
            Ranking = ranking;
            PreviousRanking = previousRanking;
        }

        /// <summary>
        /// Construit une instance à partir d'une instance de <see cref="Services.AtpRanking"/>. Dédiée au classement ATP.
        /// </summary>
        /// <param name="innerRanking">L'instance de base.</param>
        /// <param name="previousInnerRankingList">Le classement à l'échéance précédente.</param>
        /// <param name="defaultRanking">Le classement par défaut.</param>
        /// <returns>Une instance de <see cref="LiveRanking"/>.</returns>
        public static LiveRanking BuildFromAtp(Services.AtpRanking innerRanking,
            IEnumerable<Services.AtpRanking> previousInnerRankingList, uint defaultRanking)
        {
            return new LiveRanking(
                innerRanking.Player.Name,
                innerRanking.RollingPoints,
                innerRanking.RollingRank,
                previousInnerRankingList.FirstOrDefault(_ => _.Player == innerRanking.Player)?.RollingRank ?? defaultRanking);
        }

        /// <summary>
        /// Construit une instance à partir d'une instance de <see cref="Services.AtpRanking"/>. Dédiée au classement ELO.
        /// </summary>
        /// <param name="innerRanking">L'instance de base.</param>
        /// <param name="indexOfInInnerList"></param>
        /// <param name="previousInnerRankingList">Le classement à l'échéance précédente.</param>
        /// <param name="defaultRanking">Le classement par défaut.</param>
        /// <returns>Une instance de <see cref="LiveRanking"/>.</returns>
        public static LiveRanking BuildFromElo(Services.AtpRanking innerRanking, int indexOfInInnerList,
            List<Services.AtpRanking> previousInnerRankingList, uint defaultRanking)
        {
            uint previousRank = defaultRanking;
            if (previousInnerRankingList.Any(_ => _.Player.ID == innerRanking.Player.ID))
            {
                previousRank = (uint)previousInnerRankingList.IndexOf(previousInnerRankingList.First(_ => _.Player.ID == innerRanking.Player.ID)) + 1;
            }

            return new LiveRanking(
                innerRanking.Player.Name,
                innerRanking.Elo,
                (uint)(indexOfInInnerList + 1),
                previousRank
            );
        }
    }
}
