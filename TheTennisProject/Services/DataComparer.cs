using System;
using System.Collections.Generic;

namespace TheTennisProject.Services
{
    /// <summary>
    /// Vérificateur d'égalité entre deux instances de <see cref="BaseService"/>.
    /// </summary>
    public class DataComparer : IEqualityComparer<BaseService>
    {
        /// <summary>
        /// Vérifié l'équivalence (par leur type sous-jacent et leur identifiant) de deux instances de <see cref="BaseService"/>.
        /// </summary>
        /// <param name="x">Objet 1.</param>
        /// <param name="y">Objet 2.</param>
        /// <returns><c>Vrai</c> si les instances sont équivalentes ; <c>Faux</c> sinon.</returns>
        public bool Equals(BaseService x, BaseService y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }

            if (x.GetType() != y.GetType())
            {
                return false;
            }

            return x.ID == y.ID;
        }

        /// <summary>
        /// Surcharge de la méthode de hachage.
        /// </summary>
        /// <remarks>Non implémentée, récupère le hachage de base.</remarks>
        /// <param name="obj">L'objet.</param>
        /// <returns>Code de hachage.</returns>
        public int GetHashCode(BaseService obj)
        {
            return obj.GetHashCode();
        }
    }
}
