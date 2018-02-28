

namespace NHibernateTDD.Domain
{
    public class Address : Entity
    {
        public virtual string City { set; get; }

        public virtual string StreetAddress { set; get; }

        public virtual string StateName { set; get; }

        public virtual string Zip { set; get; }
    }
}
