﻿using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface IRestaurantService {

        Task<bool> ExistingRestaurantAsync(Restaurant domainRestaurant);
        Task<bool> ExistsById(int restaurantID);

        // GET
        Task<Restaurant> GetRestaurantByIdAsync(int id);
        Task<List<Restaurant>> GetAvailableRestaurantsForDateAsync(DateOnly date, int partySize);
        Task<List<Restaurant>> GetRestaurantsByZipAndCuisineAsync(int? zipcode = null, string? cuisine = null);

        // POST
        Task<Restaurant> CreateRestaurantAsync(Restaurant restaurant);

        // PUT
        Task<Restaurant> UpdateRestaurantAsync(int restaurantID, Restaurant updatedRestaurant);

        // DELETE
        Task RemoveRestaurantAsync(int restaurantID);
    }
}
