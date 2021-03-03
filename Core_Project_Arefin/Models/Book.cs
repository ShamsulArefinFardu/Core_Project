using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Project_Arefin.Models
{    
    public class Book
    {
        [Key]
        public long ID { get; set; }

        [Required, Display(Name = "Book Name")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string BookDetails { get; set; }

        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        [DataType(DataType.Date)]

        public DateTime PublishDate { get; set; }

        [Required]
        public long Quantity { get; set; }

        [ForeignKey("Publisher")]
        public long PublisherID { get; set; }


        public virtual Publisher Publisher { get; set; }
    }
}
