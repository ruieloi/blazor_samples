namespace Sample.Web.Shared
{
    public record Employee
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string FirstName { get; init; }
        public string FamilyName { get; init; }

        //EmployeeContract data
        //should be a subclass
        public bool IsTermContract { get; init; }

        //EmployeeHR data
        //should be a subclass
        public string? Address { get; init; }
        public Gender Gender { get; init; }

        public string FullName { 
            get {
                return $"{FirstName} {FamilyName}";
            }
        }

        public EmployeeSummary ToSummary() => new EmployeeSummary
        {
            Id = Id,
            Name = FullName
        };
    }
}
