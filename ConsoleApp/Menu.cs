using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace ConsoleApp
{
    class Menu
    {
        private const string AdminFilePath = @"..\..\..\admin.json";
        private const string BankFilePath = @"..\..\..\bank.json";

        public Menu()
        {
            choise = default;

            if (File.Exists(AdminFilePath))
                admin = SerializationHelper.ReadFromFile<BankAccount>(AdminFilePath);
            else
            {
                admin = new BankAccount("Tural");
                SerializationHelper.WriteToFile(AdminFilePath, admin);
            }

            if (File.Exists(BankFilePath))
            {
                bank = SerializationHelper.ReadFromFile<Bank>(BankFilePath);
                Console.WriteLine($"Welcome to the {bank.Name}!");
                Pause();

            }
            else
            {
                Console.WriteLine("Welcome to the Bank!");
                Console.Write("Enter the name of the Bank: ");
                bank = new Bank(Console.ReadLine());
            }
        }

        char choise;
        Bank bank;
        BankAccount? currentAccount;
        BankAccount? admin;

        private static void Pause()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private static char SetChoise(string? info)
        {
            Console.Clear();
            Console.WriteLine(info);
            return Console.ReadKey().KeyChar;
        }

        private static void DisplayExceptionMsg(string? text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
            Pause();
        }

        public void Start()
        {
            AppDomain.CurrentDomain.ProcessExit += (s, e) => SerializationHelper.WriteToFile(BankFilePath, bank);
            while (true)
            {
                try
                {
                    choise = SetChoise("1. Login\n2. Register\n0. Exit");
                    Console.Clear();
                    switch (choise)
                    {
                        case '1':
                            LoginMenu();
                            break;
                        case '2':
                            //RegisterMenu();
                            break;
                        case '0':
                            return;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    DisplayExceptionMsg(e.Message);
                }
            }
        }

        #region login menu
        private void LoginMenu()
        {
            Console.WriteLine("Enter the account number to log in");
            string? acountNumber = Console.ReadLine();
            currentAccount = bank.GetAccountByAccountNumber(acountNumber);
            if (acountNumber == admin.AccountNumber)
            {
                AdminMenu();
                return;
            }
            if (currentAccount is not null)
                UserMenu();
        }

        #region admin menu
        private void AdminMenu()
        {
            while (true)
            {
                choise = SetChoise("1. Add bank account\n2. Delete account\n3. Display account\n4. Display bank\n0. Exit");
                Console.Clear();
                try
                {
                    switch (choise)
                    {
                        case '1':
                            AddAccount();
                            break;
                        case '2':
                            DeleteAccountMenu();
                            break;
                        case '3':
                            DisplayAccountMenu();
                            Pause();
                            break;
                        case '4':
                            DisplayBank();
                            Pause();
                            break;
                        case '0':
                            return;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    DisplayExceptionMsg(e.Message);
                }
            }
        }

        #region admin panel menu methods
        private void AddAccount()
        {
            Console.Write("Enter the owner name: ");
            string? ownerName = Console.ReadLine();
            bank.AddAccount(new BankAccount(ownerName));
        }

        private void DeleteAccountMenu()
        {
            while (true)
            {
                choise = SetChoise("1. Delete by account ID\n2. Delete by account number\n0. Exit");
                Console.Clear();
                switch (choise)
                {
                    case '1':
                        DeleteAccountById();
                        return;
                    case '2':
                        DeleteAccountByAccountNumber();
                        return;
                    case '0':
                        return;
                    default:
                        break;
                }
            }
        }

        #region delete account menu methods
        private void DeleteAccountById()
        {
            Console.Write("Enter the account id: ");
            bank.DeleteAccountById(Guid.Parse(Console.ReadLine()));
        }

        private void DeleteAccountByAccountNumber()
        {
            Console.Write("Enter the account number: ");
            bank.DeleteAccountByAccountNumber(Console.ReadLine());
        }
        #endregion

        private void DisplayAccountMenu()
        {
            while (true)
            {
                choise = SetChoise("1. Display by account ID\n2. Display by account number\n3. Display account by owner name\n0. Exit");
                Console.Clear();
                switch (choise)
                {
                    case '1':
                        DisplayAccountById();
                        return;
                    case '2':
                        DisplayAccountByAccountNumber();
                        return;
                    case '3':
                        DisplayAccountByOwnerName();
                        return;
                    case '0':
                        return;
                    default:
                        break;
                }
            }
        }

        #region display account menu methods
        private void DisplayAccountById()
        {
            Console.Write("Enter the account id: ");
            bank.GetAccountById(Guid.Parse(Console.ReadLine()));
        }

        private void DisplayAccountByAccountNumber()
        {
            Console.Write("Enter the account number: ");
            bank.GetAccountByAccountNumber(Console.ReadLine());
        }

        private void DisplayAccountByOwnerName()
        {
            Console.Write("Enter the owner name: ");
            bank.GetAccountByOwnerName(Console.ReadLine());
        }
        #endregion

        private void DisplayBank()
        {
            bank.DisplayBank();
        }
        #endregion 
        #endregion

        #region user menu
        private void UserMenu()
        {
            while (true)
            {
                choise = SetChoise("1. Deposit\n2. Withdraw\n3. Display account\n4. Change owner name\n5. Transfer\n0. Exit");
                Console.Clear();
                try
                {
                    switch (choise)
                    {
                        case '1':
                            Deposit();
                            break;
                        case '2':
                            Withdraw();
                            break;
                        case '3':
                            DisplayAccount();
                            Pause();
                            break;
                        case '4':
                            ChangeOwnerName();
                            break;
                        case '5':
                            Transfer();
                            break;
                        case '0':
                            return;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    DisplayExceptionMsg(e.Message);
                }
            }
        }

        #region user menu methods
        private void Deposit()
        {
            Console.Write("Enter the amount to deposit: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                currentAccount?.Deposit(amount);
        }

        private void Withdraw()
        {
            Console.Write("Enter the amount to withdraw: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                currentAccount?.Withdraw(amount);
        }

        private void DisplayAccount()
        {
            currentAccount?.DisplayAccount();
        }

        private void ChangeOwnerName()
        {
            currentAccount.OwnerName = Console.ReadLine();
        }

        private void Transfer()
        {
            Console.Write("Enter the receiver account ID: ");
            Guid? receiverId = Guid.Parse(Console.ReadLine());
            BankAccount? receiver = bank.GetAccountById(receiverId);

            Console.Write("Enter the amount to transfer: ");
            Transaction transaction = new(currentAccount?.Id, receiverId, decimal.Parse(Console.ReadLine()));

            transaction.Execute(currentAccount, receiver);

            bank.AddTransaction(transaction);
        }

        #endregion
        #endregion
        #endregion
    }
}