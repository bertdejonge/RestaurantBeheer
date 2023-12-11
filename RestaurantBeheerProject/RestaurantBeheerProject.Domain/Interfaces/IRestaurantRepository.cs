using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface IRestaurantRepository {
        void CreateRestaurant(Restaurant restaurant);

        Restaurant GetRestaurant(int id);

        // Get restaurants by certain date
        List<Restaurant> GetAvailableRestaurantsForDate(DateTime date, int partySize);

        // Search by zipcode OR cuisine OR zipcode and cuisine
        List<Restaurant> GetRestaurantsByZipAndCuisine(int? zipcode = null, string? cuisine = null);

        void UpdateRestaurant(int restaurantID, Restaurant updatedRestaurant);

        void RemoveRestaurant(int restaurantID);
    }
}
