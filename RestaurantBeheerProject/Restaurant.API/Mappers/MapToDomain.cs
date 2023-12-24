using RestaurantProject.API.Models.Input;
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
    }
}