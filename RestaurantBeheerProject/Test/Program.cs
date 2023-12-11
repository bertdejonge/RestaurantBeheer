using RestaurantProject.Datalayer.Data;

namespace Test {
    public class Program {
        static void Main(string[] args) {
            RestaurantDbContext db = new RestaurantDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}