namespace ConsoleApp
{
    internal class Program
    {
        static void ExceptionMsg(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void Main(string[] args)
        {

            BankAccount account1 = new BankAccount(0, "12");
            BankAccount account2 = new BankAccount(0, "22");
            Bank bank = new Bank([account1, account2]);
            while (true)
            {
                Console.WriteLine("Enter the account number to log in");
                string accountNumber = Console.ReadLine();
                BankAccount account = bank.getAccountByAccountNumber(accountNumber);
                if (account is not null)
                {
                    char choise = default;
                    while (choise != '`')
                    {
                        Console.WriteLine("1. Deposit\r\n2. Withdraw\r\n3. AccountInfo\r\n4. Change OwnerName\r\n5. P2P\r\n`. Log out");
                        choise = Console.ReadKey().KeyChar;
                        Console.Write('\b');
                        switch (choise)
                        {
                            case '1':
                                {
                                    Console.WriteLine("Enter the amount of deposit.");
                                    try
                                    {
                                        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                                        {
                                            account.Deposit(amount);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        ExceptionMsg("Amount is not valid. It should be posetive number.");
                                    }
                                    break;
                                }
                            case '2':
                                {
                                    try
                                    {
                                        Console.WriteLine("Enter the amount of withdraw.");
                                        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                                            account.Withdraw(amount);
                                    }
                                    catch (Exception)
                                    {
                                        ExceptionMsg("Amount is not valid. It should be posetive number.");
                                    }
                                    break;
                                }
                            case '3':
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    account.DisplayAccountInfo();
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    break;
                                }
                            case '4':
                                {
                                    try
                                    {
                                        account.OwnerName = Console.ReadLine();
                                    }
                                    catch (Exception)
                                    {
                                        ExceptionMsg("Owner name should starts with capital letter and can't contain any number or symbol.");
                                    }
                                    break;
                                }
                            case '5':
                                {
                                    try
                                    {
                                        Console.WriteLine("Enter the account number to deposit.");
                                        string otherAccountNumber = Console.ReadLine();
                                        BankAccount bankAccount = bank.getAccountByAccountNumber(otherAccountNumber);
                                        if (bankAccount is not null)
                                        {
                                            Console.WriteLine("Enter the amount to send.");
                                            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                                            {
                                                account.Withdraw(amount);
                                                bankAccount.Deposit(amount);
                                            }
                                        }
                                        else
                                        {
                                            ExceptionMsg("Account don't exist.");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        ExceptionMsg("Amount is not valid. It should be posetive number.");
                                    }
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
