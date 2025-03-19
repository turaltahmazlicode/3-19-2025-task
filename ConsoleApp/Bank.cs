using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Bank
    {
        private BankAccount[] _bankAccounts;

        public Bank(BankAccount[] bankAccounts)
        {
            _bankAccounts = bankAccounts;
        }

        public BankAccount[] BankAccounts
        {
            get { return _bankAccounts; }
            set { _bankAccounts = value; }
        }

        internal BankAccount getAccountByAccountNumber(string accountNumber)
        {
            foreach (var bankAccount in BankAccounts)
                if (bankAccount.AccountNumber == accountNumber)
                    return bankAccount;
            return null;
        }
    }
}
