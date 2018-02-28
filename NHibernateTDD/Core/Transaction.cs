using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTDD.Core
{
    public class Transaction : Entity
    {
        public Transaction()
        {
            this.TransactionDate = DateTime.Now;
            this.Rentals = new HashSet<Rental>();
        }

        public Transaction(Account account)
            :this()
        {
            // TODO: Complete member initialization
            this.Account = account;
        }

        public virtual SpecialCustomer Customer { get; set; }

        public virtual Account Account { get; set; }

        public virtual ISet<Rental> Rentals { get; set; }

        public virtual int RentalsCount { get; set; }

        public virtual decimal SubTotal()
        {
            return this.Rentals.Sum(t => t.Price);
        }

        public virtual DateTime TransactionDate { get; set; }
    }
}
