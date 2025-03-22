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
        #endregion

        #region fields
        private Guid? _id;
        private string? _accountNumber;
        private string? _ownerName;
        private decimal? _balance;
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
                if (Regex.IsMatch(value, @"^[A-Z][a-z]*$"))
                {
                    _ownerName = value;
                    return;
                }
                throw new Exception("Invalid owner name");
            }
        }

        public decimal? Balance
        {
            get => _balance;
            private set
            {
                if (value > 0)
                {
                    _balance = value;
                    return;
                }
                throw new Exception("Invalid balance");
            }
        }
        #endregion
        #endregion

        #region methods
        public void Deposit(decimal? amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                return;
            }
            throw new Exception("Invalid amount");
        }

        public void Withdraw(decimal? amount)
        {
            if (amount > 0 && Balance - amount >= 0)
            {
                Balance -= amount;
                return;
            }
            throw new Exception("Invalid amount");
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
