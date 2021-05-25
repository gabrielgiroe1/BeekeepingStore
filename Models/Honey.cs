using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BeekeepingStore.Extensions;
namespace BeekeepingStore.Models
{
    public class Honey
    {
        public int Id { get; set; }

        public Make Make { get; set; }

        [RegularExpression("^[1-9]*$", ErrorMessage = "Select Make")]
        public int MakeID { get; set; }

        public Model Model { get; set; }

        //  [RegularExpression("^[1-102]*$", ErrorMessage = "Select Model")]
        public int ModelID { get; set; }

        [Required(ErrorMessage = "Provide Year")]
        [Range(2000, 2030, ErrorMessage = "Invalid Year")]
        public int Year { get; set; }
        public string Details { get; set; }

        [Required(ErrorMessage = "Provide Seller Name")]
        public string SellerName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email ID")]
        public string SellerEmail { get; set; }
        [Required(ErrorMessage = "Provide Seller Phone")]
        public string SellerPhone { get; set; }
        [Required(ErrorMessage = "Provide Selling Price")]
        public int Price { get; set; }

        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Select Currency")]
        [Required(ErrorMessage = "Provide Selling Currency")]
        public string Curency { get; set; }
        //  [Required]
        public string ImagePath { get; set; }
    }
}
