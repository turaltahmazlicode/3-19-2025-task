using System.Text.RegularExpressions;

namespace ConsoleApp
{
    public class BankAccount
    {
        #region constructors
        public BankAccount()
        {
            _id = Guid.NewGuid();
            _accountNumber = _id.ToString()[..8];
            _ownerName = "Unknown";
            _balance = 0;
        }

        public BankAccount(string? ownerName) : this()
        {
            OwnerName = ownerName;
        }

        public BankAccount(Guid? id, string? accountNumber, string? ownerName, decimal balance)
        {
            _id = id ?? Guid.NewGuid();
            _accountNumber = accountNumber ?? _id.ToString()[..8];
            OwnerName = ownerName;
            Balance = balance;
        }
        #endregion

        #region fields
        private Guid? _id;
        private string? _accountNumber;
        private string? _ownerName;
        private decimal _balance;
        #endregion

        #region properties
        #region prop
        public Guid? Id => _id;
        public string? AccountNumber => _accountNumber;
        #endregion

        #region propfull
        public string? OwnerName
        {
            get => _ownerName;
            set
            {
                if (Regex.IsMatch(value, @"^[A-Za-z]+$"))
                {
                    _ownerName = char.ToUpper(value[0]) + value[1..].ToLower();
                    return;
                }
                throw new Exception("Invalid owner name. The name must contain only alphabetic characters.");
            }
        }

        public decimal Balance
        {
            get => _balance;
            private set
            {
                if (value >= 0)
                {
                    _balance = value;
                    return;
                }
                throw new Exception("Invalid balance. Balance cannot be negative.");
            }
        }
        #endregion
        #endregion

        #region methods
        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                return;
            }
            throw new Exception("Invalid amount. Amount must be greater than zero.");
        }

        public void Withdraw(decimal amount)
        {
            if (amount > 0 && Balance - amount >= 0)
            {
                Balance -= amount;
                return;
            }
            throw new Exception("Invalid amount. The amount must be greater than zero and not exceed the current balance.");
        }

        public void DisplayAccount()
        {
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            return $"ID: {Id}\nAccount number: {AccountNumber}\nOwner name: {OwnerName}\nBalance: {Balance}";
        }
        #endregion
    }
}
