using System;
using System.Linq;
using projetua3.Application.Services;
using projetua3.Domain.Exceptions;
using projetua3.Infrastructure;
using projetua3.Utils;

namespace projetua3
{
    /// <summary>
    /// Point d'entree principal de l'application CiteBanque
    /// Gere l'interface utilisateur et la coordination entre les services
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Methode Main - point d'entree de l'application
        /// </summary>
        /// <param name="args">Arguments de ligne de commande</param>
        private static void Main(string[] args)
        {
            // Initialisation des composants avec injection de dependances manuelle
            var accountRepository = new FileAccountRepository();
            var transactionLogger = new FileTransactionLogger();
            var fraudDetector = new BasicFraudDetector();

            // Creation du service principal avec injection de dependances
            var accountService = new AccountService(accountRepository, transactionLogger, fraudDetector);

            var menu = new ConsoleMenu();
            bool exit = false;

            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║   Bienvenue a CITEBANQUE                 ║");
            Console.WriteLine("║   Votre partenaire bancaire de confiance ║");
            Console.WriteLine("╚══════════════════════════════════════════╝\n");

            while (!exit)
            {
                menu.DisplayMenu();
                int choice = menu.GetMenuChoice();

                try
                {
                    switch (choice)
                    {
                        case 1:
                            CreateAccountMenu(accountService, menu);
                            break;
                        case 2:
                            ListAccountsMenu(accountService, menu);
                            break;
                        case 3:
                            SearchAccountMenu(accountService, menu);
                            break;
                        case 4:
                            DepositMenu(accountService, menu);
                            break;
                        case 5:
                            WithdrawMenu(accountService, menu);
                            break;
                        case 6:
                            TransferMenu(accountService, menu);
                            break;
                        case 7:
                            exit = true;
                            Console.WriteLine("\n╔══════════════════════════════════════════╗");
                            Console.WriteLine("║  Merci d'avoir utilise CiteBanque !      ║");
                            Console.WriteLine("║  A bientot !                             ║");
                            Console.WriteLine("╚══════════════════════════════════════════╝");
                            break;
                        default:
                            menu.DisplayError("Choix invalide. Veuillez entrer un numero entre 1 et 7.");
                            break;
                    }
                }
                catch (AccountNotFoundException ex)
                {
                    menu.DisplayError(ex.Message);
                }
                catch (InsufficientFundsException ex)
                {
                    menu.DisplayError(ex.Message);
                }
                catch (FraudDetectedException ex)
                {
                    menu.DisplayError(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    menu.DisplayError(ex.Message);
                }
                catch (Exception ex)
                {
                    menu.DisplayError($"Erreur inattendue : {ex.Message}");
                }

                if (!exit)
                {
                    menu.WaitForUser();
                    menu.ClearScreen();
                }
            }
        }

        /// <summary>
        /// Menu pour creer un nouveau compte bancaire
        /// </summary>
        /// <param name="accountService">Service de gestion des comptes</param>
        /// <param name="menu">Interface du menu console</param>
        private static void CreateAccountMenu(AccountService accountService, ConsoleMenu menu)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════╗");
            Console.WriteLine("║       Creation de compte                 ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");

            Console.Write("\nType de compte (Courant/Epargne) : ");
            string accountType = InputParser.ParseString(Console.ReadLine() ?? "");

            Console.Write("Nom du titulaire : ");
            string accountHolder = InputParser.ParseString(Console.ReadLine() ?? "");

            var account = accountService.CreateAccount(accountHolder, accountType);

            menu.DisplaySuccess("Compte cree avec succes !");
            Console.WriteLine($"\n  Numero de compte : {account.AccountNumber}");
            Console.WriteLine($"  Type             : {account.AccountType}");
            Console.WriteLine($"  Titulaire        : {account.OwnerName}");
            Console.WriteLine($"  Solde initial    : {account.Balance:C}");
        }

        /// <summary>
        /// Menu pour afficher la liste de tous les comptes
        /// </summary>
        /// <param name="accountService">Service de gestion des comptes</param>
        /// <param name="menu">Interface du menu console</param>
        private static void ListAccountsMenu(AccountService accountService, ConsoleMenu menu)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════╗");
            Console.WriteLine("║       Liste des comptes                  ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");

            var accounts = accountService.GetAllAccounts();

            if (accounts == null || !accounts.Any())
            {
                menu.DisplayInfo("Aucun compte trouve dans le systeme.");
                return;
            }

            Console.WriteLine($"\n{"Numero",-10} {"Type",-18} {"Titulaire",-25} {"Solde",15}");
            Console.WriteLine(new string('─', 70));

            foreach (var account in accounts)
            {
                Console.WriteLine($"{account.AccountNumber,-10} {account.AccountType,-18} {account.OwnerName,-25} {account.Balance,15:C}");
            }

            Console.WriteLine(new string('─', 70));
            menu.DisplayInfo($"Total : {accounts.Count()} compte(s) enregistre(s)");
        }

        /// <summary>
        /// Menu pour rechercher un compte specifique
        /// </summary>
        /// <param name="accountService">Service de gestion des comptes</param>
        /// <param name="menu">Interface du menu console</param>
        private static void SearchAccountMenu(AccountService accountService, ConsoleMenu menu)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════╗");
            Console.WriteLine("║       Recherche de compte                ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");

            Console.Write("\nNumero du compte : ");
            int accountNumber = InputParser.ParseInt(Console.ReadLine() ?? "");

            var account = accountService.GetAccount(accountNumber);

            menu.DisplaySuccess("Compte trouve !");
            Console.WriteLine($"\n  Numero de compte : {account.AccountNumber}");
            Console.WriteLine($"  Type             : {account.AccountType}");
            Console.WriteLine($"  Titulaire        : {account.OwnerName}");
            Console.WriteLine($"  Solde            : {account.Balance:C}");
            Console.WriteLine($"  Date de creation : {account.CreatedAt:dd/MM/yyyy HH:mm}");
        }

        /// <summary>
        /// Menu pour effectuer un depot sur un compte
        /// </summary>
        /// <param name="accountService">Service de gestion des comptes</param>
        /// <param name="menu">Interface du menu console</param>
        private static void DepositMenu(AccountService accountService, ConsoleMenu menu)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════╗");
            Console.WriteLine("║       Depot d'argent                     ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");

            Console.Write("\nNumero du compte : ");
            int accountNumber = InputParser.ParseInt(Console.ReadLine() ?? "");

            Console.Write("Montant a deposer : ");
            decimal amount = InputParser.ParseDecimal(Console.ReadLine() ?? "");

            accountService.Deposit(accountNumber, amount);

            var account = accountService.GetAccount(accountNumber);
            menu.DisplaySuccess("Depot effectue avec succes !");
            Console.WriteLine($"\n  Montant depose   : {amount:C}");
            Console.WriteLine($"  Nouveau solde    : {account.Balance:C}");
        }

        /// <summary>
        /// Menu pour effectuer un retrait sur un compte
        /// </summary>
        /// <param name="accountService">Service de gestion des comptes</param>
        /// <param name="menu">Interface du menu console</param>
        private static void WithdrawMenu(AccountService accountService, ConsoleMenu menu)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════╗");
            Console.WriteLine("║       Retrait d'argent                   ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");

            Console.Write("\nNumero du compte : ");
            int accountNumber = InputParser.ParseInt(Console.ReadLine() ?? "");

            Console.Write("Montant a retirer : ");
            decimal amount = InputParser.ParseDecimal(Console.ReadLine() ?? "");

            accountService.Withdraw(accountNumber, amount);

            var account = accountService.GetAccount(accountNumber);
            menu.DisplaySuccess("Retrait effectue avec succes !");
            Console.WriteLine($"\n  Montant retire   : {amount:C}");
            Console.WriteLine($"  Nouveau solde    : {account.Balance:C}");
        }

        /// <summary>
        /// Menu pour effectuer un transfert entre deux comptes
        /// </summary>
        /// <param name="accountService">Service de gestion des comptes</param>
        /// <param name="menu">Interface du menu console</param>
        private static void TransferMenu(AccountService accountService, ConsoleMenu menu)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════╗");
            Console.WriteLine("║       Transfert d'argent                 ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");

            Console.Write("\nNumero du compte source : ");
            int fromAccount = InputParser.ParseInt(Console.ReadLine() ?? "");

            Console.Write("Numero du compte destination : ");
            int toAccount = InputParser.ParseInt(Console.ReadLine() ?? "");

            Console.Write("Montant a transferer : ");
            decimal amount = InputParser.ParseDecimal(Console.ReadLine() ?? "");

            accountService.Transfer(fromAccount, toAccount, amount);

            var accountFrom = accountService.GetAccount(fromAccount);
            var accountTo = accountService.GetAccount(toAccount);

            menu.DisplaySuccess("Transfert effectue avec succes !");
            Console.WriteLine($"\n  Montant transfere : {amount:C}");
            Console.WriteLine($"  Solde source ({fromAccount})      : {accountFrom.Balance:C}");
            Console.WriteLine($"  Solde destination ({toAccount}) : {accountTo.Balance:C}");
        }
    }
}