using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Book_Store_Application.Models.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [DisplayName("Discount Percent")]
        public double? DiscountPercent { get; set; }
        [DisplayName("Discount Price")]
        public double PriceAfterDiscount { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        [Required]
        [DisplayName("Available")]
        public bool IsAvailable { get; set; }
        [DisplayName("Book Author")]
        public int BookAuthorId { get; set; }

        [ForeignKey("BookAuthorId")]
        [ValidateNever]
        public BookAuthor BookAuthor { get; set; }
        [DisplayName("Category")]
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }


    }
}
