using Microsoft.Identity.Client;
using RestaurantProject.API.Models.Output;
using RestaurantProject.Domain.Models;

namespace RestaurantProject.API.Mappers {
    public static class MapFromDomain {
        public static RestaurantOutputDTO MapFromRestaurantDomain(Restaurant restaurant) {
            ValidateNotNull(restaurant, "restaurant");

            RestaurantOutputDTO restoDTO = new() {
                ID = restaurant.RestaurantID,
                Name = restaurant.Name,
                Cuisine = restaurant.Cuisine,
                PhoneNumber = restaurant.PhoneNumber,
                Email = restaurant.Email,
                Municipality = restaurant.Municipality,
                ZipCode = restaurant.ZipCode,
                Tables = new List<TableOutputDTO>(restaurant.Tables.Select(t => MapFromTableDomain(t)))
            };

            return restoDTO;

        }

        private static TableOutputDTO MapFromTableDomain(Table t) {
            ValidateNotNull(t, "table");

            TableOutputDTO tabloDTO = new() {
                TableID = t.TableID,
                TableNumber = t.TableNumber,
                Seats = t.Seats
            };

            return tabloDTO;
        }


        public static UserOutputDTO MapFromUserDomain(User user) {
            ValidateNotNull(user, "user");

            UserOutputDTO useroDTO = new() {
                Id = user.UserID,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Municipality = user.Municipality,
                ZipCode = user.ZipCode
            };

            if (!string.IsNullOrWhiteSpace(user.StreetName)) {
                useroDTO.StreetName = user.StreetName;
            }

            if (!string.IsNullOrWhiteSpace(user.HouseNumberLabel)) {
                useroDTO.HouseNumberLabel = user.HouseNumberLabel;
            }

            return useroDTO;
        }

        public static ReservationOutputDTO MapFromReservationDomain(Reservation r) {
            ValidateNotNull(r, "reservation");

            ReservationOutputDTO resoDTO = new() {
                ReservationID = r.ReservationId,
                RestaurantInfo = r.Restaurant.ToString(),
                UserInfo = r.User.ToString(),
                PartySize = r.PartySize,
                Date = r.Date,
                StartTime = r.StartTime,
                TableNumber = r.TableNumber
            };

            return resoDTO;
        }

        private static void ValidateNotNull(object obj, string name) {
            if (obj == null) throw new ArgumentNullException(name);
        }

        
    }
}
