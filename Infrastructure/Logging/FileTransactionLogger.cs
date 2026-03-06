using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using projetua3.Domain.Interfaces;

namespace projetua3.Infrastructure
{
    /// <summary>
    /// Implementation du logger de transactions avec sauvegarde dans un fichier JSON
    /// Respecte le principe de responsabilite unique (SRP) de SOLID
    /// </summary>
    public class FileTransactionLogger : ITransactionLogger
    {
        /// <summary>
        /// Classe interne pour representer une transaction dans le fichier JSON
        /// </summary>
        public class SimpleTransaction
        {
            /// <summary>
            /// Montant de la transaction
            /// </summary>
            public decimal Amount { get; set; }

            /// <summary>
            /// Description de l'operation effectuee
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Date et heure de la transaction
            /// </summary>
            public DateTime Date { get; set; }
        }

        private readonly string _filePath;

        /// <summary>
        /// Constructeur avec chemin de fichier configurable
        /// </summary>
        /// <param name="filePath">Chemin du fichier JSON pour stocker les logs (defaut: transactions.json)</param>
        public FileTransactionLogger(string filePath = "transactions.json")
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Enregistre un message de transaction simple
        /// Implementation de l'interface ITransactionLogger
        /// </summary>
        /// <param name="message">Message a enregistrer</param>
        public void Log(string message)
        {
            // Pour l'instant, on affiche simplement dans la console
            // TODO: Ameliorer pour aussi ecrire dans le fichier JSON
            Console.WriteLine($"[LOG] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }

        /// <summary>
        /// Enregistre une transaction detaillee dans le fichier JSON
        /// </summary>
        /// <param name="amount">Montant de la transaction</param>
        /// <param name="description">Description de l'operation</param>
        public void LogTransaction(decimal amount, string description = "")
        {
            var transaction = new SimpleTransaction
            {
                Amount = amount,
                Description = description,
                Date = DateTime.Now
            };

            List<SimpleTransaction> transactions = ReadAllTransactions();
            transactions.Add(transaction);

            string json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        /// <summary>
        /// Lit toutes les transactions enregistrees dans le fichier JSON
        /// </summary>
        /// <returns>Liste de toutes les transactions ou liste vide si le fichier n'existe pas</returns>
        public List<SimpleTransaction> ReadAllTransactions()
        {
            if (!File.Exists(_filePath))
                return new List<SimpleTransaction>();

            string json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
                return new List<SimpleTransaction>();

            return JsonSerializer.Deserialize<List<SimpleTransaction>>(json) ?? new List<SimpleTransaction>();
        }
    }
}
