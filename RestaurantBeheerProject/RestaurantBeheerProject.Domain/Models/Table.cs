using RestaurantProject.Domain.Exceptions;
using RestaurantProject.Domain.Util;

namespace RestaurantProject.Domain.Models {
    public class Table {

        public Table(int tableNumber, int seats) {
            TableNumber = tableNumber;
            Seats = seats;
			CollectionPopulator.PopulateAvailableHours(TableReservationHours);
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

		public List<TimeOnly> TableReservationHours = new List<TimeOnly>();


		// Check if the given reservation time is still available
		// If it is still available, remove it from the list to avoid double bookings
		// Furthermore, remove the available slots so that the table is reserved for 1,5 hours
        public bool IsAvailable(TimeOnly reservationTime) {
			if(reservationTime <= new TimeOnly(22,0) ||  reservationTime >= new TimeOnly(17, 30)) {
                if (TableReservationHours.Contains(reservationTime)) {
                    return true;
                } else {
                    return false;
                }
            } else {
				throw new TableException("Invalid reservation time.");
			}			
		}

		public void RemoveTakenTime(TimeOnly reservationTime) {

            // Remove the time from the list to avoid double bookings
            TableReservationHours.Remove(reservationTime);

            // Remove the available slots so that the table is reserved for 1,5 hours
            for (int i = 0; i < 2; i++) {
				reservationTime = reservationTime.AddMinutes(30);
                TableReservationHours.Remove(reservationTime);

            }
        }
    }
}