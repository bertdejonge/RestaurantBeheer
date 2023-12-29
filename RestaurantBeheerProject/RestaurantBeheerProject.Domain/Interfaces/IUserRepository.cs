using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface IUserRepository {
        Task<bool> ExistingUser(string name);
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(int userID, User updatedUser);
        Task RemoveUserAsync(int userId);
    }
}
