using RestaurantProject.Datalayer.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestaurantProject.API.Models.Input {
    public class RestaurantInputDTO {
        public string Name { get; set; }
        public string Cuisine { get; set; }

        //Contact info
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        // Location
        public string Municipality { get; set; }
        public int ZipCode { get; set; }
        public string StreetName { get; set; }
        public string HouseNumberLabel { get; set; }

        public List<TableInputDTO> Tables { get; set; }
    }
}
