using Company.Test.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Company.Test.PL.ViewModels
{
    public class EmployeeVM
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }


        [Range(25, 60, ErrorMessage = "Age must be between 25 , 60")]
        public int? Age { get; set; }
        public string Address { get; set; }


        [Required(ErrorMessage = "Salary Is Required")]
        public decimal Salary { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime HiringDate { get; set; }
        public int? WorkforId { get; set; }
        public Department? Workfor { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; }
    }
}
