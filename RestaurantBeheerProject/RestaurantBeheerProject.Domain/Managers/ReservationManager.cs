using RestaurantProject.Domain.Exceptions;
using RestaurantProject.Domain.Interfaces;
using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Managers {
    public class ReservationManager {

        private IReservationRepository _repo;

        public ReservationManager(IReservationRepository repo) {
            _repo = repo;
        }

        public async Task<Reservation> GetReservationByIDAsync(int reservationID) {
            try {
                if (reservationID <= 0) {
                    throw new ReservationManagerException("ID must be positive.");
                }

                return await _repo.GetReservationByIDAsync(reservationID);

            } catch (Exception ex) {

                throw new ReservationManagerException("Error in GetReservation: " + ex);
            }
        }

        public async Task<List<Reservation>> GetReservationsRestaurantForDateAsync(int restaurantID, DateOnly date, DateOnly? optionalDate = null) {
            try {
                if (restaurantID <= 0) {
                    throw new ReservationManagerException("RestaurantID must be positive.");
                }

                if (date == null || date < DateOnly.FromDateTime(DateTime.Now.Date) || date < DateOnly.FromDateTime(DateTime.Now.Date.AddMonths(3))) {
                    throw new ReservationManagerException("Invalid date. Date must be between today and three months from now");
                }

                return await _repo.GetReservationsRestaurantForDateAsync(restaurantID, date, optionalDate);

            } catch (Exception ex) {

                throw new ReservationManagerException("Error in GetReservationsRestaurantForDateAsync: " + ex);
            }
        }

        public async Task CancelReservationAsync(int reservationID) {
            try {
                if (reservationID <= 0) {
                    throw new ReservationManagerException("ID must be positive.");
                }

                await _repo.CancelReservationAsync(reservationID);
            } catch (Exception ex) {

                throw new ReservationManagerException("Error in CancelReservationAsync: " + ex.Message);
            }
        }


        public async Task CreateReservationAsync(Reservation reservation) {
            try {
                if (reservation == null) {
                    throw new ReservationManagerException("Restaurant can't be null. ");
                }

                await _repo.CreateReservationAsync(reservation);
            } catch (Exception ex) {

                throw new ReservationManagerException("Error in CreateReservationAsync: " + ex);
            }
        }

        public async Task UpdateReservationAsync(Reservation domainReservation) {
            try {
                await _repo.UpdateReservationAsync(domainReservation)
            } catch (Exception ex) {

                throw new ReservationManagerException("Error is UpdateReservationAsync: " + ex.Message);
            }
        }
    }
}
