using System;
using System.Text.RegularExpressions;

namespace ConsoleApp
{
    public class BankAccount
    {
        private decimal _balance;
        private string _accountNumber;
        private string _ownerName;

        public BankAccount(decimal balance, string accountNumber)
        {
            Balance = balance;
            AccountNumber = accountNumber;
        }

        public BankAccount(decimal balance, string accountNumber, string ownerName) : this(balance, accountNumber)
        {
            OwnerName = ownerName;
        }

        public decimal Balance
        {
            get => _balance;
            private set
            {
                if (value > 0)
                    _balance = value;
            }
        }

        public string AccountNumber
        {
            get => _accountNumber;
            private set
            {
                if (Regex.IsMatch(value, @"^\d{2}$"))
                    _accountNumber = value;
                else
                    throw new Exception();
            }
        }

        public string OwnerName
        {
            get => _ownerName;
            set
            {
                if (Regex.IsMatch(value, @"^[A-Z][a-z]*$"))
                    _ownerName = value;
                else
                    throw new Exception();
            }
        }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
                Balance += amount;
            else
                throw new Exception();
        }

        public void Withdraw(decimal amount)
        {
            if (amount > 0 && Balance - amount >= 0)
                Balance -= amount;
            else
                throw new Exception();
        }

        public void DisplayAccountInfo()
        {
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            return $"Account Number: {AccountNumber}\nOwner Name: {OwnerName}\nBalance: {Balance}";
        }
    }
}
