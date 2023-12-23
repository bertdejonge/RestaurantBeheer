using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Datalayer.Models {
    public class UserEF {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [MaxLength(255)]
        [Required]
        public string Name { get; set; }

        //Contact info
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        // LOCATION
        [Required]                                                          // Municipality = required
        [MaxLength(255)]
        public string Municipality { get; set; }

        [Required]
        public int Zipcode { get; set; }

        [MaxLength(255)]
        public string StreetName { get; set; }

        [MaxLength(10)]
        public string HouseNumberLabel { get; set; }

        public override bool Equals(object? obj) {
            return obj is UserEF eF &&
                   Name == eF.Name &&
                   PhoneNumber == eF.PhoneNumber &&
                   Email == eF.Email;
        }
    }
}
