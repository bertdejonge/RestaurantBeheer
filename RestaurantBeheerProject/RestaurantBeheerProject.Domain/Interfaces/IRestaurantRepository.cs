using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface IRestaurantRepository {

        Task<bool> ExistingRestaurantAsync(int zipCode, string name, string cuisine);

        // GET
        Task<List<Restaurant>> GetAvailableRestaurantsForDateAsync(DateOnly date, int partySize);
        Task<List<Restaurant>> GetRestaurantsByZipAndCuisineAsync(int? zipcode = null, string? cuisine = null);
        Task<Restaurant> GetRestaurantByIdAsync(int id);

        // CREATE
        Task<Restaurant> CreateRestaurantAsync(Restaurant restaurant);

        // UPDATE
        Task<Restaurant> UpdateRestaurantAsync(int restaurantID, Restaurant updatedRestaurant);

        // DELETE
        Task RemoveRestaurantAsync(int restaurantID);
    }
}
