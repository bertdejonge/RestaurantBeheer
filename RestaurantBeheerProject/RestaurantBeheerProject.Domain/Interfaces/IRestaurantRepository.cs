using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface IRestaurantRepository {
        Task CreateRestaurantAsync(Restaurant restaurant);

        Task<Restaurant> GetRestaurantByIdAsync(int id);

        // Get restaurants by certain date
        Task<List<Restaurant>> GetAvailableRestaurantsForDateAsync(DateOnly date, int partySize);

        // Search by zipcode OR cuisine OR zipcode and cuisine
        Task<List<Restaurant>> GetRestaurantsByZipAndCuisineAsync(int? zipcode = null, string? cuisine = null);

        Task UpdateRestaurantAsync(Restaurant updatedRestaurant);

        Task RemoveRestaurantAsync(int restaurantID);
    }
}
