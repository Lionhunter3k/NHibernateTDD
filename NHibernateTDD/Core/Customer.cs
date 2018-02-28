using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTDD.Core
{
    public class SpecialCustomer : Entity
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public SpecialCustomer(string p1, string p2)
        {
            // TODO: Complete member initialization
            this.FirstName = p1;
            this.LastName = p2;
        }

        protected SpecialCustomer()
        {
        }
    }
}
