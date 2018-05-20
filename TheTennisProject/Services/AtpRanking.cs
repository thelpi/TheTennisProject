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
        // liste de toutes les instances groupées par date et triées par rang sur l'année glissante.
        private static Dictionary<Tuple<uint, uint>, List<AtpRanking>> _instancesByDateRollingRanking = new Dictionary<Tuple<uint, uint>, List<AtpRanking>>();
        // liste de toutes les instances groupées par date et triées par rang sur l'année civile.
        private static Dictionary<Tuple<uint, uint>, List<AtpRanking>> _instancesByDateCalendarRanking = new Dictionary<Tuple<uint, uint>, List<AtpRanking>>();

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
        /// Date correspondante à la combination <see cref="Year"/> / <see cref="WeekNo"/>.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return Tools.GetMondayOfFirstWeekIsoOfYear((int)Year).AddDays(-1).AddDays(WeekNo * 7);
            }
        }
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
        /// Nombre de semaines à la première place (année glissante) en cumulé.
        /// </summary>
        public int CumulWeekTop1 { get; private set; }
        /// <summary>
        /// Nombre de semaines dans le top 3 (année glissante) en cumulé.
        /// </summary>
        public int CumulWeekTop3 { get; private set; }
        /// <summary>
        /// Nombre de semaines dans le top 10 (année glissante) en cumulé.
        /// </summary>
        public int CumulWeekTop10 { get; private set; }
        /// <summary>
        /// Nombre de semaines dans le top 100 (année glissante) en cumulé.
        /// </summary>
        public int CumulWeekTop100 { get; private set; }

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
            List<AtpRanking> instancesAtDate =
                (rollingSort ? _instancesByDateRollingRanking : _instancesByDateCalendarRanking)
                    .FirstOrDefault(_ => _.Key.Item1 == date.Year && _.Key.Item2 == Tools.GetWeekNoFromDate(date))
                    .Value;

            // Sécurité dans le cas où une date invalide est spécifiée
            if (instancesAtDate == null)
            {
                instancesAtDate = new List<AtpRanking>();
            }

            return instancesAtDate
                    .Take(limit)
                    .ToList()
                    .AsReadOnly();
        }

        /// <summary>
        /// Récupère à date le classement des joueurs ayant cumulés le plus grand nombre de semaines dans le top 1.
        /// </summary>
        /// <param name="date">Date du classement (le dimanche).</param>
        /// <param name="limit">Nombre de joueurs retournés.</param>
        /// <returns>A date, les informations de classement des <paramref name="limit"/> joueurs les mieux classés au cumul.</returns>
        public static ReadOnlyCollection<AtpRanking> GetCumulWeekTop1RankingAtDate(DateTime date, int limit)
        {
            return _instances
                    .Where(_ => _.Year == date.Year && _.WeekNo == Tools.GetWeekNoFromDate(date))
                    .OrderByDescending(_ => _.CumulWeekTop1)
                    .Take(limit)
                    .ToList()
                    .AsReadOnly();
        }

        /// <summary>
        /// Récupère à date le classement des joueurs ayant cumulés le plus grand nombre de semaines dans le top 3.
        /// </summary>
        /// <param name="date">Date du classement (le dimanche).</param>
        /// <param name="limit">Nombre de joueurs retournés.</param>
        /// <returns>A date, les informations de classement des <paramref name="limit"/> joueurs les mieux classés au cumul.</returns>
        public static ReadOnlyCollection<AtpRanking> GetCumulWeekTop3RankingAtDate(DateTime date, int limit)
        {
            return _instances
                    .Where(_ => _.Year == date.Year && _.WeekNo == Tools.GetWeekNoFromDate(date))
                    .OrderByDescending(_ => _.CumulWeekTop3)
                    .Take(limit)
                    .ToList()
                    .AsReadOnly();
        }

        /// <summary>
        /// Récupère à date le classement des joueurs ayant cumulés le plus grand nombre de semaines dans le top 10.
        /// </summary>
        /// <param name="date">Date du classement (le dimanche).</param>
        /// <param name="limit">Nombre de joueurs retournés.</param>
        /// <returns>A date, les informations de classement des <paramref name="limit"/> joueurs les mieux classés au cumul.</returns>
        public static ReadOnlyCollection<AtpRanking> GetCumulWeekTop10RankingAtDate(DateTime date, int limit)
        {
            return _instances
                    .Where(_ => _.Year == date.Year && _.WeekNo == Tools.GetWeekNoFromDate(date))
                    .OrderByDescending(_ => _.CumulWeekTop10)
                    .Take(limit)
                    .ToList()
                    .AsReadOnly();
        }

        /// <summary>
        /// Récupère à date le classement des joueurs ayant cumulés le plus grand nombre de semaines dans le top 100.
        /// </summary>
        /// <param name="date">Date du classement (le dimanche).</param>
        /// <param name="limit">Nombre de joueurs retournés.</param>
        /// <returns>A date, les informations de classement des <paramref name="limit"/> joueurs les mieux classés au cumul.</returns>
        public static ReadOnlyCollection<AtpRanking> GetCumulWeekTop100RankingAtDate(DateTime date, int limit)
        {
            return _instances
                    .Where(_ => _.Year == date.Year && _.WeekNo == Tools.GetWeekNoFromDate(date))
                    .OrderByDescending(_ => _.CumulWeekTop100)
                    .Take(limit)
                    .ToList()
                    .AsReadOnly();
        }

        /// <summary>
        /// Compute the rolling and calendar ranking of each known week, then compute individual statistics.
        /// </summary>
        public static void SetRankingAndStatistics()
        {
            List<Tuple<uint, uint>> weekNoList =
                _instances
                    .Select(_ => new Tuple<uint, uint>(_.Year, _.WeekNo))
                    .Distinct()
                    .OrderBy(_ => _.Item1)
                    .ThenBy(_ => _.Item2)
                    .ToList();

            foreach (Tuple<uint, uint> weekNo in weekNoList)
            {
                IEnumerable<AtpRanking> rankingList = _instances.Where(_ => _.Year == weekNo.Item1 && _.WeekNo == weekNo.Item2);
                _instancesByDateRollingRanking.Add(weekNo, rankingList.OrderByDescending(_ => _.YearRollingPoints).ToList());
                _instancesByDateCalendarRanking.Add(weekNo, rankingList.OrderByDescending(_ => _.YearCalendarPoints).ToList());

                List<AtpRanking> previousRankingList =
                    _instances
                        .Where(_ => _.Year == weekNo.Item1 && _.WeekNo < weekNo.Item2 || _.Year < weekNo.Item1)
                        .OrderByDescending(_ => _.Date)
                        .ToList();

                List<Player> players =
                    rankingList
                        .Select(_ => _.Player)
                        .Distinct(new DataComparer())
                        .Cast<Player>()
                        .ToList();
                foreach (Player p in players)
                {
                    AtpRanking lastPreviousRankingOfPlayer = previousRankingList.FirstOrDefault(_ => _.Player.ID == p.ID);

                    AtpRanking instanceToEvaluate = _instancesByDateRollingRanking[weekNo].First(_ => _.Player.ID == p.ID);
                    int rankofInstance = _instancesByDateRollingRanking[weekNo].IndexOf(instanceToEvaluate) + 1;
                    if (rankofInstance > 1 && instanceToEvaluate.YearRollingPoints == _instancesByDateRollingRanking[weekNo].ElementAt(0).YearRollingPoints)
                    {
                        rankofInstance = 1;
                    }
                    else if (rankofInstance > 3 && instanceToEvaluate.YearRollingPoints == _instancesByDateRollingRanking[weekNo].ElementAt(2).YearRollingPoints)
                    {
                        rankofInstance = 3;
                    }
                    else if (rankofInstance > 10 && instanceToEvaluate.YearRollingPoints == _instancesByDateRollingRanking[weekNo].ElementAt(9).YearRollingPoints)
                    {
                        rankofInstance = 10;
                    }
                    else if (rankofInstance > 100 && instanceToEvaluate.YearRollingPoints == _instancesByDateRollingRanking[weekNo].ElementAt(99).YearRollingPoints)
                    {
                        rankofInstance = 100;
                    }

                    instanceToEvaluate.CumulWeekTop1 = (lastPreviousRankingOfPlayer?.CumulWeekTop1 ?? 0) + (rankofInstance <= 1 ? 1 : 0);
                    instanceToEvaluate.CumulWeekTop3 = (lastPreviousRankingOfPlayer?.CumulWeekTop3 ?? 0) + (rankofInstance <= 3 ? 1 : 0);
                    instanceToEvaluate.CumulWeekTop10 = (lastPreviousRankingOfPlayer?.CumulWeekTop10 ?? 0) + (rankofInstance <= 10 ? 1 : 0);
                    instanceToEvaluate.CumulWeekTop100 = (lastPreviousRankingOfPlayer?.CumulWeekTop100 ?? 0) + (rankofInstance <= 100 ? 1 : 0);
                }
            }
        }
    }
}
