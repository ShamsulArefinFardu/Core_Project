using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Project_Arefin.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public long ID { get; set; }

        [Required, Display(Name = "Category")]
        public string Name { get; set; }

        public virtual IList<Item> Items { get; set; }
    }
}
