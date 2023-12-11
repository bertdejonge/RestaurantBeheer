using RestaurantProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Interfaces {
    public interface IUserRepository {
        void CreateUser(User user);
        User GetUser(int id);
        void UpdateUser(int userId, User updatedUser);
        void RemoveUser(int userId);
    }
}
