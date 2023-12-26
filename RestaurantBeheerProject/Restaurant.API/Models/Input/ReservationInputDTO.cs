using RestaurantProject.Datalayer.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestaurantProject.API.Models.Input {
    public class ReservationInputDTO {
        public int RestaurantID { get; set; }
        public int UserID { get; set; }
        public int PartySize { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
    }
}
