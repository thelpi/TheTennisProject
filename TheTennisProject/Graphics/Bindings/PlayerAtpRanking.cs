using System;
using System.Collections.Generic;
using System.Linq;

namespace TheTennisProject.Graphics.Bindings
{
    /// <summary>
    /// Représente les statistiques d'un joueur, pour liaison avec le composant WPF.
    /// </summary>
    internal class PlayerAtpRanking : IEquatable<PlayerAtpRanking>
    {
        // Statistiques.
        private Dictionary<Services.StatType, uint> _statistics = new Dictionary<Services.StatType, uint>();

        /// <summary>
        /// L'objet <see cref="Services.Player"/> sous-jacent.
        /// </summary>
        public Services.Player InnerPlayer { get; private set; }
        /// <summary>
        /// Classement du joueur après tri sur une statistique.
        /// </summary>
        public ushort Ranking { get; private set; }
        /// <summary>
        /// Copie des statistiques en lecture-seule.
        /// </summary>
        public Dictionary<Services.StatType, uint> Statistics
        {
            get
            {
                return _statistics.ToDictionary(s => s.Key, s => s.Value);
            }
        }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="innerPlayer">Le joueur.</param>
        /// <param name="statistics">Statistiques.</param>
        /// <exception cref="ArgumentNullException">Le paramètre <paramref name="innerPlayer"/> ne peut pas être <c>Null</c>.</exception>
        public PlayerAtpRanking(Services.Player innerPlayer, Dictionary<Services.StatType, uint> statistics)
        {
            if (innerPlayer == null)
            {
                throw new ArgumentNullException("innerPlayer");
            }

            InnerPlayer = innerPlayer;
            Ranking = 0;
            if (statistics != null)
            {
                _statistics = statistics.ToDictionary(s => s.Key, s => s.Value);
            }
        }

        /// <summary>
        /// Met à jour la valeur d'une statistique par addition.
        /// </summary>
        /// <param name="statType">La statistique.</param>
        /// <param name="statValue">La valeur à ajouter.</param>
        public void AggregateStatistic(Services.StatType statType, uint statValue)
        {
            if (_statistics.ContainsKey(statType))
            {
                _statistics[statType] += statValue;
            }
        }

        /// <summary>
        /// Applique des paramètres de tri sur la liste de <see cref="PlayerAtpRanking"/> fournie, et leur assigne un rang suite à ce tri.
        /// </summary>
        /// <remarks>En l'absence de paramètre de tri valide, celui-ci se fait sur le nom du joueur croissant.</remarks>
        /// <param name="originalList">Liste de <see cref="PlayerAtpRanking"/> à trier.</param>
        /// <param name="sortParameter">Propriétés utilisées pour le tri, par order de priorité décroissante.
        /// La clé attendu est le nom exact des propriétés de l'objet, à savoir "Name" (depuis <see cref="InnerPlayer"/>), <see cref="Ranking"/> ou valeur de l'énumération <see cref="Services.StatType"/> convertie en chaîne de caractères.
        /// La clé <see cref="Ranking"/> n'a de sens qu'en première position et décroissante. Sinon, elle est ignorée.
        /// Le booléen indique un tri croissant (<c>False</c>) ou décroissant (<c>True</c>) sur cette propriété.</param>
        /// <returns>Liste triée avec rang assigné.</returns>
        public static List<PlayerAtpRanking> SortAndSetRanking(ref List<PlayerAtpRanking> originalList, Dictionary<string, bool> sortParameter)
        {
            if (originalList == null)
            {
                originalList = new List<PlayerAtpRanking>();
            }

            // tri par défaut "première chance"
            if (sortParameter == null)
            {
                sortParameter = new Dictionary<string, bool>();
            }

            IOrderedEnumerable<PlayerAtpRanking> orderedList = originalList.OrderBy(p => p.InnerPlayer.Name);
            bool firstSort = true;
            foreach (var propName in sortParameter.Keys)
            {
                if (propName == "Name")
                {
                    if (firstSort)
                    {
                        if (sortParameter[propName])
                        {
                            orderedList = orderedList.OrderByDescending(p => p.InnerPlayer.Name);
                        }
                        else
                        {
                            orderedList = orderedList.OrderBy(p => p.InnerPlayer.Name);
                        }
                        firstSort = false;
                    }
                    else
                    {
                        if (sortParameter[propName])
                        {
                            orderedList = orderedList.ThenByDescending(p => p.InnerPlayer.Name);
                        }
                        else
                        {
                            orderedList = orderedList.ThenBy(p => p.InnerPlayer.Name);
                        }
                    }
                }
                else if (propName == "Ranking")
                {
                    // ne trie jamais par rang ici
                    // la valeur de "firstSort" ne doit pas être modifiée
                }
                else
                {
                    Services.StatType statType;
                    if (Enum.TryParse(propName, out statType))
                    {
                        if (firstSort)
                        {
                            if (sortParameter[propName])
                            {
                                orderedList = orderedList.OrderByDescending(p => p.Statistics[statType]);
                            }
                            else
                            {
                                orderedList = orderedList.OrderBy(p => p.Statistics[statType]);
                            }
                            firstSort = false;
                        }
                        else
                        {
                            if (sortParameter[propName])
                            {
                                orderedList = orderedList.ThenByDescending(p => p.Statistics[statType]);
                            }
                            else
                            {
                                orderedList = orderedList.ThenBy(p => p.Statistics[statType]);
                            }
                        }
                    }
                }
            }

            originalList = orderedList.ToList();

            // calcule le rang
            foreach (var element in originalList)
            {
                element.Ranking = (ushort)(originalList.IndexOf(element) + 1);
            }

            // inverse l'ordre
            if (sortParameter.Keys.First() == "Ranking" && sortParameter["Ranking"])
            {
                originalList.Reverse();
            }

            return originalList;
        }

        /// <summary>
        /// Vérifie l'égalité entre cette instance et une autre.
        /// </summary>
        /// <param name="other">L'autre instance.</param>
        /// <returns><c>True</c> si égalité, <c>False</c> sinon.</returns>
        public bool Equals(PlayerAtpRanking other)
        {
            return InnerPlayer.ID == other?.InnerPlayer.ID;
        }
    }
}
