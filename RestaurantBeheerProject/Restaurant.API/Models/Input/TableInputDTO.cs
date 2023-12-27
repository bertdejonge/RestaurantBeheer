using RestaurantProject.Datalayer.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestaurantProject.API.Models.Input {
    public class TableInputDTO {
        public int TableNumber { get; set; }
        public int Seats { get; set; }
    }
}