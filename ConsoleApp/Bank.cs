using System.Text;

namespace ConsoleApp
{
    class Bank
    {
        #region constructors
        public Bank()
        {
            _id = Guid.NewGuid();
            Name = "Default Bank";
            _bankAccounts = [];
            _transactions = [];
        }

        public Bank(string? name) : this() => Name = name;

        public Bank(string? name, BankAccount[]? bankAccounts) : this(name) => _bankAccounts = bankAccounts ?? [];
        #endregion

        #region fields
        private Guid? _id;
        private BankAccount[]? _bankAccounts;
        private Transaction[]? _transactions;
        #endregion

        #region properties
        #region prop
        public Guid? Id => _id;
        public string? Name { get; set; }
        public int AccountsCount => BankAccounts?.Length ?? 0;
        public int TransactionCount => Transactions?.Length ?? 0;
        #endregion

        #region propfull
        public BankAccount[]? BankAccounts
        {
            get => _bankAccounts;
            private set => _bankAccounts = value ?? [];
        }

        public Transaction[]? Transactions
        {
            get => _transactions;
            private set => _transactions = value ?? [];
        }
        #endregion
        #endregion

        #region methods
        public void AddAccount(BankAccount? bankaccount)
        {
            _bankAccounts ??= [];
            Array.Resize(ref _bankAccounts, BankAccounts.Length + 1);
            BankAccounts[^1] = bankaccount;
        }

        public void AddTransaction(Transaction? transaction)
        {
            _transactions ??= [];
            Array.Resize(ref _transactions, Transactions.Length + 1);
            Transactions[^1] = transaction;
        }

        public BankAccount? GetAccountBy(Func<BankAccount?, bool>? predicate)
        {
            if (BankAccounts is null || predicate is null)
                return null;
            foreach (var bankAccount in BankAccounts)
                if (predicate(bankAccount))
                    return bankAccount;
            return null;
        }

        public BankAccount? GetAccountById(Guid? id) => GetAccountBy(account => account?.Id == id);

        public BankAccount? GetAccountByAccountNumber(string? accountNumber) => GetAccountBy(account => account?.AccountNumber == accountNumber);

        public BankAccount? GetAccountByOwnerName(string? ownerName) => GetAccountBy(account => account?.OwnerName == ownerName);

        private void DeleteAccountBy(Func<BankAccount?, bool>? predicate)
        {
            if (BankAccounts is null || predicate is null)
                return;
            for (int i = 0; i < BankAccounts.Length; i++)
                if (predicate(BankAccounts[i]))
                {
                    BankAccounts[i] = BankAccounts[^1];
                    Array.Resize(ref _bankAccounts, BankAccounts.Length - 1);
                    return;
                }
            throw new Exception("Account not found");
        }

        public void DeleteAccountById(Guid? id) => DeleteAccountBy(account => account?.Id == id);

        public void DeleteAccountByAccountNumber(string? accountNumber) => DeleteAccountBy(account => account?.AccountNumber == accountNumber);

        public void ExecuteTransaction(Transaction? transaction)
        {
            if (transaction is null)
                return;
            BankAccount? sender = GetAccountById(transaction.SenderId), receiver = GetAccountById(transaction.ReceiverId);
            transaction.Execute(sender, receiver);
            AddTransaction(transaction);
        }

        public void DisplayBank()
        {
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"Bank ID: {Id}");
            sb.AppendLine($"Bank name: {Name}");
            sb.AppendLine($"Bank has {AccountsCount} accounts and {TransactionCount} transactions");
            if (BankAccounts != null)
            {
                foreach (var account in BankAccounts)
                    sb.AppendLine(account.ToString());
            }
            if (Transactions != null)
            {
                foreach (var transaction in Transactions)
                    sb.AppendLine(transaction.ToString());
            }
            return sb.ToString();
        }
        #endregion
    }
}
