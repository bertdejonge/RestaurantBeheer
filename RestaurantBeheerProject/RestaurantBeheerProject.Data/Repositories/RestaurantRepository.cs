using Microsoft.EntityFrameworkCore;
using RestaurantProject.Datalayer.Data;
using RestaurantProject.Datalayer.Exceptions;
using RestaurantProject.Datalayer.Mappers;
using RestaurantProject.Datalayer.Models;
using RestaurantProject.Domain.Interfaces;
using RestaurantProject.Domain.Models;
using RestaurantProject.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Datalayer.Repositories {
    public class RestaurantRepository : IRestaurantRepository {

        private readonly RestaurantDbContext _context;

        public RestaurantRepository(RestaurantDbContext context) {
            _context = context ?? throw new ContextException(nameof(context));
        }

        // Reusable method as to not write double code
        private async Task SaveAndClearAsync() {
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }

        // Check if a restaurant already exists. Restaurants are equal when they have the same zipcode and name
        public async Task<bool> ExistingRestaurantAsync(int zipCode, string name, string cuisine) {
            return await _context.Restaurants.AnyAsync(r => r.ZipCode == zipCode && r.Name == name && r.Cuisine == cuisine);
        }

        // Get restaurant by ID
        public async Task<Restaurant> GetRestaurantByIdAsync(int id) {
            try {               

                var dataRestaurant = await _context.Restaurants
                                     .Include(t => t.Tables)
                                     .FirstOrDefaultAsync(i => i.RestaurantID == id);


                if (dataRestaurant == null) {
                    throw new RestaurantRepositoryException($"No restaurant found with id {id}");
                }

                return RestaurantMapper.MapToDomain(dataRestaurant, _context);

            } catch (Exception) {
                throw;
            }
        }

        // Gets all Restaurants that have at least one table available to accomodate the needed partysize
        public async Task<List<Restaurant>> GetAvailableRestaurantsForDateAsync(DateOnly date, int partySize) {
            try {
                

                var restaurants = await _context.Restaurants
                                     .Include(t => t.Tables).Select(r => RestaurantMapper.MapToDomain(r, _context))
                                     .ToListAsync();


                List<Restaurant> availableRestaurants = restaurants.Where(r => r.IsAnyTableAvailableForDate(date, partySize)).ToList();



                return availableRestaurants;

            } catch (Exception) {
                throw;
            }
        }

        // Gets all restaurants either by their zipcode and/or their cuisine
        public async Task<List<Restaurant>> GetRestaurantsByZipAndCuisineAsync(int? zipCode = null, string? cuisine = null) {
            try {
                List<RestaurantEF> dataRestaurants = new(); 

                // At least one of the paramters will not be null (checked in manager)
                dataRestaurants = await _context.Restaurants
                                 .Where(r => r.Cuisine == cuisine && r.ZipCode == zipCode).ToListAsync();

                return dataRestaurants.Select(r => RestaurantMapper.MapToDomain(r, _context)).ToList();

            } catch (Exception) {
                throw;
            }
        }

        // Creates a new restaurant record in the DB based on a restaurant from the 'UI'
        public async Task CreateRestaurantAsync(Restaurant domainRestaurant) {
            try {

                // Check if the given restaurant already exists in the database
                // 2 restaurants are equal when they have the same zipcode and name
                if (await ExistingRestaurantAsync(domainRestaurant.ZipCode, domainRestaurant.Name, domainRestaurant.Cuisine)) {
                    throw new RestaurantRepositoryException($"Restaurant with Name {domainRestaurant.Name} and Zipcode {domainRestaurant.ZipCode} already exists.");
                }

                // If new restaurant, add it to the database and save
                _context.Add(RestaurantMapper.MapToData(domainRestaurant, _context));

                await SaveAndClearAsync();

            } catch (Exception ) {
                throw;
            }
        }
        
        // Removes a restaurant based on its ID
        public async Task RemoveRestaurantAsync(int restaurantID) {
            try {

                // Try to get the restaurant 
                var dataRestaurant = await _context.Restaurants
                                     .Include(t => t.Tables)
                                     .FirstOrDefaultAsync(i => i.RestaurantID == restaurantID);

                // perform a null check
                if (dataRestaurant == null) {
                    throw new RestaurantRepositoryException($"No restaurant found with id {restaurantID}");
                }

                // If a restaurant was retrieved, that means that the restaurant exists, and thus can be removed
                _context.Remove(dataRestaurant);

                // Also remove all the reservations for the restaurant 
                _context.Reservations.RemoveRange(_context.Reservations.Where(r => r.RestaurantID ==  restaurantID));

                await SaveAndClearAsync();

            } catch (Exception) {
                throw;
            };
        }

        // Updates a restaurant 
        public async Task UpdateRestaurantAsync(Restaurant domainRestaurant) {
            try {
                RestaurantEF updatedRestaurant = RestaurantMapper.MapToData(domainRestaurant, _context);

                if(! await ExistingRestaurantAsync(updatedRestaurant.ZipCode, updatedRestaurant.Name, updatedRestaurant.Cuisine)) {
                    throw new RestaurantRepositoryException("Restaurant doesn't exist");
                }

                // Change tracker of the the current restaurant and detach it to avoid conflicts
                // EF might track the existing and update restaurant, which will result in a conflict 
                var existingRestaurant = _context.ChangeTracker.Entries<RestaurantEF>()
                                        .FirstOrDefault(r => r.Entity.RestaurantID == updatedRestaurant.RestaurantID);

                if (existingRestaurant != null) {
                    existingRestaurant.State = EntityState.Detached;
                }

                // Equals overridden
                if (updatedRestaurant.Equals(existingRestaurant)) {
                    throw new RestaurantRepositoryException("Restaurants are the same. No update required.");
                }
                
                _context.Restaurants.Update(updatedRestaurant);
                await SaveAndClearAsync();

            } catch (Exception) {
                throw;
            }
        }
    }
}

        

        