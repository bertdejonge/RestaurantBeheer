using RestaurantProject.Domain.Interfaces;
using RestaurantProject.Domain.Models;
using RestaurantProject.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantProject.Datalayer.Exceptions;

namespace RestaurantProject.Domain.Managers {
    public class UserManager {
        private IUserRepository _repo;

        public UserManager(IUserRepository repo) {
            _repo = repo;
        }

        public async Task<User> GetUserByIdAsync(int id) {
            try {
                if (id <= 0) {
                    throw new UserManagerException("Id can't be 0.");
                }

                return await _repo.GetUserByIdAsync(id);

            } catch (Exception ex) {

                throw new UserManagerException("Error in GetUserByIdAsync " + ex.Message);
            }
        }

        public async Task CreateUserAsync(User user) {
            try {
                if (user == null) {
                    throw new UserManagerException("User can't be null. ");
                }

                await _repo.CreateUserAsync(user);
            } catch (Exception ex) {

                throw new UserManagerException("Error in CreateUserAsync: " + ex.Message);
            }
        }

        public async Task RemoveUserAsync(int userId) {
            try {
                if (userId <= 0) {
                    throw new UserManagerException("Id can't be 0.");
                }

                await _repo.RemoveUserAsync(userId);

            } catch (Exception ex) {

                throw new UserManagerException("Error in RemoveUserAsync: " + ex.Message);
            }
        }

        public async Task UpdateUserAsync(User domainUser) {
            try {
                if (domainUser == null) {
                    throw new UserManagerException("User can't be null");
                }

                await _repo.UpdateUserAsync(domainUser);

            } catch (Exception ex) {

                throw new UserManagerException("Error in UpdateUserAsync: " + ex.Message);
            }
        }
    }
}