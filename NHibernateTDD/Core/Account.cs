using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTDD.Core
{
    public class Account : Entity
    {
        public Account()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public virtual ISet<Transaction> Transactions { get; set; }

        public virtual Transaction CreateTransaction()
        {
            var tx = new Transaction(this);
            this.Transactions.Add(tx);
            return tx;
        }
    }
}
