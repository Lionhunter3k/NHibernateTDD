using System.Collections.Generic;

namespace NHibernateTDD.Domain
{
    public class Customer : Entity
    {
        public Customer()
        {
            Address = new Address();
            RelatedCustomers = new HashSet<Customer>();
        }

        public virtual string Name { set; get; }

        public virtual Level Level { set; get; }

        public virtual Address Address { set; get; } // Many-to-one Association

        public virtual InterestComponent Interest { set; get; } // Component mapping

        protected ISet<Order> orders = new HashSet<Order>();

        public virtual IEnumerable<Order> Orders
        {
            get
            {
                return this.orders;
            }
        }// One-to-many Association

        public virtual ICollection<Customer> RelatedCustomers { set; get; } // A self-referencing object

        public virtual int TotalOrders
        {
            get
            {
                return this.orders.Count;
            }
        }
    }
}
