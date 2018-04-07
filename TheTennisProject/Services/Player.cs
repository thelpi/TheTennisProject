﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TheTennisProject.Services
{
    /// <summary>
    /// Représente un joueur.
    /// </summary>
    public class Player : BaseService
    {
        #region Champs et propriétés

        /// <summary>
        /// L'identifiant de joueur utilisé quand le joueur réel est inconnu.
        /// </summary>
        public const ulong UNKNOWN_PLAYER_ID = 199999;

        /// <summary>
        /// Code ISO utilisée quand la nationalitée est indéterminée.
        /// </summary>
        public const string UNKNOWN_NATIONALITY_CODE = "UNK";

        // Historique des changements de nationalité sportive.
        private Dictionary<string, DateTime> _nationalitiesHistory = new Dictionary<string, DateTime>();

        /// <summary>
        /// Nom complet.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Nationalité sportive (code ISO de trois lettres).
        /// </summary>
        public string Nationality { get; private set; }
        /// <summary>
        /// Détermine si le joueur est gaucher (vrai), droitier (faux) ou si l'information n'est pas connue (null).
        /// </summary>
        public bool? IsLeftHanded { get; private set; }
        /// <summary>
        /// Taille, en centimètres. 0 si sa taille n'est pas connue.
        /// </summary>
        public uint Height { get; private set; }
        /// <summary>
        /// Date de naissance.
        /// </summary>
        public DateTime? DateOfBirth { get; private set; }
        /// <summary>
        /// Taille, en mètres. Calculée depuis <see cref="Height"/>.
        /// </summary>
        public decimal HeightInMeters
        {
            get
            {
                return Math.Round(this.Height / 100M, 2);
            }
        }
        /// <summary>
        /// Historique des précédentes nationalités sportives. La date spécifiée est celle de fin.
        /// <remarks>Les résultats sont triés par date croissante.</remarks>
        /// </summary>
        public Dictionary<string, DateTime> NationalitiesHistory
        {
            get
            {
                return _nationalitiesHistory.OrderBy(item => item.Value).ToDictionary(item => item.Key, item => item.Value);
            }
        }

        /// <summary>
        /// Liste de tous les joueurs instanciés.
        /// </summary>
        public static ReadOnlyCollection<Player> GetList
        {
            get
            {
                return GetList<Player>();
            }
        }

        #endregion

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="id">Identifiant.</param>
        /// <param name="name">Nom.</param>
        /// <param name="nationality">Nationalité (code ISO).</param>
        /// <param name="isLeftHanded">Détermine si le joueur est gaucher (vrai), droitier (faux) ou si l'information est inconnue (null).</param>
        /// <param name="height">Hauteur en centimètres (note : null sera remplacée par 0).</param>
        /// <param name="dateOfBirth">Date de naissance.</param>
        /// <exception cref="BaseService.NotUniqueIdException">L'identifiant n'est pas unique.</exception>
        /// <exception cref="ArgumentException">Un joueur avec le même identifiant existe déjà.</exception>
        /// <exception cref="ArgumentException">Le nom ne peut pas être vide.</exception>
        /// <exception cref="ArgumentException">La nationalité ne peut pas être vide.</exception>
        /// <exception cref="ArgumentException">La nationalité doit être un sigle de trois lettres.</exception>
        /// <exception cref="ArgumentException">L'argument spécifié n'est pas une date valide.</exception>
        public Player(ulong id, string name, string nationality, bool? isLeftHanded, uint? height, DateTime? dateOfBirth)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Le nom ne peut pas être vide.", "name");
            }

            if (string.IsNullOrWhiteSpace(nationality))
            {
                throw new ArgumentException("La nationalité ne peut pas être vide.", "nationality");
            }

            nationality = nationality.Trim().ToUpper();
            if (nationality.Length != 3)
            {
                throw new ArgumentException("La nationalité doit être un sigle de trois lettres.", "nationality");
            }

            DateOfBirth = dateOfBirth;
            Name = name;
            Nationality = nationality;
            IsLeftHanded = isLeftHanded;
            Height = height.HasValue ? height.Value : 0;
        }

        /// <summary>
        /// Ajoute une nationalité à l'historique du joueur.
        /// </summary>
        /// <remarks>L'historique n'est pas destiné à stocker la nationalité actuelle du joueur.</remarks>
        /// <param name="nationality">Nationalité (code ISO).</param>
        /// <param name="endDate">Date de fin.</param>
        /// <exception cref="ArgumentException">La nationalité ne peut pas etre vide.</exception>
        /// <exception cref="ArgumentException">La nationalité doit être un sigle de trois lettres.</exception>
        /// <exception cref="ArgumentException">La nationalité à mettre en historique ne doit pas être l'actuelle du joueur.</exception>
        /// <exception cref="ArgumentException">La nationalité spécifiée est déjà existante dans l'historique.</exception>
        /// <exception cref="ArgumentException">La date de fin spécifiée est déjà existante dans l'historique.</exception>
        public void AddNationalitiesHistoryEntry(string nationality, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(nationality))
            {
                throw new ArgumentException("La nationalité ne peut pas etre vide.", "nationality");
            }

            nationality = nationality.Trim().ToUpper();
            if (nationality.Length < 3)
            {
                throw new ArgumentException("La nationalité doit être un sigle de trois lettres.", "nationality");
            }

            if (nationality == Nationality)
            {
                throw new ArgumentException("La nationalité à mettre en historique ne doit pas être l'actuelle du joueur.", "nationality");
            }

            if (_nationalitiesHistory.ContainsKey(nationality))
            {
                throw new ArgumentException("La nationalité spécifiée est déjà existante dans l'historique.", "nationality");
            }

            if (_nationalitiesHistory.Select(item => item.Value.Date).ToList().Contains(endDate.Date))
            {
                throw new ArgumentException("La date de fin spécifiée est déjà existante dans l'historique.", "endDate");
            }

            _nationalitiesHistory.Add(nationality, endDate);
        }

        /// <summary>
        /// Calcule le nombre de points ATP (voir <see cref="PointsAtpScale"/> pour les détails) sur un an à partir d'une date donnée.
        /// </summary>
        /// <remarks>C'est la date de début de compétition qui fait foi, pas la date exacte du match (qui n'est pas connue).</remarks>
        /// <param name="beginDate">Date de début de prise en comtpe.</param>
        /// <returns>Le nombre de points ATP cumulés par le joueur sur cette période.</returns>
        /// <exception cref="ArgumentException">La date de début doit être antérieure à la date de fin.</exception>
        public long ComputeAtpPoints(DateTime beginDate)
        {
            return ComputeAtpPoints(beginDate, null);
        }

        /// <summary>
        /// Calcule le nombre de points ATP (voir <see cref="PointsAtpScale"/> pour les détails) sur une période donnée.
        /// </summary>
        /// <remarks>C'est la date de début de compétition qui fait foi, pas la date exacte du match (qui n'est pas connue).</remarks>
        /// <param name="beginDate">Date de début de prise en comtpe.</param>
        /// <param name="endDate">Date de fin de prise en compte. Si <c>Null</c>, la valeur retenue sera <paramref name="beginDate"/> incrémentée de un an.</param>
        /// <returns>Le nombre de points ATP cumulés par le joueur sur cette période.</returns>
        /// <exception cref="ArgumentException">La date de début doit être antérieure à la date de fin.</exception>
        public uint ComputeAtpPoints(DateTime beginDate, DateTime? endDate)
        {
            if (!endDate.HasValue)
            {
                endDate = beginDate.AddYears(1);
            }
            else if (endDate <= beginDate)
            {
                throw new ArgumentException("La date de début doit être antérieure à la date de fin.", "endDate");
            }

            uint points = 0;

            // TODO : fonctionner par semaine entière
            var editions = GetList<Edition>().Where(e => e.DateBegin >= beginDate && e.DateBegin <= endDate).ToList();
            foreach (var edition in editions)
            {
                points += ComputePlayerStatsForEdition(edition.ID, StatType.points);
            }

            return points;
        }

        /// <summary>
        /// Calcule les statistiques du joueur pour une édition de tournoi.
        /// </summary>
        /// <param name="editionId">Identifiant de l'édition.</param>
        /// <param name="stats">Statistique à calculer.</param>
        /// <returns>La statistique.</returns>
        public uint ComputePlayerStatsForEdition(ulong editionId, StatType stats)
        {
            var baseMatchesList = Match.GetPlayerMatches(ID)
                .Where(item => item.Edition.ID == editionId)
                .ToList();

            if (baseMatchesList.Count == 0)
            {
                return 0;
            }

            switch (stats)
            {
                case StatType.round:
                    return (uint)baseMatchesList.OrderBy(m => m.Round.GetSortOrder()).First().Round;
                case StatType.is_winner:
                    return (uint)(baseMatchesList.OrderBy(m => m.Round.GetSortOrder()).First().Winner == this ? 1 : 0);
                #region Calcul des points ATP
                case StatType.points:
                    // matchs avec points cumulés
                    var cumuledTypeMatches = baseMatchesList
                        .Where(item =>
                            PointsAtpScale.GetLevelScale(item.Edition.TournamentLevel, item.Round)[0].IsCumuled)
                        .ToList();
                    var p1 = cumuledTypeMatches.Sum(item => PointsAtpScale.GetPoints(item, this, item.PlayerWasExempt(this)));

                    // matchs perdus dés l'entrée en lice
                    var nonCumuledFirstTurnLose = baseMatchesList
                        .Where(item =>
                            !PointsAtpScale.GetLevelScale(item.Edition.TournamentLevel, item.Round)[0].IsCumuled &&
                            item.Loser == this &&
                            !baseMatchesList.Any(subItem => subItem.Edition == item.Edition && subItem.Round.RoundIsBefore(item.Round)))
                        .ToList();
                    var p2 = nonCumuledFirstTurnLose.Sum(item => PointsAtpScale.GetPoints(item, this, item.PlayerWasExempt(this)));

                    // matchs gagnés
                    var nonCumuledBestWin = baseMatchesList
                        .Where(item =>
                            !PointsAtpScale.GetLevelScale(item.Edition.TournamentLevel, item.Round)[0].IsCumuled &&
                            item.Winner == this &&
                            !baseMatchesList.Any(subItem => subItem.Edition == item.Edition && item.Round.RoundIsBefore(subItem.Round) && subItem.Winner == this))
                        .ToList();
                    var p3 = nonCumuledBestWin.Sum(item => PointsAtpScale.GetPoints(item, this, item.PlayerWasExempt(this)));

                    return (uint)(p1 + p2 + p3);
                #endregion
                case StatType.match_win:
                    return (uint)baseMatchesList.Count(m => m.Winner == this && !m.Walkover);
                case StatType.match_lost:
                    return (uint)baseMatchesList.Count(m => m.Loser == this && !m.Walkover);
                case StatType.set_win:
                    return (uint)baseMatchesList.Sum(m => m.Sets.Count(s => s.HasValue && (m.Winner == this ? s.Value.Key == this : s.Value.Key != this)));
                case StatType.set_lost:
                    return (uint)baseMatchesList.Sum(m => m.Sets.Count(s => s.HasValue && (m.Winner == this ? s.Value.Key != this : s.Value.Key == this)));
                case StatType.game_win:
                    return (uint)baseMatchesList.Sum(m => m.Sets.Sum(s => !s.HasValue ? 0 : ((m.Winner == this ? s.Value.Key == this : s.Value.Key != this) ? s.Value.Value.WScore : s.Value.Value.LScore)));
                case StatType.game_lost:
                    return (uint)baseMatchesList.Sum(m => m.Sets.Sum(s => !s.HasValue ? 0 : ((m.Winner == this ? s.Value.Key != this : s.Value.Key == this) ? s.Value.Value.WScore : s.Value.Value.LScore)));
                case StatType.tb_win:
                    return (uint)baseMatchesList.Sum(m => m.Sets.Count(s => s.HasValue && (m.Winner == this ? s.Value.Key == this : s.Value.Key != this) && s.Value.Value.IsTieBreak));
                case StatType.tb_lost:
                    return (uint)baseMatchesList.Sum(m => m.Sets.Count(s => s.HasValue && (m.Winner == this ? s.Value.Key != this : s.Value.Key == this) && s.Value.Value.IsTieBreak));
                case StatType.ace:
                    return (uint)(baseMatchesList.Sum(m => m.Winner == this ? m.WinnerCountAce ?? 0 : m.LoserCountAce ?? 0));
                case StatType.d_f:
                    return (uint)(baseMatchesList.Sum(m => m.Winner == this ? m.WinnerCountDbFault ?? 0 : m.LoserCountDbFault ?? 0));
                case StatType.sv_pt:
                    return (uint)(baseMatchesList.Sum(m => m.Winner == this ? m.WinnerCountServePt ?? 0 : m.LoserCountServePt ?? 0));
                case StatType.first_in:
                    return (uint)(baseMatchesList.Sum(m => m.Winner == this ? m.WinnerCount1stIn ?? 0 : m.LoserCount1stIn ?? 0));
                case StatType.first_won:
                    return (uint)(baseMatchesList.Sum(m => m.Winner == this ? m.WinnerCount1stWon ?? 0 : m.LoserCount1stWon ?? 0));
                case StatType.second_won:
                    return (uint)(baseMatchesList.Sum(m => m.Winner == this ? m.WinnerCount2ndWon ?? 0 : m.LoserCount2ndWon ?? 0));
                case StatType.sv_gms:
                    return (uint)(baseMatchesList.Sum(m => m.Winner == this ? m.WinnerCountServeGames ?? 0 : m.LoserCountServeGames ?? 0));
                case StatType.bp_saved:
                    return (uint)(baseMatchesList.Sum(m => m.Winner == this ? m.WinnerCountBreakPtSaved ?? 0 : m.LoserCountBreakPtSaved ?? 0));
                case StatType.bp_faced:
                    return (uint)(baseMatchesList.Sum(m => m.Winner == this ? m.WinnerCountBreakPtFaced ?? 0 : m.LoserCountBreakPtFaced ?? 0));
            }

            return 0;
        }
    }
}