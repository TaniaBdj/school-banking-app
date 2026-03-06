using System;

namespace projetua3.Utils
{
    /// <summary>
    /// Classe utilitaire statique pour parser et valider les entrees utilisateur
    /// Assure la robustesse de l'application en gerant les erreurs de saisie
    /// </summary>
    public static class InputParser
    {
        /// <summary>
        /// Convertit une chaine en entier avec validation
        /// Redemande la saisie jusqu'a obtenir une valeur valide
        /// </summary>
        /// <param name="input">Chaine a convertir</param>
        /// <param name="prompt">Message a afficher en cas d'erreur</param>
        /// <returns>Valeur entiere valide</returns>
        public static int ParseInt(string input, string prompt = "Veuillez entrer un nombre entier valide : ")
        {
            int result;
            while (!int.TryParse(input, out result))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(prompt);
                Console.ResetColor();
                input = Console.ReadLine() ?? "";
            }
            return result;
        }

        /// <summary>
        /// Convertit une chaine en decimal avec validation
        /// Redemande la saisie jusqu'a obtenir une valeur valide
        /// </summary>
        /// <param name="input">Chaine a convertir</param>
        /// <param name="prompt">Message a afficher en cas d'erreur</param>
        /// <returns>Valeur decimale valide</returns>
        public static decimal ParseDecimal(string input, string prompt = "Veuillez entrer un montant valide : ")
        {
            decimal result;
            while (!decimal.TryParse(input, out result) || result < 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(prompt);
                Console.ResetColor();
                input = Console.ReadLine() ?? "";
            }
            return result;
        }

        /// <summary>
        /// Valide qu'une chaine n'est pas vide ou composee uniquement d'espaces
        /// Redemande la saisie jusqu'a obtenir une valeur non vide
        /// </summary>
        /// <param name="input">Chaine a valider</param>
        /// <param name="prompt">Message a afficher en cas d'erreur</param>
        /// <returns>Chaine non vide validee</returns>
        public static string ParseString(string input, string prompt = "La saisie ne peut pas etre vide : ")
        {
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(prompt);
                Console.ResetColor();
                input = Console.ReadLine() ?? "";
            }
            return input.Trim();
        }

        /// <summary>
        /// Lit un entier positif strictement superieur a zero
        /// </summary>
        /// <param name="input">Chaine a convertir</param>
        /// <param name="prompt">Message a afficher en cas d'erreur</param>
        /// <returns>Entier positif valide</returns>
        public static int ParsePositiveInt(string input, string prompt = "Veuillez entrer un nombre positif : ")
        {
            int result;
            while (!int.TryParse(input, out result) || result <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(prompt);
                Console.ResetColor();
                input = Console.ReadLine() ?? "";
            }
            return result;
        }

        /// <summary>
        /// Lit un decimal positif strictement superieur a zero
        /// </summary>
        /// <param name="input">Chaine a convertir</param>
        /// <param name="prompt">Message a afficher en cas d'erreur</param>
        /// <returns>Decimal positif valide</returns>
        public static decimal ParsePositiveDecimal(string input, string prompt = "Veuillez entrer un montant positif : ")
        {
            decimal result;
            while (!decimal.TryParse(input, out result) || result <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(prompt);
                Console.ResetColor();
                input = Console.ReadLine() ?? "";
            }
            return result;
        }
    }
}