﻿using RestaurantProject.Domain.Exceptions;
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

                throw new RestaurantManagerException("Error in GetRestaurantByIdAsync: " + ex.Message);
            }
        }

        public async Task<List<Restaurant>> GetAvailableRestaurantsForDateAsync(DateOnly date, int partySize) {
            try {
                // Check if valid date
                if (date < DateOnly.FromDateTime(DateTime.Now.Date) || date < DateOnly.FromDateTime(DateTime.Now.Date.AddMonths(3)) || partySize <= 0) {
                    throw new RestaurantManagerException("Invalid date. ");
                }

                if (partySize <= 0) {
                    throw new RestaurantManagerException("Invalid party size.");
                }

                return await _repo.GetAvailableRestaurantsForDateAsync(date, partySize);

            } catch (Exception ex) {
                throw new RestaurantManagerException("Error in GetAvailableRestaurantsForDateAsync: " + ex.Message);
            }
        }

        public async Task<List<Restaurant>> GetRestaurantsByZipAndCuisineAsync(int? zipCode = null, string? cuisine = null) {
            try {
                bool isValidZip = false;
                bool isValidCuisine = false;

                if (zipCode >= 1000 && zipCode <= 9999) {
                    isValidZip = true;
                }

                // Check if the given cuisine corresponds with a cuine type in the hardcoded list
                if (CuisineType.IsInList(cuisine)) {
                    isValidCuisine = true;
                }

                if (!isValidCuisine && !isValidZip) {
                    throw new RestaurantManagerException("Zipcode and/or cuisine has to be filled in.");
                } else {
                    return await _repo.GetRestaurantsByZipAndCuisineAsync(zipCode, cuisine);
                }

            } catch (Exception ex) {

                throw new RestaurantManagerException("Error in GetRestaurantsByZipAndCuisineAsync: " + ex.Message);
            }
        }

        public async Task CreateRestaurantAsync(Restaurant domainRestaurant) {
            try {
                if (domainRestaurant == null) {
                    throw new RestaurantManagerException("Restaurant can't be null. ");
                }

                await _repo.CreateRestaurantAsync(domainRestaurant);
            } catch (Exception ex) {
                throw new RestaurantManagerException("Error in CreateRestaurantAsync: " + ex.Message);
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
                throw new RestaurantManagerException("Error is RemoveREstaurantAsync" + ex.Message);
            }
        }

        public async Task UpdateRestaurantAsync(Restaurant domainRestaurant) {
            try {
                if (domainRestaurant == null) {
                    throw new RestaurantManagerException("The new restaurant can't be null");
                }

                await _repo.UpdateRestaurantAsync(domainRestaurant);
            } catch(Exception ex) {
                throw new RestaurantManagerException("Error is UpdateRestaurantAsync " + ex.Message);
            }
        } 
    }
}