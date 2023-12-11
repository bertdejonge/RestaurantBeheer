using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface IReservationRepository {

        void CreateReservation(Reservation reservation);

        Reservation GetReservation(int reservationID);

        // Takes 2 or 3 arguments
        // If 2, search for date
        // If 3, search in range
        Reservation GetReservationsRestaurantForDate(int restaurantID, DateTime date, DateTime? optionalDate = null);

        // Uur, Datum, aantal plaatsen
        void UpdateReservation(int userId, User updatedUser);

        // Enkel voor aankomende reservaties
        void CancelReservation(Reservation reservation);
    }
}
