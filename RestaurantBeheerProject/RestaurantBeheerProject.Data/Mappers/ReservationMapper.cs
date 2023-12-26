using RestaurantProject.Datalayer.Data;
using RestaurantProject.Datalayer.Models;
using RestaurantProject.Domain.Models;

namespace RestaurantProject.Datalayer.Mappers {
    public class ReservationMapper {
        public static Reservation MapToDomain(ReservationEF dataReservation, RestaurantDbContext context) {
            Reservation domain = new(RestaurantMapper.MapToDomain(dataReservation.Restaurant, context), UserMapper.MapToDomain(dataReservation.User),
                                     dataReservation.PartySize, DateOnly.FromDateTime(dataReservation.DateAndStartTime.Date), TimeOnly.FromTimeSpan(dataReservation.DateAndStartTime.TimeOfDay));

            return domain;
        }

        public async static Task<ReservationEF> MapToData(Reservation domainReservation, RestaurantDbContext context) {
            DateTime date = domainReservation.Date.ToDateTime(domainReservation.StartTime);

            ReservationEF dataReservation = new() {
                Restaurant = await RestaurantMapper.MapToData(domainReservation.Restaurant, context),
                User = await UserMapper.MapToData(domainReservation.User, context),
                PartySize = domainReservation.PartySize,
                DateAndStartTime = date,
                TableNumber = domainReservation.TableNumber
            };

            return dataReservation;
        }
    }
}