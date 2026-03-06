using System;

namespace projetua3.Utils
{
    /// <summary>
    /// Classe utilitaire statique pour la manipulation des chaines de caracteres
    /// Fournit des methodes simplifiees pour le nettoyage et la validation
    /// </summary>
    public static class SimpleStringHelper
    {
        /// <summary>
        /// Nettoie une chaine en supprimant les espaces au debut et a la fin
        /// et en convertissant tous les caracteres en majuscules
        /// </summary>
        /// <param name="str">Chaine a nettoyer</param>
        /// <returns>Chaine nettoyee en majuscules ou chaine vide si null</returns>
        public static string Clean(string str)
        {
            if (str == null)
                return string.Empty;

            return str.Trim().ToUpper();
        }

        /// <summary>
        /// Verifie si une chaine est vide, nulle ou composee uniquement d'espaces
        /// </summary>
        /// <param name="str">Chaine a verifier</param>
        /// <returns>True si la chaine est vide ou null, False sinon</returns>
        public static bool IsEmpty(string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Capitalise la premiere lettre d'une chaine et met le reste en minuscules
        /// </summary>
        /// <param name="str">Chaine a capitaliser</param>
        /// <returns>Chaine capitalisee ou chaine vide si null</returns>
        public static string Capitalize(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            str = str.Trim();
            return char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }

        /// <summary>
        /// Tronque une chaine a une longueur maximale et ajoute des points de suspension
        /// </summary>
        /// <param name="str">Chaine a tronquer</param>
        /// <param name="maxLength">Longueur maximale</param>
        /// <returns>Chaine tronquee avec "..." si necessaire</returns>
        public static string Truncate(string str, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Length <= maxLength)
                return str ?? string.Empty;

            return str.Substring(0, maxLength - 3) + "...";
        }
    }
}