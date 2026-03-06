namespace projetua3.Domain.Interfaces
{
    /// <summary>
    /// Interface pour l'enregistrement et le suivi des operations bancaires
    /// Respecte le principe de responsabilite unique (SRP) de SOLID
    /// </summary>
    public interface ITransactionLogger
    {
        /// <summary>
        /// Enregistre un message de transaction dans le systeme de logs
        /// </summary>
        /// <param name="message">Message decrivant l'operation effectuee</param>
        void Log(string message);
    }
}