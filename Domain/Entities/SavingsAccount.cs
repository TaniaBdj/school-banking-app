using System;
using System.Text.Json.Serialization;
using projetua3.Domain.Exceptions;

namespace projetua3.Domain.Entities
{
    /// <summary>
    /// Compte epargne avec taux d'interet et aucun decouvert autorise
    /// Herite de la classe abstraite Account
    /// </summary>
    public class SavingsAccount : Account
    {
        /// <summary>
        /// Taux d'interet annuel du compte epargne (2.5% par defaut)
        /// </summary>
        public decimal InterestRate { get; set; } = 0.025m;

        /// <summary>
        /// Constructeur du compte epargne
        /// </summary>
        /// <param name="accountNumber">Numero du compte</param>
        /// <param name="ownerName">Nom du titulaire</param>
        public SavingsAccount(int accountNumber, string ownerName)
            : base(accountNumber, ownerName, "Compte Epargne")
        {
        }

        /// <summary>
        /// Constructeur vide pour la deserialisation JSON
        /// </summary>
        public SavingsAccount() : base()
        {
            InterestRate = 0.025m;
        }

        /// <summary>
        /// Effectue un retrait sans autorisation de decouvert
        /// Override de la methode abstraite de Account
        /// </summary>
        /// <param name="amount">Montant a retirer</param>
        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Le montant du retrait doit etre positif.");

            if (Balance < amount)
            {
                throw new InsufficientFundsException(
                    $"Retrait refuse : les comptes epargne n'autorisent pas le decouvert. " +
                    $"Solde actuel : {Balance:C}, montant demande : {amount:C}."
                );
            }
            Balance -= amount;
        }

        /// <summary>
        /// Applique les interets au solde du compte
        /// Calcule et ajoute les interets selon le taux defini
        /// </summary>
        /// <returns>Montant des interets appliques</returns>
        public decimal ApplyInterest()
        {
            decimal gain = Balance * InterestRate;
            Balance += gain;
            return gain;
        }

        /// <summary>
        /// Retourne une representation textuelle du compte epargne
        /// Inclut les informations sur le taux d'interet
        /// </summary>
        /// <returns>Description complete du compte avec taux d'interet</returns>
        public override string ToString()
        {
            return base.ToString() + $" | Taux d'interet : {InterestRate:P}";
        }
    }
}