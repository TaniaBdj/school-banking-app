using System;
using projetua3.Domain.Interfaces;

namespace projetua3.Infrastructure
{
    /// <summary>
    /// Implementation basique de la detection de fraude par seuil de montant
    /// </summary>
    public class BasicFraudDetector : IFraudDetector
    {
        private readonly decimal _fraudThreshold;

        /// <summary>
        /// Constructeur avec seuil de detection de fraude configurable
        /// </summary>
        /// <param name="fraudThreshold">Seuil a partir duquel une transaction est consideree suspecte (defaut: 5000)</param>
        public BasicFraudDetector(decimal fraudThreshold = 5000m)
        {
            _fraudThreshold = fraudThreshold;
        }

        /// <summary>
        /// Determine si un montant depasse le seuil de fraude
        /// </summary>
        /// <param name="amount">Montant de la transaction a verifier</param>
        /// <returns>True si le montant est superieur ou egal au seuil, False sinon</returns>
        public bool IsFraud(decimal amount)
        {
            return amount >= _fraudThreshold;
        }
    }
}