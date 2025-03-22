namespace ConsoleApp
{
    public record Transaction
    {
        #region constructors
        private Transaction()
        {
            TransactionId = Guid.NewGuid();
            ExecutionDate = null;
        }

        public Transaction(Guid? senderId, Guid? receiverId, decimal? amount) : this()
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Amount = amount;
        }
        #endregion

        #region fields
        decimal? _amount;
        #endregion

        #region properties
        #region prop
        public Guid? TransactionId { get; private set; }
        public Guid? SenderId { get; private set; }
        public Guid? ReceiverId { get; private set; }
        public DateTime? ExecutionDate { get; private set; }
        #endregion

        #region propfull
        public decimal? Amount
        {
            get => _amount;
            set
            {
                if (value > 0)
                {
                    _amount = value;
                    return;
                }
                throw new Exception("Invalid amount");
            }
        }
        #endregion
        #endregion

        #region methods
        public void Execute(BankAccount? sender, BankAccount? receiver)
        {
            if (ExecutionDate is not null)
                throw new Exception("Transaction is already executed");
            if (sender is null || receiver is null)
                throw new Exception("Invalid account id");
            if (sender.Balance < Amount)
                throw new Exception("Insufficient balance");
            sender.Withdraw(Amount.Value);
            receiver.Deposit(Amount.Value);
        }

        public void DisplayTransaction()
        {
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            Console.WriteLine(ExecutionDate is null);
            return $"Transaction ID: {TransactionId}\nSender ID: {SenderId}\nReceiver ID: {ReceiverId}\nAmount: {Amount}\nExecution date: {ExecutionDate}";
        }
        #endregion
    }
}
