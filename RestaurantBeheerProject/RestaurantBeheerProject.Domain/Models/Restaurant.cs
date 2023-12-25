using RestaurantProject.Domain.Exceptions;
using RestaurantProject.Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Models
{
    public class Restaurant {

        // Constructor with only municipality
        public Restaurant(string name, string municipality, int zipCode, string cuisine, string email, string phoneNumber, List<Table> tables) {
            Name = name;
            Municipality = municipality;
            ZipCode = zipCode;
            Cuisine = cuisine;
            Email = email;
            PhoneNumber = phoneNumber;
            Tables = tables;
        }

        // + streetname
        public Restaurant(string name, string municipality, int zipCode, string cuisine, string email, string phoneNumber, List<Table> tables, string streetName) 
            :this(name, municipality, zipCode, cuisine, email, phoneNumber, tables){
            Name = name;
            Municipality = municipality;
            ZipCode = zipCode;
            Cuisine = cuisine;
            Email = email;
            PhoneNumber = phoneNumber;
            Tables = tables;
            StreetName = streetName;
        }

        // +  housenumberlabel
        public Restaurant(string name, string municipality, int zipCode, string cuisine, string email, string phoneNumber, List<Table> tables, string streetName, string houseNumberLabel) 
            : this(name, municipality, zipCode, cuisine, email, phoneNumber, tables, streetName){
            Name = name;
            Municipality = municipality;
            ZipCode = zipCode;
            Cuisine = cuisine;
            Email = email;
            PhoneNumber = phoneNumber;
            Tables = tables;            
            StreetName = streetName;
            HouseNumberLabel = houseNumberLabel;
        }

        // + ID
        public Restaurant(string name, string municipality, string cuisine, string email, string phoneNumber, List<Table> tables, int zipCode, string streetName, string houseNumberLabel, int id)
            : this(name, municipality, zipCode, cuisine, email, phoneNumber, tables, streetName, houseNumberLabel) {
            Name = name;
            Municipality = municipality;
            ZipCode = zipCode;
            Cuisine = cuisine;
            Email = email;
            PhoneNumber = phoneNumber;
            Tables = tables;
            StreetName = streetName;
            HouseNumberLabel = houseNumberLabel;
            RestaurantID = id;
        }

        private int _restaurantID;
        public int RestaurantID {
            get { return _restaurantID; }
            set {
                if (value > 0) {
                    _restaurantID = value;
                }
            }
        }


        private string _name;
        public string Name {
            get { return _name; }
            set { if(!string.IsNullOrWhiteSpace(value)) {
                    _name = value;
                } else {
                    throw new RestaurantException("Invalid name. Please choose a name that contains characters other than whitespace.");
                }
            }
        }

        // LOCATION
        private int _zipCode;
        public int ZipCode {
            get { return _zipCode; }
            set {
                if (value > 0 && value < 9999) {
                    _zipCode = value;
                } else {
                    throw new RestaurantException("Invalid zip code. Please insert a valid zipcode between 0 and 9999.");
                }
            }
        }

        
        private string _municipality;
        public string Municipality {
            get { return _municipality; }
            set {
                if (!string.IsNullOrWhiteSpace(value)) {
                    _municipality = value;
                } else {
                    throw new RestaurantException("Invalid municipality. Please insert a valid municipality name.");
                }
            }
        }

        private string _streetName;
        public string StreetName {
            get { return _streetName; }
            set {
                if (!string.IsNullOrWhiteSpace(value)) {
                    _streetName = value;
                } else {
                    throw new RestaurantException("Invalid street name");
                }
            }
        }

        private string _houseNumberLabel;

        public string HouseNumberLabel {
            get { return _houseNumberLabel; }
            set {
                if (string.IsNullOrWhiteSpace(value)) { }
                _houseNumberLabel = value;
            }
        }

        private string _cuisine;
        public string Cuisine {
            get { return _cuisine; }
            set { if (CuisineType.IsInList(value)) {            // Check the type of cuisine a list of options
                    _cuisine = value;
                } else {
                    throw new RestaurantException("No cuisine found. Please give another cuisine");
                }
            }
        }

        private string _email;
        public string Email {
            get { return _email; }
            set {
                if (value.Contains('@')) {
                    if (CheckProperty.CheckEmail(value)) {
                        _email = value;
                    } else {
                        throw new RestaurantException("Invalid email. \"The email address is not in the correct format \\\"john@example.com\\\"\\\"\"");
                    }
                }else {
                    throw new RestaurantException("Invalid email. The email address has to contain an '@'");
                }
            }
        }

        private string _phoneNumber;

        public string PhoneNumber {
            get { return _phoneNumber; }
            set { if (CheckProperty.CheckPhoneNumber(value)) {
                    _phoneNumber = value;
                } else {
                    throw new RestaurantException("Invalid phone number. Please enter a phone number that only contains numbers.");
                }
            }
        }
        

        public List<Table> Tables { get; set; }

        public Table ChooseBestTable(int requiredSeats, DateOnly date, TimeOnly reservationTime) {
            // Get the available tables that have enough seats for the party
            // Order them so that the tables with the least ammount of seats come first 
            List<Table> availableTables = Tables.Where(t => t.Seats >= requiredSeats && t.IsAvailableForStartTime(date, reservationTime))
                                                .OrderBy(t => t.Seats).OrderBy(t => t.TableNumber)                                                              
                                                .ToList();
            if (availableTables.Count > 0) {
                availableTables[0].RemoveTakenTimeForDate(date, reservationTime);
                return availableTables[0];
            } else {
                throw new ReservationException("No available tables for the given reservation time. Please try another time "); 
            }
        }

        // CHeck if there is any table available for a given date
        // If any table has an available reservationHour: return true
        // Else return false
        public bool IsAnyTableAvailableForDate(DateOnly date, int partySize) {
            foreach(Table table in Tables.Where(t => t.Seats >= partySize)) {
                if (table.DateToReservationHours[date].Count > 0) {
                    return true;
                }
            } return false;
        }       

        public override string? ToString() {
            bool isValidStreet = string.IsNullOrWhiteSpace(StreetName);
            bool isValidHouseNumber = string.IsNullOrWhiteSpace(HouseNumberLabel);

            if (!isValidStreet && !isValidHouseNumber) {
                return $"{Name}, {Cuisine} cuisine, in {Municipality}({ZipCode}). " +
                       $"ContactInfo: {Email}, {PhoneNumber}";
            } else if(isValidStreet) {
                return $"{Name}, {Cuisine} cuisine, at {StreetName} ({Municipality}, {ZipCode}). " +
                       $"ContactInfo: {Email}, {PhoneNumber}";
            } else if(isValidStreet && isValidHouseNumber) {
                return $"{Name}, {Cuisine} cuisine, at {StreetName} {HouseNumberLabel} ({Municipality}, {ZipCode}). " +
                       $"ContactInfo: {Email}, {PhoneNumber}";
            }else {
                return $"{Name}, {Cuisine} cuisine, in {Municipality}, {ZipCode}). " +
                       $"ContactInfo: {Email}, {PhoneNumber}";
            }
        }



        // Class diagram code
        public Table Table {
            get => default;
            set {
            }
        }
    }
}
