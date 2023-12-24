using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface IRestaurantService {       
        // GET
        Task<Restaurant> GetRestaurantByIdAsync(int id);
        Task<List<Restaurant>> GetAvailableRestaurantsForDateAsync(DateOnly date, int partySize);
        Task<List<Restaurant>> GetRestaurantsByZipAndCuisineAsync(int? zipcode = null, string? cuisine = null);

        // POST
        Task CreateRestaurantAsync(Restaurant restaurant);

        // PUT
        Task UpdateRestaurantAsync(Restaurant updatedRestaurant);

        // DELETE
        Task RemoveRestaurantAsync(int restaurantID);
    }
}
