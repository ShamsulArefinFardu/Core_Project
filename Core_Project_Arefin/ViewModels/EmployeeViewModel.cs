using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Project_Arefin.ViewModels
{
    public class EmployeeViewModel:EditImageViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string EmployeeName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Join Date")]
        public DateTime JoiningDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Resign Date")]
        public DateTime ResignDate { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Not Match")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
