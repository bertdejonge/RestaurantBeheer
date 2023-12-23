using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Datalayer.Models {
    public class ReservationEF {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationID { get; set; }

        [ForeignKey("RestaurantID")]
        public int RestaurantID { get; set; }

        public RestaurantEF Restaurant { get; set; }

        [ForeignKey("UserID")]
        public int UserID { get; set; }

        public UserEF User { get; set; }

        [Required]
        public int PartySize { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [ForeignKey("TableID")]
        public int TableNumber { get; set; }

        public override bool Equals(object? obj) {
            return obj is ReservationEF eF &&
                   ReservationID == eF.ReservationID &&
                   RestaurantID == eF.RestaurantID &&
                   UserID == eF.UserID &&
                   Date.Equals(eF.Date);
        }
    }
}
