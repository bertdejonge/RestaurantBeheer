using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Datalayer.Models {
    public class Restaurant {
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

        public int Zipcode { get; set; }

        [MaxLength(255)]
        public string StreetName { get; set; }

        [MaxLength(10)]
        public string HouseNumber { get; set; }

        // relationships other tables
        [Required]
        public IEnumerable<Reservation> Reservations { get; set;}

        [Required]
        public IEnumerable<Table> Tables { get; set;}
    }
}
