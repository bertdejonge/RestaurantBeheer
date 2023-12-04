using RestaurantProject.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RestaurantProject.Domain.Models {
    public static class CheckProperty {

        public static bool CheckPhoneNumber(string value) {
            foreach (char c in value) {
                bool succeeded = int.TryParse(c.ToString(), out _);
                if (!succeeded) {
                    return false;
                }
            }
            return true;
        }

        public static bool CheckEmail(string value) {
            // First check if input contains an @
            // If not, give error message
            if (value.Contains('@')) {

                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regexPattern = new(pattern);

                if(regexPattern.IsMatch(value)) {
                    return true;
                } else {
                    throw new CheckPropertyException("The email address is not in the correct format \"john@example.com\"\"");
                }
            } else {
                throw new CheckPropertyException("The e-mail address has to contain an @-sign.");
            }
        }
    }
}
