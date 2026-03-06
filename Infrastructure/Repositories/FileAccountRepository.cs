using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using projetua3.Domain.Entities;
using projetua3.Domain.Interfaces;

namespace projetua3.Infrastructure
{
    /// <summary>
    /// Repository pour la gestion de la persistence des comptes dans un fichier JSON
    /// Respecte le principe d'inversion de dependance (DIP) de SOLID
    /// </summary>
    public class FileAccountRepository : IAccountRepository
    {
        private readonly string _filePath;
        private List<Account> _accounts;

        /// <summary>
        /// Constructeur avec chemin de fichier configurable
        /// </summary>
        /// <param name="filePath">Chemin du fichier JSON pour stocker les comptes (defaut: accounts.json)</param>
        public FileAccountRepository(string filePath = "accounts.json")
        {
            _filePath = filePath;
            _accounts = LoadAccountsFromFile();
        }

        /// <summary>
        /// Ajoute un nouveau compte au repository
        /// </summary>
        /// <param name="account">Compte a ajouter</param>
        public void Add(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account), "Le compte ne peut pas etre null.");

            _accounts.Add(account);
            SaveChanges();
        }

        /// <summary>
        /// Met a jour un compte existant dans le repository
        /// </summary>
        /// <param name="account">Compte a mettre a jour</param>
        public void Update(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account), "Le compte ne peut pas etre null.");

            var existingAccount = _accounts.FirstOrDefault(a => a.AccountNumber == account.AccountNumber);

            if (existingAccount != null)
            {
                _accounts.Remove(existingAccount);
                _accounts.Add(account);
                SaveChanges();
            }
            else
            {
                // Si le compte n'existe pas, on l'ajoute
                Add(account);
            }
        }

        /// <summary>
        /// Recherche un compte par son numero
        /// </summary>
        /// <param name="accountNumber">Numero du compte a rechercher</param>
        /// <returns>Le compte trouve ou null si inexistant</returns>
        public Account GetByNumber(int accountNumber)
        {
            return _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
        }

        /// <summary>
        /// Retourne tous les comptes du repository
        /// </summary>
        /// <returns>Collection de tous les comptes</returns>
        public IEnumerable<Account> GetAll()
        {
            return _accounts;
        }

        /// <summary>
        /// Genere le prochain numero de compte unique
        /// </summary>
        /// <returns>Numero de compte incremental</returns>
        public int GetNextAccountNumber()
        {
            if (_accounts.Count == 0)
                return 1001;

            return _accounts.Max(a => a.AccountNumber) + 1;
        }

        /// <summary>
        /// Sauvegarde tous les comptes dans le fichier JSON
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                string json = JsonSerializer.Serialize(_accounts, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new AccountJsonConverter() }
                });

                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new IOException($"Erreur lors de la sauvegarde des comptes: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Charge les comptes depuis le fichier JSON
        /// </summary>
        /// <returns>Liste des comptes charges ou liste vide si le fichier n'existe pas</returns>
        private List<Account> LoadAccountsFromFile()
        {
            if (!File.Exists(_filePath))
                return new List<Account>();

            try
            {
                string json = File.ReadAllText(_filePath);

                if (string.IsNullOrWhiteSpace(json))
                    return new List<Account>();

                return JsonSerializer.Deserialize<List<Account>>(json, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new AccountJsonConverter() }
                }) ?? new List<Account>();
            }
            catch (Exception)
            {
                return new List<Account>();
            }
        }
    }
}
