using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateTDD.Core
{
    public enum EmployeeType
    {
        CustomerServiceRep
    }

    public class Employee : Entity
    {
        public Employee(string p1, string p2, EmployeeType employeeType)
        {
            // TODO: Complete member initialization
            this.FirstName = p1;
            this.LastName = p2;
            this.Type = employeeType;
        }

        protected Employee()
        {
        }

        public virtual string FirstName { get; set; }

        public virtual EmployeeType Type { get; set; }

        public virtual string LastName { get; set; }

        public virtual decimal HourlyWage { get; set; }
    }
}
