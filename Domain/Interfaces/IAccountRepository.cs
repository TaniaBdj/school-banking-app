using System.Collections.Generic;
using projetua3.Domain.Entities;

namespace projetua3.Domain.Interfaces
{
    /// <summary>
    /// Interface pour la gestion du stockage et de la persistence des comptes bancaires
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Ajoute un nouveau compte au repository
        /// </summary>
        /// <param name="account">Compte a ajouter</param>
        void Add(Account account);

        /// <summary>
        /// Met a jour un compte existant dans le repository
        /// </summary>
        /// <param name="account">Compte a mettre a jour</param>
        void Update(Account account);

        /// <summary>
        /// Recherche un compte par son numero unique
        /// </summary>
        /// <param name="accountNumber">Numero du compte a rechercher</param>
        /// <returns>Le compte trouve ou null si inexistant</returns>
        Account GetByNumber(int accountNumber);

        /// <summary>
        /// Retourne la liste complete de tous les comptes
        /// </summary>
        /// <returns>Collection de tous les comptes bancaires</returns>
        IEnumerable<Account> GetAll();

        /// <summary>
        /// Genere le prochain numero de compte unique disponible
        /// </summary>
        /// <returns>Numero de compte unique pour un nouveau compte</returns>
        int GetNextAccountNumber();

        /// <summary>
        /// Sauvegarde toutes les modifications en cours dans le systeme de stockage
        /// </summary>
        void SaveChanges();
    }
}
