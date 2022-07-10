using Bogus;
using Sample.Web.Shared;
using System.Collections.Concurrent;
using System.Linq;

namespace Employee.API.Data
{
    public class EmployeeStore: IEmployeeStore
    {
        //ideally we shouldn't be using the Sample.Web.Shared project 
        //Internal classes will probably have more fields etc that shouldn't be exposed in UI
        private static ConcurrentBag<Sample.Web.Shared.Employee> employees;

        public ConcurrentBag<Sample.Web.Shared.Employee> Employees
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
            var employeeFaker = new Faker<Sample.Web.Shared.Employee>()
                                .RuleFor(o => o.FirstName, f => f.Name.FirstName())
                                .RuleFor(o => o.FamilyName, f => f.Name.LastName())
                                .RuleFor(o => o.Gender, f => f.PickRandom<Gender>())
                                .RuleFor(o => o.IsTermContract, f => f.Random.Bool())
                                .RuleFor(o => o.Address, f => f.Address.FullAddress());

            employees = new ConcurrentBag<Sample.Web.Shared.Employee>(employeeFaker.Generate(2).ToList());
        }
    }
}
