using RestaurantProject.Domain.Exceptions;
using RestaurantProject.Domain.Interfaces;
using RestaurantProject.Domain.Models;
using RestaurantProject.Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Managers {
    public class RestaurantManager : IRestaurantService {

        private IRestaurantRepository _repo;

        public RestaurantManager(IRestaurantRepository repo) {
            _repo = repo;
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id) {
            try {

                if (id <= 0) {
                    throw new RestaurantManagerException("Invalid id.");
                } else {
                    return await _repo.GetRestaurantByIdAsync(id);
                }
            } catch (Exception ex) {

                throw new RestaurantManagerException("Error in GetRestaurantByIdAsync: \n" + ex.Message);
            }
        }

        public async Task<List<Restaurant>> GetAvailableRestaurantsForDateAsync(DateOnly date, int partySize) {
            try {
                // Check if valid date
                if (date < DateOnly.FromDateTime(DateTime.Now.Date) || date > DateOnly.FromDateTime(DateTime.Now.Date.AddMonths(3))) {
                    throw new RestaurantManagerException("Invalid date. Date must be between now and 3 months from now");
                }

                if (partySize <= 0) {
                    throw new RestaurantManagerException("Invalid party size. Must be bigger than 0.");
                }

                return await _repo.GetAvailableRestaurantsForDateAsync(date, partySize);

            } catch (Exception ex) {
                throw new RestaurantManagerException("Error in GetAvailableRestaurantsForDateAsync: \n" + ex.Message);
            }
        }

        public async Task<List<Restaurant>> GetRestaurantsByZipAndCuisineAsync(int? zipCode = null, string? cuisine = null) {
            try {
                bool isValidZip = false;
                bool isValidCuisine = false;

                if (zipCode != null) {
                    if (zipCode >= 1000 && zipCode <= 9999) {
                        isValidZip = true;
                    }
                }

                if (cuisine != null) {
                    // Check if the given cuisine corresponds with a cuine type in the hardcoded list
                    if (CuisineType.IsInList(cuisine)) {
                        isValidCuisine = true;
                    }
                } 

                if (!isValidCuisine && !isValidZip) {
                    throw new RestaurantManagerException("Zipcode and/or cuisine has to be filled in.");
                } else {
                    return await _repo.GetRestaurantsByZipAndCuisineAsync(zipCode, cuisine);
                }

            } catch (Exception ex) {

                throw new RestaurantManagerException("Error in GetRestaurantsByZipAndCuisineAsync: \n" + ex.Message);
            }
        }

        public async Task<Restaurant> CreateRestaurantAsync(Restaurant domainRestaurant) {
            try {
                if (domainRestaurant == null) {
                    throw new RestaurantManagerException("Restaurant can't be null. ");
                }

                return await _repo.CreateRestaurantAsync(domainRestaurant);
            } catch (Exception ex) {
                throw new RestaurantManagerException("Error in CreateRestaurantAsync: \n" + ex.Message);
            }
        }

        public async Task RemoveRestaurantAsync(int restaurantID) {
            try {
                // First check the ID validity
                if (restaurantID <= 0) {
                    throw new RestaurantManagerException("Invalid id.");
                }

                await _repo.RemoveRestaurantAsync(restaurantID);
            } catch (Exception ex) {
                throw new RestaurantManagerException("Error in RemoveRestaurantAsync: \n" + ex.Message);
            }
        }

        public async Task<Restaurant> UpdateRestaurantAsync(int restaurantID, Restaurant domainRestaurant) {
            try {
                if (domainRestaurant == null) {
                    throw new RestaurantManagerException("The new restaurant can't be null");
                }

                return await _repo.UpdateRestaurantAsync(restaurantID, domainRestaurant);
            } catch(Exception ex) {
                throw new RestaurantManagerException("Error in UpdateRestaurantAsync: \n" + ex.Message);
            }
        }

        public async Task<bool> ExistingRestaurantAsync(Restaurant domainRestaurant) {
            return await _repo.ExistingRestaurantAsync(domainRestaurant.ZipCode, domainRestaurant.Name, domainRestaurant.Cuisine);
        }
    }
}
