using System.Collections.Generic;

namespace NHibernateTDD.Domain
{
    public class Product : Entity
    {
        public Product()
        {
            Orders = new HashSet<Order>();
        }

        public virtual string Name { set; get; }
        
        public virtual ICollection<Order> Orders { set; get; } // Many-to-many Association        
    }
}
