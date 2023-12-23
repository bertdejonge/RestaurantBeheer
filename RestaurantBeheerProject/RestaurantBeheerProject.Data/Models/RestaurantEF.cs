using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Datalayer.Models {
    public class RestaurantEF {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RestaurantID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public string Cuisine {  get; set; }

        //Contact info
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        // Location
        [Required]                                                          // Municipality = required
        [MaxLength(255)]
        public string Municipality { get; set; }

        public int ZipCode { get; set; }

        [MaxLength(255)]
        public string StreetName { get; set; }

        [MaxLength(10)]
        public string HouseNumberLabel { get; set; }

        [Required]
        public List<TableEF> Tables { get; set;}

        public override bool Equals(object? obj) {
            return obj is RestaurantEF eF &&
                   RestaurantID == eF.RestaurantID &&
                   Name == eF.Name &&
                   Cuisine == eF.Cuisine &&
                   PhoneNumber == eF.PhoneNumber &&
                   Email == eF.Email &&
                   Municipality == eF.Municipality &&
                   ZipCode == eF.ZipCode &&
                   StreetName == eF.StreetName &&
                   HouseNumberLabel == eF.HouseNumberLabel &&
                   EqualityComparer<List<TableEF>>.Default.Equals(Tables, eF.Tables);
        }

        public override string ToString() {
            return $"Restaurant {Name} ({Cuisine}) at {Municipality}, {ZipCode}" +
                   $"Phone:{PhoneNumber} Email: {Email}";
        }
    }
}
