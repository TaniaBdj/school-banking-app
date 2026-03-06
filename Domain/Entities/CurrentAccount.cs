using System;
using System.Text.Json.Serialization;
using projetua3.Domain.Exceptions;

namespace projetua3.Domain.Entities
{
    /// <summary>
    /// Compte courant avec autorisation de decouvert
    /// Herite de la classe abstraite Account
    /// </summary>
    public class CurrentAccount : Account
    {
        /// <summary>
        /// Limite de decouvert autorisee (montant negatif)
        /// </summary>
        public decimal OverdraftLimit { get; set; } = -500m;

        /// <summary>
        /// Constructeur du compte courant
        /// </summary>
        /// <param name="accountNumber">Numero du compte</param>
        /// <param name="ownerName">Nom du titulaire</param>
        public CurrentAccount(int accountNumber, string ownerName)
            : base(accountNumber, ownerName, "Compte Courant")
        {
        }

        /// <summary>
        /// Constructeur vide pour la deserialisation JSON
        /// </summary>
        public CurrentAccount() : base()
        {
            OverdraftLimit = -500m;
        }

        /// <summary>
        /// Effectue un retrait avec verification du decouvert autorise
        /// Override de la methode abstraite de Account
        /// </summary>
        /// <param name="amount">Montant a retirer</param>
        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Le montant du retrait doit etre positif.");

            decimal futureBalance = Balance - amount;
            if (futureBalance < OverdraftLimit)
            {
                throw new InsufficientFundsException(
                    $"Retrait refuse : decouvert maximum {OverdraftLimit:C}. " +
                    $"Solde actuel : {Balance:C}, retrait demande : {amount:C}."
                );
            }
            Balance -= amount;
        }

        /// <summary>
        /// Retourne une representation textuelle du compte courant
        /// Inclut les informations de decouvert
        /// </summary>
        /// <returns>Description complete du compte avec decouvert</returns>
        public override string ToString()
        {
            return base.ToString() + $" | Decouvert autorise : {OverdraftLimit:C}";
        }
    }
}