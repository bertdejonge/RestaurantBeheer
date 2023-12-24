using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface IUserService {
        // GET
        Task<User> GetUserByIdAsync(int id);
        
        // POST
        Task<User> CreateUserAsync(User user);
        
        // PUT
        Task<User> UpdateUserAsync(int userID, User updatedUser);

        // DELETE
        Task RemoveUserAsync(int userId);
    }
}
