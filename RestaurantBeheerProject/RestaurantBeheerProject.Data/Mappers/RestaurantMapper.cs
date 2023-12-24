using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantProject.Domain.Models;
using RestaurantProject.Datalayer.Data;
using Microsoft.EntityFrameworkCore;
using RestaurantProject.Datalayer.Models;
using RestaurantProject.Datalayer.Exceptions;

namespace RestaurantProject.Datalayer.Mappers {
    public class RestaurantMapper {
        public static Restaurant MapToDomain( RestaurantEF dataRestaurant, RestaurantDbContext context) {
            // Check if the data resto has resevations and tables, if not, assign new ones
            dataRestaurant.Tables ??= new List<TableEF>();

            List<Table> domainTables = new();            

            Restaurant domainResto = new(dataRestaurant.Name,
                                               dataRestaurant.Municipality,
                                               dataRestaurant.ZipCode,
                                               dataRestaurant.Cuisine,
                                               dataRestaurant.Email,
                                               dataRestaurant.PhoneNumber,
                                               domainTables);

            // Populate the domainTables
            foreach(TableEF table in dataRestaurant.Tables) { 
                domainResto.Tables.Add(TableMapper.MapToDomain(table, context)); 
            }

            if(!string.IsNullOrWhiteSpace(dataRestaurant.StreetName)) {
                domainResto.StreetName = dataRestaurant.StreetName;
            }

            if(!string.IsNullOrWhiteSpace(dataRestaurant.HouseNumberLabel)) {
                domainResto.HouseNumberLabel = dataRestaurant.HouseNumberLabel;
            }

            domainResto.RestaurantID = dataRestaurant.RestaurantID;

            return domainResto;
        }

        public async static Task<RestaurantEF> MapToData(Restaurant domainRestaurant, RestaurantDbContext context) {
            // ID =0 means the record hasn't been in the db, so set the values
            if (domainRestaurant.RestaurantID == 0) {
                RestaurantEF data = new();
                data.Name = domainRestaurant.Name;
                data.Cuisine = domainRestaurant.Cuisine;
                data.PhoneNumber = domainRestaurant.PhoneNumber;
                data.Email = domainRestaurant.Email;
                data.ZipCode = domainRestaurant.ZipCode;
                data.Municipality = domainRestaurant.Municipality;

                if (!string.IsNullOrWhiteSpace(domainRestaurant.StreetName)) {
                    data.StreetName = domainRestaurant.StreetName;
                }

                if (!string.IsNullOrWhiteSpace(domainRestaurant.HouseNumberLabel)) {
                    data.HouseNumberLabel = domainRestaurant.HouseNumberLabel;
                }

                return data;

            } else {
                var existingResto = await context.Restaurants
                                    .Include(r => r.Tables)
                                    .FirstOrDefaultAsync(r => r.RestaurantID == domainRestaurant.RestaurantID);

                if (existingResto == null) { 
                    throw new RestaurantMapperException($"No restaurant found with ID {domainRestaurant.RestaurantID}");
                }

                return existingResto;
            }
        }
    }
}
