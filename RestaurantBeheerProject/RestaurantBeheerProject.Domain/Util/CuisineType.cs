using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Util {
    public static class CuisineType {
        public static bool IsInList(string cuisine) {
            List<string> cuisineOptions = new List<string>() {
            "French",
            "Italian",
            "Greek",
            "Chinese",
            "Japanese",
            "Mexican",
            "Indian",
            "Thai",
            "Spanish",
            "American",
            "Brazilian",
            "Turkish",
            "Lebanese",
            "Korean",
            "Vietnamese",
            "Moroccan",
            "Peruvian",
            "Russian",
            "Belgian",
            "Dutch",
            "German",
            "British",
            "Fusion", // For fusion cuisines that combine multiple culinary traditions
            "Other" // For any other cuisine type not listed
            };

            // search case-insensitive
            if(cuisineOptions.Contains(cuisine, StringComparer.OrdinalIgnoreCase)) {
                return true;
            } else { 
                return false; 
            }

        }

    }

}
