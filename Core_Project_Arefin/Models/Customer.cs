using Core_Project_Arefin.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Project_Arefin.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        //[MyValidation]
        [Column(TypeName = "nvarchar(11)")]
        [Required(ErrorMessage = "Must be filled Phone number")]
        [MaxLength(11, ErrorMessage = "Maximum 11 characters only")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please input Your name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Gender")]
        public string Gender { get; set; }
        
        [Required(ErrorMessage = "please Enter your Address.")]
        public string Address { get; set; }

        [DisplayName("Registration Fee")]
        [Required(ErrorMessage = "must be filled Fee.")]
        public int RegistrationFee { get; set; }


        [CustomDate]
        [DisplayName("Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Date { get; set; }
    }
}

