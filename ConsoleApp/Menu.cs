using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ConsoleApp
{
    class Menu
    {
        public Menu()
        {
            choise = default;
            Console.WriteLine("This is your admin account info: \n");
            BankAccount admin = new("Tural");
            Console.WriteLine(admin);
            Console.WriteLine();
            Pause();
            Console.Clear();
            Console.WriteLine("Welcome to the Bank!");
            Console.WriteLine("Enter the name of the Bank: ");
            Bank = new Bank(Console.ReadLine(), [admin]);
        }

        public Bank Bank { get; private set; }

        char choise;

        BankAccount? currentAccount;

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

        private void LoginMenu()
        {
            Console.WriteLine("Enter the account number to log in");
            string? acountNumber = Console.ReadLine();
            currentAccount = Bank.GetAccountByAccountNumber(acountNumber);
            if (acountNumber == Bank.BankAccounts?[0].AccountNumber)
            {
                AdminMenu();
                return;
            }
            UserMenu();
        }

        private void AdminMenu()
        {
            while (true)
            {
                choise = SetChoise("1. Add bank account\n2. Add bank transaction\n3. Delete account\n4. Display account\n5. Display bank\n0. Exit");
                Console.Clear();
                try
                {
                    switch (choise)
                    {
                        case '1':
                            AddAccount();
                            break;
                        case '2':
                            AddTransaction();
                            break;
                        case '3':
                            DisplayAccountMenu();
                            break;
                        case '4':
                            DeleteAccountMenu();
                            break;
                        case '5':
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
            Bank.AddAccount(new BankAccount(ownerName));
        }

        private void AddTransaction()
        {
            Console.Write("Enter the sender account id: ");
            Guid? senderAccountNumber = Guid.Parse(Console.ReadLine());
            Console.Write("Enter the receiver account id: ");
            Guid? receiverAccountNumber = Guid.Parse(Console.ReadLine());
            Console.Write("Enter the amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Bank.ExecuteTransaction(new Transaction(senderAccountNumber, receiverAccountNumber, amount));
            }
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
            Bank.DeleteAccountById(Guid.Parse(Console.ReadLine()));
        }

        private void DeleteAccountByAccountNumber()
        {
            Console.Write("Enter the account number: ");
            Bank.DeleteAccountByAccountNumber(Console.ReadLine());
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
            Bank.GetAccountById(Guid.Parse(Console.ReadLine()));
        }

        private void DisplayAccountByAccountNumber()
        {
            Console.Write("Enter the account number: ");
            Bank.GetAccountByAccountNumber(Console.ReadLine());
        }

        private void DisplayAccountByOwnerName()
        {
            Console.Write("Enter the owner name: ");
            Bank.GetAccountByOwnerName(Console.ReadLine());
        }
        #endregion

        private void DisplayBank()
        {
            Bank.DisplayBank();
        }
        #endregion

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
            BankAccount? receiver = Bank.GetAccountById(receiverId);

            Console.Write("Enter the amount to transfer: ");
            Transaction transaction = new(currentAccount?.Id, receiverId, decimal.Parse(Console.ReadLine()));

            transaction.Execute(currentAccount, receiver);

            Bank.AddTransaction(transaction);
        }
        #endregion
    }
}