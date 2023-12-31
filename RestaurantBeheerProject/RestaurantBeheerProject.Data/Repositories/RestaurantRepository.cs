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
using System.Transactions;

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

        public async Task<bool> ExistingRestaurantByIdAsync(int restaurantID) {
            return await _context.Restaurants.AnyAsync(r => r.RestaurantID == restaurantID);
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
                
                // First get all restaurants
                List<RestaurantEF> dataRestaurants = await _context.Restaurants.Include(r => r.Tables).ToListAsync();

                List<Restaurant> restaurants = dataRestaurants.Select(dr => RestaurantMapper.MapToDomain(dr, _context)).ToList();


                // And check for every restaurant if they have an available table that day
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
                dataRestaurants = await _context.Restaurants.Include(r => r.Tables)
                                 .Where(r => r.Cuisine == cuisine || r.ZipCode == zipCode).ToListAsync();

                return dataRestaurants.Select(r => RestaurantMapper.MapToDomain(r, _context)).ToList();

            } catch (Exception) {
                throw;
            }
        }

        // Creates a new restaurant record in the DB based on a restaurant from the 'UI'
        public async Task<Restaurant> CreateRestaurantAsync(Restaurant domainRestaurant) {
            using (var transaction = await _context.Database.BeginTransactionAsync()) {
                try {

                    // Check if the given restaurant already exists in the database
                    // 2 restaurants are equal when they have the same zipcode and name
                    if (await ExistingRestaurantAsync(domainRestaurant.ZipCode, domainRestaurant.Name, domainRestaurant.Cuisine)) {
                        throw new RestaurantRepositoryException($"Restaurant with Name {domainRestaurant.Name} and Zipcode {domainRestaurant.ZipCode} already exists.");
                    }

                    // Map the restaurant and clear its tables so we can insert the restaurant first
                    // and then get its ID to add to the tables so we can link the tables to their restaurant
                    RestaurantEF dataResto = await RestaurantMapper.MapToData(domainRestaurant, _context);
                    List<TableEF> tempDataTables = new(dataResto.Tables);
                    dataResto.Tables.Clear();

                    // Insert the restaurant with an empty list of tables
                    _context.Add(dataResto);
                    await SaveAndClearAsync();

                    int restaurantID = dataResto.RestaurantID;

                    // Now link the restaurant ID to every table in the temp and add them to the restaurant
                    foreach (TableEF table in tempDataTables) {
                        table.RestaurantID = restaurantID;
                        dataResto.Tables.Add(table);
                    }

                    _context.Update(dataResto);
                    await SaveAndClearAsync();
                    await transaction.CommitAsync();

                    return RestaurantMapper.MapToDomain(dataResto, _context);

                } catch (Exception) {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        
        // Removes a restaurant based on its ID
        public async Task RemoveRestaurantAsync(int restaurantID) {
            try {

                // Try to get the restaurant 
                var dataRestaurant = await _context.Restaurants
                                     .Include(t => t.Tables)
                                     .FirstOrDefaultAsync(i => i.RestaurantID == restaurantID);

                _context.Remove(dataRestaurant);

                // Also remove all tables & reservations for the restaurant 
                _context.Reservations.RemoveRange(_context.Reservations.Where(r => r.RestaurantID ==  restaurantID));
                _context.Tables.RemoveRange(_context.Tables.Where(t => t.RestaurantID == restaurantID));

                await SaveAndClearAsync();

            } catch (Exception) {
                throw;
            }
        }

        // Updates a restaurant 
        public async Task<Restaurant> UpdateRestaurantAsync(int restaurantID, Restaurant input) {
            try {
                RestaurantEF oldRestaurant = await _context.Restaurants.Include(r => r.Tables).FirstOrDefaultAsync(r => r.RestaurantID == restaurantID);

                if(oldRestaurant == null) {
                    throw new RestaurantRepositoryException("No restaurant found to update. Insert the correct RestaurantID");
                }

                RestaurantEF updatedRestaurant = await RestaurantMapper.MapToData(input, _context);

                if(oldRestaurant.Equals(updatedRestaurant)) {
                    throw new RestaurantRepositoryException("Restaurants are the same. No update required.");
                }

                oldRestaurant.Name = input.Name;
                oldRestaurant.Cuisine = input.Cuisine;
                oldRestaurant.PhoneNumber = input.PhoneNumber;
                oldRestaurant.Email = input.Email;
                oldRestaurant.Municipality = input.Municipality;
                oldRestaurant.ZipCode = input.ZipCode;
                oldRestaurant.StreetName = input.StreetName;
                oldRestaurant.HouseNumberLabel = input.HouseNumberLabel;
                
                _context.Update(oldRestaurant);
                await SaveAndClearAsync();
                return RestaurantMapper.MapToDomain(oldRestaurant, _context);

            } catch (Exception) {
                throw;
            }
        }

        
    }
}

        

        