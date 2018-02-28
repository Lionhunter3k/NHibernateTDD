using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTDD.Core
{
    public class Person : Entity
    {
        
        public Person(string p1, string p2)
        {
            // TODO: Complete member initialization
            this.FirstName = p1;
            this.LastName = p2;
            this.Address = new Address();
        }

        protected Person()
        {
        }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual DateTime DateOfBirth { get; set; }

        public virtual string HomePhone { get; set; }

        public virtual string AltPhone { get; set; }

        public virtual Address Address { get; set; }
    }

    public class Address
    {
        public virtual string Line1 { get; set; }

        public virtual string State { get; set; }

        public virtual string City { get; set; }

        public virtual string Zip { get; set; }
    }
}
