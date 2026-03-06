using System;
using System.Collections.Generic;
using projetua3.Domain.Entities;
using projetua3.Domain.Interfaces;

namespace projetua3.Application.Services
{
    /// <summary>
    /// Service qui gere les operations sur les comptes bancaires
    /// </summary>
    public class AccountService
    {
        private readonly IAccountRepository _repository;
        private readonly ITransactionLogger _logger;
        private readonly IFraudDetector _fraudDetector;

        /// <summary>
        /// Constructeur du service de gestion de comptes
        /// </summary>
        /// <param name="repository">Repository pour le stockage des comptes</param>
        /// <param name="logger">Logger pour enregistrer les transactions</param>
        /// <param name="fraudDetector">Detecteur de fraude</param>
        public AccountService(IAccountRepository repository, ITransactionLogger logger, IFraudDetector fraudDetector)
        {
            _repository = repository;
            _logger = logger;
            _fraudDetector = fraudDetector;
        }

        /// <summary>
        /// Cree un nouveau compte bancaire (courant ou epargne)
        /// </summary>
        /// <param name="ownerName">Nom du titulaire du compte</param>
        /// <param name="accountType">Type de compte (courant ou epargne)</param>
        /// <returns>Le compte nouvellement cree</returns>
        public Account CreateAccount(string ownerName, string accountType)
        {
            if (string.IsNullOrWhiteSpace(ownerName))
                throw new ArgumentException("Nom du titulaire invalide");

            int newNumber = _repository.GetNextAccountNumber();
            Account account;

            if (accountType.ToLower() == "courant" || accountType.ToLower() == "current")
                account = new CurrentAccount(newNumber, ownerName);
            else if (accountType.ToLower() == "epargne" || accountType.ToLower() == "savings")
                account = new SavingsAccount(newNumber, ownerName);
            else
                throw new ArgumentException("Type de compte invalide");

            _repository.Add(account);
            _logger.Log($"Compte {newNumber} cree pour {ownerName} ({accountType})");
            return account;
        }

        /// <summary>
        /// Retourne la liste de tous les comptes bancaires
        /// </summary>
        /// <returns>Collection de tous les comptes</returns>
        public IEnumerable<Account> GetAllAccounts()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Recherche un compte par son numero
        /// </summary>
        /// <param name="accountNumber">Numero du compte a rechercher</param>
        /// <returns>Le compte trouve</returns>
        public Account GetAccount(int accountNumber)
        {
            var account = _repository.GetByNumber(accountNumber);
            if (account == null)
                throw new Exception($"Compte {accountNumber} introuvable");
            return account;
        }

        /// <summary>
        /// Effectue un depot sur un compte
        /// </summary>
        /// <param name="accountNumber">Numero du compte</param>
        /// <param name="amount">Montant a deposer</param>
        public void Deposit(int accountNumber, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Le montant du depot doit etre positif");

            var account = GetAccount(accountNumber);

            // Verification de fraude AVANT le depot
            if (_fraudDetector.IsFraud(amount))
                throw new Exception("Transaction refusee. Montant suspect detecte. Veuillez prendre rendez-vous avec un conseiller.");

            account.Deposit(amount);
            _logger.Log($"Depot de {amount:C} sur le compte {accountNumber}");
            _repository.Update(account);
        }

        /// <summary>
        /// Effectue un retrait sur un compte
        /// </summary>
        /// <param name="accountNumber">Numero du compte</param>
        /// <param name="amount">Montant a retirer</param>
        public void Withdraw(int accountNumber, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Le montant du retrait doit etre positif");

            var account = GetAccount(accountNumber);

            if (_fraudDetector.IsFraud(amount))
                throw new Exception("Transaction refusee. Montant suspect detecte. Veuillez prendre rendez-vous avec un conseiller.");

            account.Withdraw(amount);
            _logger.Log($"Retrait de {amount:C} sur le compte {accountNumber}");
            _repository.Update(account);
        }

        /// <summary>
        /// Transfere de l'argent d'un compte vers un autre
        /// </summary>
        /// <param name="fromAccount">Numero du compte source</param>
        /// <param name="toAccount">Numero du compte destination</param>
        /// <param name="amount">Montant a transferer</param>
        public void Transfer(int fromAccount, int toAccount, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Le montant du transfert doit etre positif");

            var src = GetAccount(fromAccount);
            var dest = GetAccount(toAccount);

            if (_fraudDetector.IsFraud(amount))
                throw new Exception("Transaction refusee. Montant suspect detecte. Veuillez prendre rendez-vous avec un conseiller.");

            src.Withdraw(amount);
            dest.Deposit(amount);

            _logger.Log($"Transfert de {amount:C} du compte {fromAccount} vers {toAccount}");

            _repository.Update(src);
            _repository.Update(dest);
        }
    }
}
