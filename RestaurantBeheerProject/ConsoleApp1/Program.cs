using RestaurantProject.Datalayer.Data;

namespace ConsoleApp1 {
    public class Program {
        static void Main(string[] args) {
            RestaurantDbContext context = new();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}