using RestaurantProject.Domain.Exceptions;
using RestaurantProject.Domain.Interfaces;
using RestaurantProject.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Managers {
    public class UserManager : IUserService{
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

        public async Task<User> CreateUserAsync(User user) {
            try {
                if (user == null) {
                    throw new UserManagerException("User can't be null. ");
                }

                User created = await _repo.CreateUserAsync(user);
                return created;
            } catch (Exception ex) {

                throw new UserManagerException("Error in CreateUserAsync: " + ex.Message);
            }
        }

        public async Task RemoveUserAsync(int userId) {
            try {
                if (userId <= 0) {
                    throw new UserManagerException("Id must be bigger than 0.");
                }

                await _repo.RemoveUserAsync(userId);

            } catch (Exception ex) {

                throw new UserManagerException("Error in RemoveUserAsync: " + ex.Message);
            }
        }

        public async Task<User> UpdateUserAsync(int userId, User domainUser) {
            try {
                if (userId <= 0) {
                    throw new UserManagerException("ID must be bigger than 0.");
                }

                if (domainUser == null) {
                    throw new UserManagerException("User can't be null");
                }               

                return await _repo.UpdateUserAsync(userId, domainUser);

            } catch (Exception ex) {

                throw new UserManagerException("Error in UpdateUserAsync: " + ex.Message);
            }
        }

        public async Task<bool> ExistingUser(string name) {
            try {
                return await _repo.ExistingUser(name);
            } catch (Exception ex) {

                throw new UserManagerException("Error in ExistingUser: " + ex.Message);
            }
        }
    }
}