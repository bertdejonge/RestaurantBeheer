﻿using RestaurantProject.Domain.Exceptions;
using RestaurantProject.Domain.Interfaces;
using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Managers {
    public class ReservationManager : IReservationService{

        private IReservationRepository _repo;

        public ReservationManager(IReservationRepository repo) {
            _repo = repo;
        }

        public async Task<bool> ExistingReservation(Reservation reservation) {
            return await _repo.ExistingReservation(reservation);
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

        public async Task<List<Reservation>> GetReservationsUserForDateOrRangeAsync(int userID, DateOnly date, DateOnly? optionalDate = null) {
            try {
                if (userID <= 0) {
                    throw new ReservationManagerException("UserID must be positive.");
                }

                if (date == null || date < DateOnly.FromDateTime(DateTime.Now.Date) || date < DateOnly.FromDateTime(DateTime.Now.Date.AddMonths(3))) {
                    throw new ReservationManagerException("Invalid date. Date must be between today and three months from now");
                }

                return await _repo.GetReservationsUserForDateOrRangeAsync(userID, date, optionalDate);

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


        public async Task<Reservation> CreateReservationAsync(Reservation reservation) {
            try {
                if (reservation == null) {
                    throw new ReservationManagerException("Restaurant can't be null. ");
                }

                return await _repo.CreateReservationAsync(reservation);
            } catch (Exception ex) {

                throw new ReservationManagerException("Error in CreateReservationAsync: " + ex);
            }
        }

        public async Task<Reservation> UpdateReservationAsync(int reservationID, Reservation domainReservation) {
            try {
                return await _repo.UpdateReservationAsync(reservationID, domainReservation);
            } catch (Exception ex) {

                throw new ReservationManagerException("Error is UpdateReservationAsync: " + ex.Message);
            }
        }

        public async Task<List<Reservation>> GetReservationsRestaurantForDateOrRangeAsync(int restaurantID, DateOnly date, DateOnly? optionalDate = null) {
            try {

                if (restaurantID <= 0) {
                    throw new ReservationManagerException("RestaurantID must be positive.");
                }

                if (date == null || date < DateOnly.FromDateTime(DateTime.Now.Date) || date < DateOnly.FromDateTime(DateTime.Now.Date.AddMonths(3))) {
                    throw new ReservationManagerException("Invalid date. Date must be between today and three months from now");
                }

                return await _repo.GetReservationsRestaurantForDateOrRangeAsync(restaurantID, date, optionalDate);
            } catch (Exception) {

                throw;
            }
        }
    }
}
