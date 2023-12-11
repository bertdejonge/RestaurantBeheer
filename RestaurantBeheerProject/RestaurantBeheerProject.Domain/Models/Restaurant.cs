using RestaurantProject.Domain.Exceptions;
using RestaurantProject.Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Models
{
    public class Restaurant {

        public Restaurant(string name, Location location, CuisineType cuisine, string email, string phoneNumber, List<Reservation> reservations, List<Table> tables) {
            Name = name;
            Location = location;
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

        private Location _location;
        public Location Location {
            get { return _location; }
            set {
                if (value != null) {
                    _location = value;
                } else {
                    throw new RestaurantException("Invalid restaurant. Please give a valid restaurant.");
                }
            }
        }

        private CuisineType _cuisine;
        public CuisineType Cuisine {
            get { return _cuisine; }
            set { _cuisine = value; }
        }

        private string _email;
        public string Email {
            get { return _email; }
            set {
                try {
                    if (!string.IsNullOrWhiteSpace(value) && CheckProperty.CheckEmail(value)) {
                        _email = value;
                    } else {
                        throw new UserException(" Please enter an email that contains characters other than whitespace.");
                    }
                } catch(Exception ex) {
                    throw new UserException($"Invalid email. {ex.Message}");
                }
            }
        }

        private string _phoneNumber;
        public string PhoneNumber {
            get { return _phoneNumber; }
            set {
                try {
                    if (!string.IsNullOrEmpty(value) && CheckProperty.CheckPhoneNumber(value)) {
                        _phoneNumber = value;
                    } else {
                        throw new UserException("Phone number should only contain numbers");
                    }
                } catch (Exception ex) {
                    throw new UserException($"Invalid phone number. {ex.Message}");
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
