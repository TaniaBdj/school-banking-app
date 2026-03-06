using System;

namespace projetua3.Domain.Exceptions
{
    /// <summary>
    /// Exception levee lorsqu'une operation depasse le solde disponible du compte
    /// Herite de la classe Exception
    /// </summary>
    public class InsufficientFundsException : Exception
    {
        /// <summary>
        /// Constructeur de l'exception avec message personnalise
        /// </summary>
        /// <param name="message">Message d'erreur decrivant le probleme de solde</param>
        public InsufficientFundsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructeur par defaut avec message generique
        /// </summary>
        public InsufficientFundsException()
            : base("Solde insuffisant pour effectuer cette operation.")
        {
        }

        /// <summary>
        /// Constructeur avec montant requis et solde disponible
        /// </summary>
        /// <param name="requestedAmount">Montant demande pour l'operation</param>
        /// <param name="availableBalance">Solde disponible sur le compte</param>
        public InsufficientFundsException(decimal requestedAmount, decimal availableBalance)
            : base($"Solde insuffisant. Montant demande : {requestedAmount:C}, Solde disponible : {availableBalance:C}")
        {
        }
    }
}