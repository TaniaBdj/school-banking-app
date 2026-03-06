using System;

namespace projetua3.Utils
{
    /// <summary>
    /// Classe responsable de l'affichage du menu console et de la capture des choix utilisateur
    /// Respecte le principe de responsabilite unique (SRP) - gestion de l'interface utilisateur
    /// </summary>
    public class ConsoleMenu
    {
        /// <summary>
        /// Affiche le menu principal de l'application bancaire
        /// </summary>
        public void DisplayMenu()
        {
            Console.WriteLine("\n╔══════════════════════════════════════╗");
            Console.WriteLine("║       CITEBANQUE - Menu Principal      ║");
            Console.WriteLine("╠════════════════════════════════════════╣");
            Console.WriteLine("║  1. Creer un compte                    ║");
            Console.WriteLine("║  2. Lister tous les comptes            ║");
            Console.WriteLine("║  3. Rechercher un compte               ║");
            Console.WriteLine("║  4. Effectuer un depot                 ║");
            Console.WriteLine("║  5. Effectuer un retrait               ║");
            Console.WriteLine("║  6. Effectuer un transfert             ║");
            Console.WriteLine("║  7. Quitter                            ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
        }

        /// <summary>
        /// Demande a l'utilisateur de choisir une option du menu
        /// Valide l'entree et retourne le choix
        /// </summary>
        /// <returns>Numero du choix (1-7) ou 0 si invalide</returns>
        public int GetMenuChoice()
        {
            Console.Write("\nVotre choix : ");
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input.Trim(), out int choice) && choice >= 1 && choice <= 7)
                return choice;

            return 0;
        }

        /// <summary>
        /// Affiche un message d'erreur formate
        /// </summary>
        /// <param name="message">Message d'erreur a afficher</param>
        public void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[ERREUR] {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Affiche un message de succes formate
        /// </summary>
        /// <param name="message">Message de succes a afficher</param>
        public void DisplaySuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[SUCCES] {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Affiche un message d'information formate
        /// </summary>
        /// <param name="message">Message d'information a afficher</param>
        public void DisplayInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n[INFO] {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Attend que l'utilisateur appuie sur une touche pour continuer
        /// </summary>
        public void WaitForUser()
        {
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        /// <summary>
        /// Efface l'ecran de la console
        /// </summary>
        public void ClearScreen()
        {
            Console.Clear();
        }
    }
}