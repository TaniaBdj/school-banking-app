using System;

namespace projetua3.Domain.Exceptions
{
    /// <summary>
    /// Exception levee lorsqu'un compte bancaire n'est pas trouve dans le systeme
    /// Herite de la classe Exception
    /// </summary>
    public class AccountNotFoundException : Exception
    {
        /// <summary>
        /// Numero du compte qui n'a pas ete trouve
        /// </summary>
        public int AccountNumber { get; }

        /// <summary>
        /// Constructeur de l'exception avec le numero de compte
        /// </summary>
        /// <param name="accountNumber">Numero du compte introuvable</param>
        public AccountNotFoundException(int accountNumber)
            : base($"Le compte numero {accountNumber} est introuvable.")
        {
            AccountNumber = accountNumber;
        }

        /// <summary>
        /// Constructeur avec message personnalise
        /// </summary>
        /// <param name="accountNumber">Numero du compte introuvable</param>
        /// <param name="message">Message d'erreur personnalise</param>
        public AccountNotFoundException(int accountNumber, string message)
            : base(message)
        {
            AccountNumber = accountNumber;
        }
    }
}
