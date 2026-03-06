using System;

namespace projetua3.Domain.Exceptions
{
    /// <summary>
    /// Exception levee lorsqu'une transaction est detectee comme suspecte ou frauduleuse
    /// Herite de la classe Exception
    /// </summary>
    public class FraudDetectedException : Exception
    {
        /// <summary>
        /// Montant de la transaction suspecte
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Constructeur de l'exception avec le montant suspect
        /// </summary>
        /// <param name="amount">Montant de la transaction detectee comme frauduleuse</param>
        public FraudDetectedException(decimal amount)
            : base($"Transaction refusee : le montant {amount:C} depasse le seuil autorise. Veuillez prendre rendez-vous avec un conseiller.")
        {
            Amount = amount;
        }

        /// <summary>
        /// Constructeur avec message personnalise
        /// </summary>
        /// <param name="amount">Montant de la transaction suspecte</param>
        /// <param name="message">Message d'erreur personnalise</param>
        public FraudDetectedException(decimal amount, string message)
            : base(message)
        {
            Amount = amount;
        }
    }
}