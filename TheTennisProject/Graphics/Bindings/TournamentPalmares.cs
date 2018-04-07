using System.Collections.Generic;
using System.Linq;

namespace TheTennisProject.Graphics.Bindings
{
    /// <summary>
    /// Représente le palmarès d'un tournoi, pour liaison avec le composant WPF.
    /// </summary>
    internal class TournamentPalmares
    {
        /// <summary>
        /// Année.
        /// </summary>
        public uint Year { get; private set; }

        /// <summary>
        /// Le joueur vainqueur.
        /// </summary>
        public string Winner { get; private set; }

        /// <summary>
        /// Le joueur finaliste.
        /// </summary>
        public string Finalist { get; private set; }

        /// <summary>
        /// Le premier joueur demi-finaliste (3ème le cas échéant).
        /// </summary>
        public string SemiFinalist1 { get; private set; }

        /// <summary>
        /// Le second joueur demi-finaliste.
        /// </summary>
        public string SemiFinalist2 { get; private set; }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="year">Année de l'édition du tournoi.</param>
        /// <param name="matchs">Liste des matches du tournoi pour cette édition.</param>
        public TournamentPalmares(uint year, IEnumerable<Services.Match> matchs)
        {
            Year = year;
            Winner = matchs?.FirstOrDefault(_ => _.Round == Services.Round.F)?.Winner?.Name ?? "N/A";
            Finalist = matchs?.FirstOrDefault(_ => _.Round == Services.Round.F)?.Loser?.Name ?? "N/A";
            if (matchs?.Any(_ => _.Round == Services.Round.BR) == true)
            {
                SemiFinalist1 = matchs?.FirstOrDefault(_ => _.Round == Services.Round.BR)?.Winner?.Name ?? "N/A";
                SemiFinalist2 = matchs?.FirstOrDefault(_ => _.Round == Services.Round.BR)?.Loser?.Name ?? "N/A";
            }
            else
            {
                SemiFinalist1 = matchs?.FirstOrDefault(_ => _.Round == Services.Round.SF)?.Loser?.Name ?? "N/A";
                SemiFinalist2 = matchs?.LastOrDefault(_ => _.Round == Services.Round.SF)?.Loser?.Name ?? "N/A";
            }
        }
    }
}
