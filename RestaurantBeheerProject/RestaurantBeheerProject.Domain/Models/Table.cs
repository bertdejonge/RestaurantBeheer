using RestaurantProject.Domain.Exceptions;
using RestaurantProject.Domain.Util;
using System.Linq;

namespace RestaurantProject.Domain.Models {
    public class Table {

        public Table(int tableNumber, int seats) {
            TableNumber = tableNumber;
            Seats = seats;
			CollectionPopulator.PopulateDateAndHours(DateToReservationHours);
        }

		private int _tableID;

		public int TableID {
			get { return _tableID; }
			set {
				if (value > 0) {
					_tableID = value;
				} else {
                    throw new TableException("Invalid TableID. Please insert a number bigger than 0.");
                }
			}
		}


		private int _tableNumber;
		public int TableNumber {
			get { return _tableNumber; }
			set {
				if (value > 0) {
					_tableNumber = value;
				} else {
					throw new TableException("Invalid TableNumber. Please insert a number bigger than 0.");
				}
			}
		}

		private int _seats;

		public int Seats {
			get { return _seats; }
			set {
				if (value > 0) {
					_seats = value;
				} else {
					throw new TableException("Invalid ammount of seats. Please insert a number bigger than 0.");
				}
			}
		}

		public Dictionary<DateOnly, List<TimeOnly>> DateToReservationHours = new Dictionary<DateOnly, List<TimeOnly>>();


		// Check if the given reservation time is still available
		// If it is still available, check if the next 2 slots are available to avoid double bookings
		// Third slot is not needed, because users can only book every half hour, so the previous party has to leave before the next arrive 
		// F.e. booking at 17.00 needs slots 17.00, 17.30 and 18.00
		// 18.30 can still be booked for the next party
		public bool IsAvailableForStartTime(DateOnly date, TimeOnly reservationTime) {
			if (reservationTime < new TimeOnly(22, 0) || reservationTime >= new TimeOnly(17, 00)) {
				if (DateToReservationHours.TryGetValue(date, out List<TimeOnly> availableHours)) {
					if (availableHours.Contains(reservationTime) && availableHours.Contains(reservationTime.AddMinutes(30))
						&& availableHours.Contains(reservationTime.AddMinutes(30))) {
						return true;
					} else {
						return false;
					}
				} else {
					return false;
				}
			} else {
				throw new TableException("Invalid reservation time.");
			}
		}

		// Used when a reservation is made
		public void RemoveTakenTimeForDate(DateOnly date, TimeOnly reservationTime) {

			// Remove the time from the list to avoid double bookings
			DateToReservationHours[date].Remove(reservationTime);

            // Remove the next 2 slots so that the table is reserved for 1,5 hours
            for (int i = 0; i < 2; i++) {
				reservationTime = reservationTime.AddMinutes(30);
                DateToReservationHours[date].Remove(reservationTime);
            }
        }

		// Used whena reservation is cancelled or updated and the new times are 
		public void AddCancelledReservationTimeForDate(DateOnly date, TimeOnly reservationTime) {
			// Add the time where the original reservation would have taken place
			DateToReservationHours[date].Add(reservationTime);

            // Add the next slots where the table  would have been unavailable 
            for (int i = 0; i < 2; i++) {
                reservationTime = reservationTime.AddMinutes(30);
                DateToReservationHours[date].Add(reservationTime);
            }
        }
    }
}