using RestaurantProject.Datalayer.Data;
using RestaurantProject.Datalayer.Models;
using RestaurantProject.Domain.Models;

namespace RestaurantProject.Datalayer.Mappers {
    public class ReservationMapper {
        public static Reservation MapToDomain(ReservationEF dataReservation, RestaurantDbContext context) {
            Reservation domain = new(RestaurantMapper.MapToDomain(dataReservation.Restaurant, context), UserMapper.MapToDomain(dataReservation.User),
                                     dataReservation.PartySize, dataReservation.Date, TimeOnly.FromTimeSpan(dataReservation.StartTime));

            return domain;
        }

        public static ReservationEF MapToData(Reservation domainReservation, RestaurantDbContext context) {
            ReservationEF dataReservation = new() {
                Restaurant = RestaurantMapper.MapToData(domainReservation.Restaurant, context),
                User = UserMapper.MapToData(domainReservation.User)

            }
        }
    }
}