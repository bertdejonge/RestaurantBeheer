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

        public async Task<bool> ExistingUser(string phoneNumber, string email) {
            return await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber || u.Email == email);
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

        public async Task<User> CreateUserAsync(User user) {
            try {
                if (await ExistingUser(user.PhoneNumber, user.Email)) {
                    throw new UserRepositoryException($"A user with phone number {user.PhoneNumber} or email {user.Email} already exists");
                }
                var dataUser = await UserMapper.MapToData(user, _context);
                _context.Add(dataUser);
                await SaveAndClearAsync();
                return UserMapper.MapToDomain(dataUser);
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

        public async Task<User> UpdateUserAsync(int userId, User input) {
            try {
                UserEF oldUser = _context.Users.FirstOrDefault(u => u.UserID == userId);

                if(oldUser == null) {
                    throw new UserRepositoryException("User doesn't exist and thus can't be updated");
                }

                UserEF updatedUser = await UserMapper.MapToData(input, _context);

                // Equals overridden
                if(oldUser.Equals(updatedUser)) {
                    throw new UserRepositoryException("Users are the same. No update required. ");
                }

                oldUser.Name = updatedUser.Name;
                oldUser.Email = updatedUser.Email;
                oldUser.PhoneNumber = updatedUser.PhoneNumber;
                oldUser.Zipcode = updatedUser.Zipcode;
                oldUser.Municipality = updatedUser.Municipality;
                oldUser.StreetName = updatedUser.StreetName;
                oldUser.HouseNumberLabel = updatedUser.HouseNumberLabel;
                
                _context.Update(oldUser);
                await SaveAndClearAsync();
                return UserMapper.MapToDomain(oldUser);

            } catch (Exception) {

                throw;
            }
        }
    }
}
