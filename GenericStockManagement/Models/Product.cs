using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericStockManagement.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(55)]
        public string Name { get; set; }
        [Required]
        [MaxLength(55)]
        public string Brand { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        public string? ImageThumbnail { get; set; }
        public int Quantity { get; set; } = 0;
        [Required]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
    }
}
