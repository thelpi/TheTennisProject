using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TheTennisProject.Services
{
    /// <summary>
    /// Représente une entrée dans la table "atp_ranking".
    /// </summary>
    public class AtpRanking
    {
        // liste de toutes les instances.
        private static List<AtpRanking> _instances = new List<AtpRanking>();

        /// <summary>
        /// Joueur.
        /// </summary>
        public Player Player { get; private set; }
        /// <summary>
        /// Année.
        /// </summary>
        public uint Year { get; private set; }
        /// <summary>
        /// Numéro de semaine.
        /// </summary>
        public uint WeekNo { get; private set; }
        /// <summary>
        /// Nombre de points cette semaine.
        /// </summary>
        public uint WeekPoints { get; private set; }
        /// <summary>
        /// Nombre cumulé de points pour cette année civile.
        /// </summary>
        public uint YearCalendarPoints { get; private set; }
        /// <summary>
        /// Nombre cumulé de points pour l'année glissante.
        /// </summary>
        public uint YearRollingPoints { get; private set; }
        /// <summary>
        /// Le classement du joueur à date pour l'année civile.
        /// </summary>
        public ushort CalendarRank { get; private set; }
        /// <summary>
        /// Le classement du joueur à date pour l'année glisante.
        /// </summary>
        public ushort RollingRank { get; private set; }
        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="playerId">Identifiant du joueur.</param>
        /// <param name="year">Année.</param>
        /// <param name="weekNo">Numéro de semaine.</param>
        /// <param name="weekPoints">Nombre de points cette semaine.</param>
        /// <param name="yearCalendarPoints">Nombre cumulé de points pour cette année civile.</param>
        /// <param name="yearRollingPoints">Nombre cumulé de points pour l'année glissante.</param>
        public AtpRanking(ulong playerId, uint year, uint weekNo, uint weekPoints, uint yearCalendarPoints, uint yearRollingPoints)
        {
            Player = Player.GetById(playerId);
            Year = year;
            WeekNo = weekNo;
            WeekPoints = weekPoints;
            YearCalendarPoints = yearCalendarPoints;
            YearRollingPoints = yearRollingPoints;
            _instances.Add(this);
        }

        /// <summary>
        /// Récupère le classement ATP à un moment précis dans le temps.
        /// </summary>
        /// <param name="date">Date du classement (le dimanche).</param>
        /// <param name="rollingSort">Si vrai, le tri est l'année glissante ; sinon, sur l'année civile.</param>
        /// <param name="limit">Nombre de joueurs retournés.</param>
        /// <returns>Le classement, trié par performance décroissante.</returns>
        public static ReadOnlyCollection<AtpRanking> GetRankingAtDate(DateTime date, bool rollingSort, int limit)
        {
            return _instances
                        .Where(_ => _.WeekNo == Tools.GetWeekNoFromDate(date) && _.Year == date.Year)
                        .OrderByDescending(_ => (rollingSort ? _.YearRollingPoints : _.YearCalendarPoints))
                        .Take(limit)
                        .ToList()
                        .AsReadOnly();
        }
    }
}
