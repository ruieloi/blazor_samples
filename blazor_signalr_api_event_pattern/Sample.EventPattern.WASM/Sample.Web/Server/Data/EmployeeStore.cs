using Bogus;
using Sample.Web.Shared;
using System.Collections.Concurrent;

namespace Sample.Web.Server.Data
{
    public class EmployeeStore: IEmployeeStore
    {

        private static ConcurrentBag<Employee> employees;

        public ConcurrentBag<Employee> Employees
        {
            get
            {
                return employees;
            }
        }

        public EmployeeStore()
        {
            //don't do this in production. Constructors should be fast
            generateStartData();
        }

        private void generateStartData()
        {
            var employeeFaker = new Faker<Employee>()
                                .RuleFor(o => o.FirstName, f => f.Name.FirstName())
                                .RuleFor(o => o.FamilyName, f => f.Name.LastName())
                                .RuleFor(o => o.Gender, f => f.PickRandom<Gender>())
                                .RuleFor(o => o.IsTermContract, f => f.Random.Bool())
                                .RuleFor(o => o.Address, f => f.Address.FullAddress());

            employees = new ConcurrentBag<Employee>(employeeFaker.Generate(2).ToList());
        }
    }
}
