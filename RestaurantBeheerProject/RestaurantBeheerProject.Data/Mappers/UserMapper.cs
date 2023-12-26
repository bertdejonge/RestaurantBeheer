using RestaurantProject.Datalayer.Data;
using RestaurantProject.Datalayer.Exceptions;
using RestaurantProject.Datalayer.Models;
using RestaurantProject.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace RestaurantProject.Datalayer.Mappers {
    public class UserMapper {
        public static User MapToDomain(UserEF dataUser) {
            User domainUser = new(dataUser.Name, dataUser.Email, dataUser.PhoneNumber, dataUser.Zipcode, dataUser.Municipality);

            if (!string.IsNullOrWhiteSpace(dataUser.StreetName)) {
                domainUser.StreetName = dataUser.StreetName;
            }

            if (!string.IsNullOrWhiteSpace(dataUser.HouseNumberLabel)) {
                domainUser.HouseNumberLabel = dataUser.HouseNumberLabel;
            }

            domainUser.SetUserID(dataUser.UserID);

            return domainUser;
        }

        public async static Task<UserEF> MapToData(User domainUser, RestaurantDbContext context) {
            if (domainUser.GetUserID() == 0) {
                UserEF dataUser = new() {
                    Name = domainUser.Name,
                    Email = domainUser.Email,
                    PhoneNumber = domainUser.PhoneNumber,
                    Municipality = domainUser.Municipality,
                    Zipcode = domainUser.ZipCode
                };
                if (!string.IsNullOrWhiteSpace(domainUser.StreetName)) {
                    dataUser.StreetName = domainUser.StreetName;
                }

                if (!string.IsNullOrWhiteSpace(domainUser.HouseNumberLabel)) {
                    dataUser.HouseNumberLabel = domainUser.HouseNumberLabel;
                }

                return dataUser;
            }else {
                UserEF existingUser = await context.Users.Where(u => u.UserID == domainUser.UserID).FirstOrDefaultAsync();
                
                if(existingUser == null) {
                    throw new UserMapperException("No user found with corresponding ID");
                }
                return existingUser;
            }            
        }
    }
}