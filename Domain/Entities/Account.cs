using System;
using System.Text.Json.Serialization;

namespace projetua3.Domain.Entities
{
    /// <summary>
    /// Classe abstraite de base pour tous les types de comptes bancaires
    /// Respecte le principe SRP (Single Responsibility Principle)
    /// </summary>
    public abstract class Account
    {
        /// <summary>
        /// Numero unique du compte bancaire
        /// </summary>
        public int AccountNumber { get; set; }

        /// <summary>
        /// Nom du titulaire du compte
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Solde actuel du compte - Propriété interne protégée
        /// </summary>
        [JsonIgnore]
        public decimal Balance { get; protected set; }

        /// <summary>
        /// Propriété publique pour la sérialisation JSON du solde
        /// </summary>
        [JsonPropertyName("Balance")]
        public decimal BalanceForJson
        {
            get => Balance;
            set => Balance = value;
        }

        /// <summary>
        /// Date et heure de creation du compte
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Type de compte (Courant, Epargne, etc.)
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        /// Constructeur protege pour initialiser un compte bancaire
        /// UTILISE PAR LES CLASSES DERIVEES
        /// </summary>
        protected Account(int accountNumber, string ownerName, string accountType)
        {
            AccountNumber = accountNumber;
            OwnerName = ownerName;
            AccountType = accountType;
            Balance = 0;
            CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// Constructeur sans paramètres pour la désérialisation JSON
        /// UTILISE PAR JsonSerializer
        /// </summary>
        protected Account()
        {
            AccountNumber = 0;
            OwnerName = string.Empty;
            AccountType = string.Empty;
            Balance = 0;
            CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// Effectue un depot d'argent sur le compte
        /// </summary>
        public virtual bool Deposit(decimal amount)
        {
            if (amount <= 0)
                return false;
            Balance += amount;
            return true;
        }

        /// <summary>
        /// Effectue un retrait d'argent du compte
        /// </summary>
        public abstract void Withdraw(decimal amount);

        /// <summary>
        /// Verifie si un retrait est possible selon le solde disponible
        /// </summary>
        protected bool CanWithdraw(decimal amount)
        {
            return amount > 0 && Balance >= amount;
        }

        /// <summary>
        /// Retourne une representation textuelle du compte
        /// </summary>
        public override string ToString()
        {
            return $"[{AccountType}] #{AccountNumber} | Titulaire: {OwnerName} | Solde: {Balance:C}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Account other)
                return AccountNumber == other.AccountNumber;
            return false;
        }

        public override int GetHashCode()
        {
            return AccountNumber.GetHashCode();
        }
    }
}