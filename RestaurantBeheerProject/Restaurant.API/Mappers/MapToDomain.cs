using Microsoft.EntityFrameworkCore;
using RestaurantProject.API.Exceptions;
using RestaurantProject.API.Models.Input;
using RestaurantProject.Datalayer.Data;
using RestaurantProject.Datalayer.Mappers;
using RestaurantProject.Datalayer.Models;
using RestaurantProject.Domain.Models;

namespace RestaurantProject.API.Mappers {
    public static class MapToDomain {
        public static User MapToUserDomain(UserInputDTO user) {
            User domainUser = new(user.Name, user.Email, user.PhoneNumber, user.ZipCode, user.Municipality);

            if (!string.IsNullOrWhiteSpace(user.StreetName)) {
                domainUser.StreetName = user.StreetName;
            }

            if (!string.IsNullOrWhiteSpace(user.HousenumberLabel)) {
                domainUser.HouseNumberLabel = user.HousenumberLabel;
            }

            return domainUser;
        }

        public static Reservation MapToReservationDomain(ReservationInputDTO input, RestaurantDbContext _context) {
            RestaurantEF dataResto = _context.Restaurants.Include(r => r.Tables).FirstOrDefault(r => r.RestaurantID == input.RestaurantID);

            if(dataResto == null) {
                throw new MapToDomainException("Given restaurantID does not correspond with a restaurant.");
            }

            Restaurant domainResto = RestaurantMapper.MapToDomain(dataResto, _context);

            UserEF dataUser = _context.Users.FirstOrDefault(u => u.UserID == input.UserID);

            if(dataUser == null) {
                throw new MapToDomainException("Given userID does not correspond with a user.");
            }

            User domainUser = UserMapper.MapToDomain(dataUser);

            if(input.PartySize <= 0) {
                throw new MapToDomainException("invalid partysize.");
            }

            if(input.Date < DateOnly.FromDateTime(DateTime.Now.Date) || input.Date < DateOnly.FromDateTime(DateTime.Now.Date.AddMonths(3))) {
                throw new MapToDomainException("Invalid date. Date must be between today and 3 months from now");
            }

            if(input.StartTime < new TimeOnly(17,00) || input.StartTime > new TimeOnly(22, 00)) {
                throw new MapToDomainException("Invalid starttime. Starttime must be between 17h00 and 22h00");
            }
            
            Reservation domainReservation = new(domainResto, domainUser, input.PartySize, input.Date, input.StartTime);

            return domainReservation;
        }

        private static Restaurant MapToRestaurantDomain(RestaurantEF dataResto) {
            throw new NotImplementedException();
        }
    }
}