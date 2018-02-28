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
    public class EmployeeTests : BaseTest
    {
        [Test]
        public void CanCreateEmployee()
        {
            Employee emp = new Employee("Joe", "Blow", EmployeeType.CustomerServiceRep);
            Assert.AreEqual("Joe", emp.FirstName, "FirstName wasn’t set properly.");
            Assert.AreEqual("Blow", emp.LastName, "LastName wasn’t set properly.");
            Assert.AreEqual(EmployeeType.CustomerServiceRep, emp.Type, "Employee type was not set.");
            Assert.AreEqual(0.0, emp.HourlyWage, "Wage was not initially 0.0");

        }

        [Test]
        public void CanSavePerson()
        {
            using (var session = SessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {

                    //create a person
                    Person p = new Person("Bruce", "Wayne");
                    p.DateOfBirth = new DateTime(1972, 6, 2);
                    p.HomePhone = "12345678";
                    p.AltPhone = "987675123";
                    p.Address.Line1 = "123 Bruce Manor";
                    p.Address.State = "GT";
                    p.Address.City = "Gotham";
                    p.Address.Zip = "99999";
                    //save the person
                    session.Save(p);
                    Assert.IsTrue(p.Id != 0, "Save should give p it’s primary key");
                    Person pVerify = (Person)session.Load(typeof(Person), p.Id);
                    Assert.AreEqual(p, pVerify, "They weren’t the same");
                }

            }

        }
    }
}
