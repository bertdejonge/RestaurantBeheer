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
        public Restaurant(string name, string municipality, string cuisine, string email, string phoneNumber, List<Reservation> reservations, List<Table> tables) {
            Name = name;
            Municipality = municipality;
            Cuisine = cuisine;
            Email = email;
            PhoneNumber = phoneNumber;
            Reservations = reservations;
            Tables = tables;
        }

        // municipality & zipcode
        public Restaurant(string name, string municipality, string cuisine, string email, string phoneNumber, List<Reservation> reservations, List<Table> tables, int zipcode) 
            :this(name, municipality, cuisine, email, phoneNumber, reservations, tables){
            Municipality = municipality;
            Cuisine = cuisine;
            Email = email;
            PhoneNumber = phoneNumber;
            Reservations = reservations;
            Tables = tables;
            ZipCode = zipcode;
        }

        // municipality, zipcode & streetname
        public Restaurant(string name, int zipCode, string municipality, string streetName,  string cuisine, string email, string phoneNumber, List<Reservation> reservations, List<Table> tables) 
            :this(name, municipality, cuisine, email, phoneNumber, reservations, tables){
            Name = name;
            ZipCode = zipCode;
            Municipality = municipality;
            StreetName = streetName;
            Cuisine = cuisine;
            Email = email;
            PhoneNumber = phoneNumber;
            Reservations = reservations;
            Tables = tables;
        }

        // municipality, zipcode, streetname & housenumberlabel
        public Restaurant(string name, int zipCode, string municipality, string streetName, string houseNumberLabel, string cuisine, string email, string phoneNumber, List<Reservation> reservations, List<Table> tables) 
            : this(name, municipality, cuisine, email,phoneNumber,reservations,tables){
            Name = name;
            ZipCode = zipCode;
            Municipality = municipality;
            StreetName = streetName;
            HouseNumberLabel = houseNumberLabel;
            Cuisine = cuisine;
            Email = email;
            PhoneNumber = phoneNumber;
            Reservations = reservations;
            Tables = tables;
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
                    throw new LocationException("Invalid zip code. Please insert a valid zipcode between 0 and 9999.");
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
                    throw new LocationException("Invalid municipality. Please insert a valid municipality name.");
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
                    throw new LocationException("Invalid street name");
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



        public List<Reservation> Reservations { get; set; }

        public List<Table> Tables { get; set; }

        public Table ChooseBestTable(int requiredSeats, DateTime date, TimeOnly reservationTime) {
            // Get the available tables that have enough seats for the party
            // Order them so that the tables with the least ammount of seats come first 
            List<Table> availableTables = Tables.Where(t => t.Seats >= requiredSeats && t.IsAvailable(date, reservationTime))
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
        public bool IsAnyTableAvailableForDate(DateTime date) {
            foreach(Table table in Tables) {
                if (table.DateToReservationHours[date].Count > 0) {
                    return true;
                }
            } return false;
        }



        // Class diagram code
        public Table Table {
            get => default;
            set {
            }
        }

        public Reservation Reservation {
            get => default;
            set {
            }
        }

        public Location Location1 {
            get => default;
            set {
            }
        }
    }
}
