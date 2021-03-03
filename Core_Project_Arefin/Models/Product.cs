using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Core_Project_Arefin.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public string Price { get; set; }
        public DateTime EntryDate { get; set; }


        [NotMapped]
        public IFormFile ImageUrl { get; set; }
    }
}
