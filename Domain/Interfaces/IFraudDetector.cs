namespace projetua3.Domain.Interfaces
{
    /// <summary>
    /// Interface pour la detection des transactions suspectes ou frauduleuses
    /// Respecte le principe de responsabilite unique (SRP) de SOLID
    /// </summary>
    public interface IFraudDetector
    {
        /// <summary>
        /// Determine si un montant de transaction est considere comme frauduleux
        /// </summary>
        /// <param name="amount">Montant de la transaction a verifier</param>
        /// <returns>True si la transaction est suspecte, False sinon</returns>
        bool IsFraud(decimal amount);
    }
}