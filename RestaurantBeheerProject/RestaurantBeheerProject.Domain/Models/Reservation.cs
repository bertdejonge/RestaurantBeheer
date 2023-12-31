﻿using RestaurantProject.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Models {
    public class Reservation {
        // When making a reservation the restaurant and user and partysize get set
        // Then we check if there is at least one spot available for the given dat
        // if true, then set the date to that date 
        // Set the starttime to the desired timeslot date
        // Finally, choose the best table for a given partysize, date and starttime 
        public Reservation(Restaurant restaurant, User user, int partySize, DateOnly date, TimeOnly startTime) {
            Restaurant = restaurant;
            User = user;
            PartySize = partySize;
            Date = date;
            StartTime = startTime;
            SetEndTime();
        }
                
        // ReservationId will be set once the reservation is inserted into the database
        public int ReservationId { get; private set; }

        public void SetReservationId(int reservationId) {
            if(reservationId > 0) {
                ReservationId = reservationId;
            }
        }

        private Restaurant _restaurant;
        public Restaurant Restaurant {
            get { return _restaurant; }
            set {
                if (value != null) {
                    _restaurant = value;
                } else {
                    throw new ReservationException("Invalid restaurant info. Please insert correct restaurant info");
                }
            }
        }

        private User _user;

        public User User {
            get { return _user; }
            set {
                if (value != null) {
                    _user = value;
                }
            }
        }        


        private int _partySize;
        public int PartySize {
            get { return _partySize; }
            set { if (value > 0) {
                    _partySize = value;
                } else {
                    throw new ReservationException("Invalid partysize. Please select a positive ammount of attendees.");
                }
            } 
        }

        private DateOnly _date;
        public DateOnly Date {
            get { return _date; }
            set { if(value >= DateOnly.FromDateTime(DateTime.Now.Date) && value < DateOnly.FromDateTime(DateTime.Now.AddMonths(3))) {
                    _date = value;
                } else {
                    throw new ReservationException("Invalid Date. Please select a data between today and three months from now.");
                }

            }
        }

        // Check controleren: in principe al gecontroleerd door een lijst met available hours te geven voor een bepaald restaurant
        private TimeOnly _startTime;
        public TimeOnly StartTime {
            get { return _startTime; }                          
            set { if ((value >= new TimeOnly(17, 00) && value < new TimeOnly(22, 00)) && (value.Minute == 0 || value.Minute == 30) && value > TimeOnly.FromDateTime(DateTime.Now)) {
                    _startTime = value; 
                } else {
                    throw new ReservationException($"Invalid starttime. Starttime must be later than now ({DateTime.Now.TimeOfDay.ToString("HH:mm")}) and between 17h00 and 22h00.");
                }
            }
        }

        public TimeOnly EndTime { get; private set; }
        private void SetEndTime() {
            EndTime = StartTime.AddHours(1).AddMinutes(30);
        }

        private int _tableNumber;
        public int TableNumber {
            get { return _tableNumber; }
            set {
                if (value > 0) {
                    _tableNumber = value;
                } else {
                    throw new ReservationException("Invalid tablenumber. Please select a number greater than 0.");
                }
            }
        }

        public override string? ToString() {
            return $"Reservation at {StartTime} for table No.{TableNumber}";
        }

        public bool Equals(Reservation reservation) {
            if(this.Date == reservation.Date && this.User.Equals(reservation.User) 
                && this.Restaurant.Equals(reservation.Restaurant) && this.PartySize == reservation.PartySize 
                && this.StartTime == reservation.StartTime) {
                return true;
            } return false;
        }

        public Restaurant Restaurant1 {
            get => default;
            set {
            }
        }

        public User User1 {
            get => default;
            set {
            }
        }
    }
}
