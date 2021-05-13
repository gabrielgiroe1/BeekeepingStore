using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingStore.Models
{
    public class Honey
    {
        public int Id { get; set; }

        public Make Make { get; set; }
        public int MakeID { get; set; }

        public Model Model { get; set; }
        public int ModelID { get; set; }

        [Required]
        public int Year { get; set; }
        public string Details { get; set; }
        [Required]
        public string SellerName { get; set; }
        public string SellerEmail { get; set; }
        [Required]
        public string SellerPhone { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Curency { get; set; }
        [Required]
        public string ImagePath { get; set; }
    }
}
