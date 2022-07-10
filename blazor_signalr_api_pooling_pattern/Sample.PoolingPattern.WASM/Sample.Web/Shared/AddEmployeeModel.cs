using System.ComponentModel.DataAnnotations;

namespace Sample.Web.Shared
{
    public class AddEmployeeModel: IValidatableObject
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string FamilyName { get; set; }

        //EmployeeContract data
        public bool IsTermContract { get; set; }

        //EmployeeHR data
        public string? Address { get; set; }
        public Gender? Gender { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //add your model validation here

            //if (this.Options.Count < 2)
            //{
            //    yield return new ValidationResult("A mployee requires at least 2 options.");
            //}

            return new List<ValidationResult>();
        }
    }
}