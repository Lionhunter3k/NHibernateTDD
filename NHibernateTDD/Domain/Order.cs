using System;
using System.Collections.Generic;

namespace NHibernateTDD.Domain
{
    public class Order : Entity
    {
        public Order()
        {
            Products = new HashSet<Product>();
            Customer = new Customer();
        }

        public virtual DateTime DateTime { set; get; }

        public virtual Customer Customer { set; get; } // Many-to-one Association        

        public virtual ICollection<Product> Products { set; get; } // Many-to-many Association        
    }
}
