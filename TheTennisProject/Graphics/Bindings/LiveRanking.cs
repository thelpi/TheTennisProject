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
        public string Ranking { get; private set; }
        /// <summary>
        /// Nombre de points.
        /// </summary>
        public uint Points { get; private set; }
        /// <summary>
        /// Couleur du pinceau.
        /// </summary>
        public Brush BarBrush
        {
            get
            {
                switch (Ranking)
                {
                    case "01":
                        return Brushes.Gold;
                    case "02":
                        return Brushes.Silver;
                    case "03":
                        return Brushes.Peru;
                    default:
                        return Brushes.Lavender;
                }
            }
        }
        /// <summary>
        /// Taille de la barre.
        /// </summary>
        public double BarWidth
        {
            get
            {
                return 150 + ((Points * 300) / (double)18000);
            }
        }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="playerName">Nom du joueur.</param>
        /// <param name="ranking">Classement.</param>
        /// <param name="points">Nombre de points.</param>
        public LiveRanking(string playerName, uint points, string ranking)
        {
            PlayerName = playerName;
            Points = points;
            Ranking = ranking;
        }
    }
}
