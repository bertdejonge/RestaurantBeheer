using Microsoft.EntityFrameworkCore;
using RestaurantProject.Datalayer.Data;
using RestaurantProject.Datalayer.Exceptions;
using RestaurantProject.Datalayer.Mappers;
using RestaurantProject.Datalayer.Models;
using RestaurantProject.Domain.Interfaces;
using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Datalayer.Repositories {
    public class UserRepository : IUserRepository {
        private readonly RestaurantDbContext _context;

        public UserRepository(RestaurantDbContext context) {
            _context = context ?? throw new ContextException(nameof(context));
        }

        // Reusable method as to not write double code
        private async Task SaveAndClearAsync() {
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }

        public async Task<bool> ExistingUser(string name, string phoneNumber, string email) {
            return await _context.Users.AnyAsync(u => u.Name == name && u.PhoneNumber == phoneNumber  && u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(int id) {
            try {
                var dataUser = await _context.Users.FirstOrDefaultAsync(u => u.UserID == id);

                if(dataUser == null) {
                    throw new UserRepositoryException($"No user found with ID {id}");
                }

                return UserMapper.MapToDomain(dataUser);

            } catch (Exception) {

                throw;
            }
        }

        public async Task CreateUserAsync(User user) {
            try {

                if (await ExistingUser(user.Name, user.PhoneNumber, user.Email)) {
                    throw new UserRepositoryException($"User with name {user.Name}, phonenumber {user.PhoneNumber} and email {user.Email} already exists.");
                }

                _context.Add(UserMapper.MapToData(user, _context));
                await SaveAndClearAsync();
            } catch (Exception) {

                throw;
            }            
        }        

        public async Task RemoveUserAsync(int userId) {
            try {
                var dataUser = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);

                if (dataUser == null) {
                    throw new UserRepositoryException($"User with id {userId} doesn't exist");
                }

                _context.Remove(dataUser);
                await SaveAndClearAsync();
            } catch (Exception) {

                throw;
            }
        }

        public async Task UpdateUserAsync(User domainUser) {
            try {
                UserEF updatedUser = UserMapper.MapToData(domainUser, _context);

                if (!await ExistingUser(updatedUser.Name, updatedUser.PhoneNumber, updatedUser.Email)) {
                    throw new UserRepositoryException("User doesn't exist and thus can't be updated");
                }

                var existingUser = _context.ChangeTracker.Entries<UserEF>()
                                   .FirstOrDefault(u => u.Entity.UserID == updatedUser.UserID);

                if(existingUser != null) {
                    existingUser.State = EntityState.Detached;
                }

                // Equals overridden
                if(updatedUser.Equals(existingUser)) {
                    throw new UserRepositoryException("Reservations are the same. No update required. ");
                }

                _context.Update(updatedUser);
                await SaveAndClearAsync();

            } catch (Exception) {

                throw;
            }
        }
    }
}
