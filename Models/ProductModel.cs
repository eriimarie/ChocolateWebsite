using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChocoShop.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public List<string> Tags { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Discount { get; set; }
        [Required]
        public int QuantityInStock { get; set; }

        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        public List<IFormFile> Images { get; set; }
        public string Thumbnail { get; set; }
        public string DetailImage { get; set; }
    }
}