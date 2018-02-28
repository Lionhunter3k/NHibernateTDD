using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernateTDD;
using NHibernateTDD.Core;

namespace NHibernateTDD.Tests
{
    [TestFixture]
    public class TransactionTests : BaseTest
    {
        private Account _account;
        private SpecialCustomer _customer;
        private Video _video;
        private Video _video2;

        public override void SetUp()
        {
            _account = new Account();
            _customer = new SpecialCustomer("john", "doe");
            base.SetUp();
        }

        [Test]
        public void CanCreateTransactionForAccount()
        {

            Account a = new Account();
            SpecialCustomer c = new SpecialCustomer("john", "doe");

            Transaction tx = a.CreateTransaction();

            tx.Customer = c;

            //we can only be accurate up to the millisecond on the date here, but we can create a timespan of 2 hours just

            //to be sure.

            Assert.IsTrue(tx.TransactionDate.CompareTo(DateTime.Now.AddHours(-1)) > 0 &&

                            tx.TransactionDate.CompareTo(DateTime.Now.AddHours(1)) < 0, "Date wasn’t set");

            Assert.IsNotNull(tx, "Transaction wasn’t returned");

            Assert.AreEqual(a, tx.Account, "Account wasn’t set");

            Assert.IsNotNull(tx.Rentals, "Rentals collection was null");

            Assert.AreEqual(0, tx.Rentals.Count, "Rentals collection wasn’t empty");

            Assert.AreEqual(0.0, tx.SubTotal());

        }

        [Test]
        public void CanAddRentalToTransaction()
        {

            Transaction tx = _account.CreateTransaction();
            tx.Customer = _customer;

            Rental r = new Rental(_video, 3.50m);
            tx.Rentals.Add(r);

            Assert.AreEqual(1, tx.Rentals.Count, "Rental wasn’t added to transaction");
            Assert.IsTrue(tx.Rentals.Contains(r), "Rental wasn’t the same!");

            Rental r2 = new Rental(_video2, 3.50m);

            tx.Rentals.Add(r2);

            Assert.AreEqual(2, tx.Rentals.Count, "2nd rental wasn’t added to transaction");

            Assert.IsTrue(tx.Rentals.Contains(r2), "2nd rental wasn’t the same!");
        }

        [Test]
        public void CanNotAddSameRentalToTransactionTwice()
        {

            Transaction tx = _account.CreateTransaction();
            tx.Customer = _customer;

            Rental r = new Rental(_video, 3.50m);

            tx.Rentals.Add(r);

            tx.Rentals.Add(r);



            Assert.AreEqual(1, tx.Rentals.Count, "Rental was added twice!");

        }

        [Test]

        public void TransactionSubTotalGiveSumOfRentalPrices()
        {

            Transaction tx = _account.CreateTransaction();
            Assert.AreEqual(0.0, tx.SubTotal(), "No rentals should yield 0.00 subtotal");
            tx.Rentals.Add(new Rental(_video, 3.50m));
            Assert.AreEqual(3.50, tx.SubTotal(), "SubTotal should be 3.50 after adding a 3.50 rental");
            tx.Rentals.Add(new Rental(_video2, 3.50m));
            Assert.AreEqual(7.00, tx.SubTotal(), "SubTotal should be 7.00 after adding 2 3.50 rentals");

        }
    }
}
