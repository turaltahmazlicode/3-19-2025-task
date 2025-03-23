namespace ConsoleApp
{
    class Menu
    {
        private const string AdminFilePath = @"..\..\..\admin.json";
        private const string BankFilePath = @"..\..\..\bank.json";

        public Menu()
        {
            Console.ForegroundColor = ConsoleColor.White;
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
                Console.Write("Welcome to the ");
                SetConsoleColor(ConsoleColor.Blue, () => Console.Write(bank.Name));
                Console.WriteLine("!");
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

        #region static methods
        private static void Pause()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private static char SetChoise(string? info)
        {
            Console.Clear();
            SetConsoleColor(ConsoleColor.White, () => Console.WriteLine(info));
            SetConsoleColor(ConsoleColor.Yellow, () => Console.Write("Your choice: "));
            return Console.ReadKey().KeyChar;
        }

        private static void DisplayExceptionMsg(string? text)
        {
            SetConsoleColor(ConsoleColor.Red, () => Console.WriteLine(text));
            SetConsoleColor(ConsoleColor.Green, Pause);
        }

        private static void SetConsoleColor(ConsoleColor color, Action action)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            action();
            Console.ForegroundColor = originalColor;
        }
        #endregion

        #region menu
        public void StartMenu()
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
                            RegisterMenu();
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
            SetConsoleColor(ConsoleColor.White, () => Console.Write("Enter the account number to log in: "));
            string? acountNumber = Console.ReadLine();
            currentAccount = bank.GetAccountByAccountNumber(acountNumber);
            if (acountNumber == admin.AccountNumber)
            {
                AdminMenu();
                return;
            }
            if (currentAccount is null)
                throw new Exception("Account could not be found.");
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
                            break;
                        case '4':
                            DisplayBank();
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
            SetConsoleColor(ConsoleColor.White, () => Console.Write("Enter the owner name: "));
            string? ownerName = Console.ReadLine();
            bank.AddAccount(new BankAccount(ownerName));
        }

        private void DeleteAccountMenu()
        {
            while (true)
            {
                choise = SetChoise("1. Delete by account ID\n2. Delete by account number\n3. Delete by owner name\n0. Exit");
                Console.Clear();
                switch (choise)
                {
                    case '1':
                        DeleteAccountById();
                        return;
                    case '2':
                        DeleteAccountByAccountNumber();
                        return;
                    case '3':
                        DeleteAccountByOwnerName();
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
            SetConsoleColor(ConsoleColor.White, () => Console.Write("Enter the account id: "));
            bank.DeleteAccountById(Guid.Parse(Console.ReadLine()));
        }

        private void DeleteAccountByAccountNumber()
        {
            SetConsoleColor(ConsoleColor.White, () => Console.Write("Enter the account number: "));
            bank.DeleteAccountByAccountNumber(Console.ReadLine());
        }

        private void DeleteAccountByOwnerName()
        {
            SetConsoleColor(ConsoleColor.White, () => Console.Write("Enter the owner name: "));
            bank.DeleteAccountByOwnerName(Console.ReadLine());
        }
        #endregion

        private void DisplayAccountMenu()
        {
            choise = SetChoise("1. Display by account ID\n2. Display by account number\n3. Display account by owner name\n0. Exit");
            Console.Clear();
            switch (choise)
            {
                case '1':
                    DisplayAccountById();
                    break;
                case '2':
                    DisplayAccountByAccountNumber();
                    break;
                case '3':
                    DisplayAccountByOwnerName();
                    break;
                default:
                    break;
            }
        }

        #region display account menu methods
        private void DisplayAccount(string accInfo)
        {
            SetConsoleColor(ConsoleColor.Green, () => Console.WriteLine(accInfo ?? throw new Exception("Account couldn't be found")));
            Pause();
        }

        private void DisplayAccountById()
        {
            SetConsoleColor(ConsoleColor.White, () => Console.Write("Enter the account id: "));
            DisplayAccount(bank.GetAccountById(Guid.Parse(Console.ReadLine())).ToString());
        }

        private void DisplayAccountByAccountNumber()
        {
            SetConsoleColor(ConsoleColor.White, () => Console.Write("Enter the account number: "));
            DisplayAccount(bank.GetAccountByAccountNumber(Console.ReadLine()).ToString());
        }

        private void DisplayAccountByOwnerName()
        {
            SetConsoleColor(ConsoleColor.White, () => Console.Write("Enter the owner name: "));
            DisplayAccount(bank.GetAccountByOwnerName(Console.ReadLine()).ToString());
        }
        #endregion

        private void DisplayBank()
        {
            SetConsoleColor(ConsoleColor.Green, bank.DisplayBank);
            Pause();
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
            SetConsoleColor(ConsoleColor.White, () => Console.Write("Enter the amount to deposit: "));
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                currentAccount?.Deposit(amount);
        }

        private void Withdraw()
        {
            SetConsoleColor(ConsoleColor.White, () => Console.Write("Enter the amount to withdraw: "));
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                currentAccount?.Withdraw(amount);
        }

        private void DisplayAccount()
        {
            SetConsoleColor(ConsoleColor.Green, currentAccount.DisplayAccount);
        }

        private void ChangeOwnerName()
        {
            SetConsoleColor(ConsoleColor.White, () => Console.Write("Enter the new owner name: "));
            currentAccount.OwnerName = Console.ReadLine();
        }

        private void Transfer()
        {
            SetConsoleColor(ConsoleColor.White, () => Console.Write("Enter the receiver account ID: "));
            Guid? receiverId = Guid.Parse(Console.ReadLine());
            BankAccount? receiver = bank.GetAccountById(receiverId);

            SetConsoleColor(ConsoleColor.White, () => Console.Write("Enter the amount to transfer: "));
            Transaction transaction = new(currentAccount?.Id, receiverId, decimal.Parse(Console.ReadLine()));

            transaction.Execute(currentAccount, receiver);

            bank.AddTransaction(transaction);
        }
        #endregion
        #endregion
        #endregion

        #region register menu
        private void RegisterMenu()
        {
            AddAccount();
        }
        #endregion 
        #endregion
    }
}