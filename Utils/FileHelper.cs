using System;
using System.IO;

namespace projetua3.Utils
{
    /// <summary>
    /// Classe utilitaire statique pour la gestion des operations sur les fichiers texte
    /// Fournit des methodes simplifiees pour la lecture et l'ecriture
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Lit le contenu complet d'un fichier texte
        /// </summary>
        /// <param name="filePath">Chemin du fichier a lire</param>
        /// <returns>Contenu du fichier ou chaine vide si le fichier n'existe pas</returns>
        public static string ReadFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Le chemin du fichier ne peut pas etre vide.", nameof(filePath));

            if (!File.Exists(filePath))
                return string.Empty;

            try
            {
                return File.ReadAllText(filePath);
            }
            catch (IOException ex)
            {
                throw new IOException($"Erreur lors de la lecture du fichier '{filePath}'.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException($"Acces refuse au fichier '{filePath}'.", ex);
            }
        }

        /// <summary>
        /// Ecrit du contenu dans un fichier texte
        /// Cree le fichier s'il n'existe pas, ecrase le contenu s'il existe
        /// </summary>
        /// <param name="filePath">Chemin du fichier a ecrire</param>
        /// <param name="content">Contenu a ecrire dans le fichier</param>
        public static void WriteFile(string filePath, string content)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Le chemin du fichier ne peut pas etre vide.", nameof(filePath));

            try
            {
                // Creer le repertoire si necessaire
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                File.WriteAllText(filePath, content);
            }
            catch (IOException ex)
            {
                throw new IOException($"Erreur lors de l'ecriture dans le fichier '{filePath}'.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException($"Acces refuse au fichier '{filePath}'.", ex);
            }
        }

        /// <summary>
        /// Verifie si un fichier existe
        /// </summary>
        /// <param name="filePath">Chemin du fichier a verifier</param>
        /// <returns>True si le fichier existe, False sinon</returns>
        public static bool FileExists(string filePath)
        {
            return !string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath);
        }

        /// <summary>
        /// Supprime un fichier s'il existe
        /// </summary>
        /// <param name="filePath">Chemin du fichier a supprimer</param>
        /// <returns>True si le fichier a ete supprime, False s'il n'existait pas</returns>
        public static bool DeleteFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return false;

            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (IOException ex)
            {
                throw new IOException($"Erreur lors de la suppression du fichier '{filePath}'.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException($"Acces refuse pour supprimer le fichier '{filePath}'.", ex);
            }
        }
    }
}