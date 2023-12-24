using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Util {
    public static class CuisineType {
        public static bool IsInList(string cuisine) {
            List<string> cuisineOptions = new List<string>() {
            "french",
            "italian",
            "greek",
            "chinese",
            "japanese",
            "mexican",
            "indian",
            "thai",
            "spanish",
            "american",
            "brazilian",
            "turkish",
            "lebanese",
            "korean",
            "vietnamese",
            "moroccan",
            "peruvian",
            "russian",
            "belgian",
            "dutch",
            "german",
            "british",
            "fusion", // For fusion cuisines that combine multiple culinary traditions
            "other" // For any other cuisine type not listed
            };

            // search case-insensitive and remove whitespace
            cuisine = cuisine.ToLower().Trim();

            foreach(string option in cuisineOptions) {
                if(option.Contains(cuisine)) {
                    return true;
                } 
            }
            return false;

        }

    }

}
