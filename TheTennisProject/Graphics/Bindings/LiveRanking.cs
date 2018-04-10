using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

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
        /// Nombre de points.
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
    }
}
